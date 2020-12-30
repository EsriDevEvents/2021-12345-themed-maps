using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;

namespace SharedMapView
{
    public partial class ThemeResponsiveMapView : MapView
    {
        public ThemeResponsiveMapView() : base()
        {
            Initialize(new SystemThemeOracle(this.Dispatcher));
        }

        public new Map ResponsiveMap
        {
            get => (Map)GetValue(ResponsiveMapProperty);
            set => SetValue(ResponsiveMapProperty, value);
        }

        public static new readonly DependencyProperty ResponsiveMapProperty =
            DependencyProperty.Register(nameof(ResponsiveMap), typeof(Map), typeof(ThemeResponsiveMapView),
            new PropertyMetadata(OnMapChanged));

        public static void OnMapChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is ThemeResponsiveMapView realMapView && args.Property == ResponsiveMapProperty)
            {
                realMapView._userSuppliedMap = args.NewValue as Map;

                _ = realMapView.BuildAndApplySubsetMap();
            }
        }

        public IList<string> AvailableThemes
        {
            get => (IList<string>)GetValue(AvailableThemesProperty.DependencyProperty);
            private set => SetValue(AvailableThemesProperty, value);
        }

        private static readonly DependencyPropertyKey AvailableThemesProperty =
            DependencyProperty.RegisterReadOnly(nameof(AvailableThemes), typeof(IList<string>), typeof(ThemeResponsiveMapView), new PropertyMetadata(null));

        public string SelectedTheme
        {
            get => (string)GetValue(SelectedThemeProperty);
            set => SetValue(SelectedThemeProperty, value);
        }

        public static readonly DependencyProperty SelectedThemeProperty =
            DependencyProperty.Register(nameof(SelectedTheme), typeof(string), typeof(ThemeResponsiveMapView),
            new PropertyMetadata("Automatic", SelectedThemeChanged));

        public static void SelectedThemeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is ThemeResponsiveMapView extentedMapView)
            {
                _ = extentedMapView.BuildAndApplySubsetMap();
            }
        }

        public string FallbackTheme
        {
            get { return (string)GetValue(FallbackThemeProperty); }
            set { SetValue(FallbackThemeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FallbackTheme.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FallbackThemeProperty =
            DependencyProperty.Register(nameof(FallbackTheme), typeof(string), typeof(ThemeResponsiveMapView), new PropertyMetadata(null));
    }
}
