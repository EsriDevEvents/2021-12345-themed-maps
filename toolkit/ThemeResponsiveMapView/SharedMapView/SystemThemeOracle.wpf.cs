using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management;
using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace SharedMapView
{
    public class SystemThemeOracle : IThemeOracle
    {
        private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string RegistryValueName = "AppsUseLightTheme";
        private const string ManagementQuery = @"SELECT * FROM RegistryValueChangeEvent WHERE Hive = 'HKEY_USERS' AND KeyPath = '{0}\\{1}' AND ValueName = '{2}'";

        private ManagementEventWatcher _watcher;

        private Dispatcher _dispatcher;

        public SystemThemeOracle(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            // Set up listener
            WatchTheme();
        }

        public event EventHandler<EventArgs> SystemThemeChanged;

        public string GetCurrentSystemTheme()
        {
            try
            {
                if (SystemParameters.HighContrast)
                {
                    return "High Contrast";
                }
                using RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath);
                object registryValueObject = key?.GetValue(RegistryValueName);
                if (registryValueObject == null)
                {
                    return "Light";
                }

                int registryValue = (int)registryValueObject;

                return registryValue > 0 ? "Light" : "Dark";
            }
            catch (Exception)
            {
                return "Light";
            }
        }

        // Adapted from: https://engy.us/blog/2018/10/20/dark-theme-in-wpf/
        private void WatchTheme()
        {
            var currentUser = WindowsIdentity.GetCurrent();
            string query = string.Format(
                CultureInfo.InvariantCulture,
                ManagementQuery,
                currentUser.User.Value,
                RegistryKeyPath.Replace(@"\", @"\\"),
                RegistryValueName);

            try
            {
                var watcher = new ManagementEventWatcher(query);
                watcher.EventArrived += (o, e) => _dispatcher.Invoke(() => SystemThemeChanged?.Invoke(this, null));

                // Start listening for events
                watcher.Start();
            }
            catch (Exception)
            {
                // This can fail on Windows 7
            }

            SystemParameters.StaticPropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(SystemParameters.HighContrast))
                {
                    SystemThemeChanged?.Invoke(this, new EventArgs());
                }
            };
        }

        public void ManuallyTriggerThemeEvent()
        {
            SystemThemeChanged?.Invoke(this, new EventArgs());
        }
    }
}
