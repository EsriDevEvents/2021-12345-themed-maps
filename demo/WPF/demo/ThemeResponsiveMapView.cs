using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace demo
{
    public class ThemeResponsiveMapView : MapView
    {
        private Dictionary<string, GroupLayer> AvailableThemes = new Dictionary<string, GroupLayer>();
        private const string ThemePrefix = "Theme: ";

        private Map _originalMap;
        private Map _subsetMap;

        public ThemeResponsiveMapView() : base()
        {
            EnableAutomaticTheming = true;
        }

        public new Map Map
        {
            get => base.Map;
            set
            {
                _originalMap = value;
                ApplyMap(value);
            }
        }

        private async void ApplyMap(Map inputMap)
        {
            try
            {
                await inputMap.RetryLoadAsync();
                AvailableThemes.Clear();

                foreach (var theme in inputMap.OperationalLayers.OfType<GroupLayer>().Where(layer => layer.Name.StartsWith(ThemePrefix)))
                {
                    AvailableThemes[theme.Name.Substring(ThemePrefix.Length)] = theme;
                }

                if (EnableAutomaticTheming)
                {
                    ChooseThemeFromSystem();
                }

                if (!AvailableThemes.ContainsKey(CurrentlySelectedTheme))
                {
                    _subsetMap = null;
                    _originalMap = inputMap;
                    base.Map = inputMap;
                    return;
                }

                _subsetMap = new Map();
                // ArcGIS Pro forces you to publish with a basemap, so in most cases you can expect it is unwanted
                //_subsetMap.Basemap = inputMap.Basemap.Clone();
                _subsetMap.OperationalLayers.AddRange(inputMap.OperationalLayers.Where(layer => layer.Name == $"{ThemePrefix}{CurrentlySelectedTheme}").Select(layer => layer.Clone()));

                base.Map = _subsetMap;
            }
            catch (Exception)
            {
                // Ignore
            }
        }

        private void ReApplyTheme()
        {
            if (_originalMap == null)
            {
                return;
            }
            _subsetMap = new Map();
            _subsetMap.OperationalLayers.AddRange(_originalMap.OperationalLayers.Where(layer => layer.Name == $"{ThemePrefix}{CurrentlySelectedTheme}").Select(layer => layer.Clone()));
            base.Map = _subsetMap;
        }

        public bool EnableAutomaticTheming
        {
            get { return (bool)GetValue(EnableAutomaticThemingProperty); }
            set { SetValue(EnableAutomaticThemingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EnableAutomaticTheming.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableAutomaticThemingProperty =
            DependencyProperty.Register("EnableAutomaticTheming", typeof(bool), typeof(ThemeResponsiveMapView), new PropertyMetadata(true));

        public string CurrentlySelectedTheme
        {
            get { return (string)GetValue(CurrentlySelectedThemeProperty); }
            set { SetValue(CurrentlySelectedThemeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentlySelectedTheme.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentlySelectedThemeProperty =
            DependencyProperty.Register("CurrentlySelectedTheme", typeof(string), typeof(ThemeResponsiveMapView), new PropertyMetadata(null, ThemeSelectionChanged));

        private static void ThemeSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                (d as ThemeResponsiveMapView).ChooseThemeFromSystem();
            }
            (d as ThemeResponsiveMapView).ReApplyTheme();
        }
        private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

        private const string RegistryValueName = "AppsUseLightTheme";

        private void ChooseThemeFromSystem()
        {
            // TODO 
            if (SystemParameters.HighContrast && AvailableThemes.ContainsKey("High Contrast"))
            {
                CurrentlySelectedTheme = "High Contrast";
                return;
            }
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
            {
                object registryValueObject = key?.GetValue(RegistryValueName);
                if (registryValueObject == null)
                {
                    CurrentlySelectedTheme = "Light";
                    return;
                }

                int registryValue = (int)registryValueObject;

                CurrentlySelectedTheme = registryValue > 0 ? "Light" : "Dark";
                return;
            }
            CurrentlySelectedTheme = "Light";
        }
    }
}
