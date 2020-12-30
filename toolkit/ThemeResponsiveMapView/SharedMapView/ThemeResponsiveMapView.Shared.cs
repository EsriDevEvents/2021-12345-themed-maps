using Esri.ArcGISRuntime.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedMapView
{
    public partial class ThemeResponsiveMapView
    {
        private Map _subsetMap;
        private Map _userSuppliedMap;

        private IThemeOracle _themeOracle;

        private void Initialize(IThemeOracle oracle)
        {
            _themeOracle = oracle;
            SelectedTheme = "Automatic";
            FallbackTheme = "Light";

            _themeOracle.SystemThemeChanged += _themeOracle_SystemThemeChanged;
        }

        private async void _themeOracle_SystemThemeChanged(object sender, EventArgs e)
        {
           await BuildAndApplySubsetMap();
        }

        /// <summary>
        /// To be called after setting _userSuppliedMap
        /// </summary>
        private async Task BuildAndApplySubsetMap()
        {
            // Clear old map
            base.Map = null;
            // update list of available themes
            AvailableThemes = await ThemeResponsiveMapUtilities.ThemesFromMap(_userSuppliedMap);
            var systemTheme = _themeOracle.GetCurrentSystemTheme();
            // Try to build map for selected theme
            if (SelectedTheme == "Automatic" && AvailableThemes.Contains(systemTheme))
            {
                _subsetMap = await ThemeResponsiveMapUtilities.ThemedSubsetFromThemeAwareMap(_userSuppliedMap, systemTheme);
            }
            else if (AvailableThemes.Contains(SelectedTheme))
            {
                _subsetMap = await ThemeResponsiveMapUtilities.ThemedSubsetFromThemeAwareMap(_userSuppliedMap, SelectedTheme);
            }
            // Fall back to fallback theme
            else if (AvailableThemes.Contains(FallbackTheme))
            {
                _subsetMap = await ThemeResponsiveMapUtilities.ThemedSubsetFromThemeAwareMap(_userSuppliedMap, FallbackTheme);
            }
            // Fall back to first theme
            else if (AvailableThemes.Any())
            {
                _subsetMap = await ThemeResponsiveMapUtilities.ThemedSubsetFromThemeAwareMap(_userSuppliedMap, AvailableThemes.First());
            }
            // Fall back to input map
            else
            {
                _subsetMap = _userSuppliedMap;
            }
            // update map
            base.Map = _subsetMap;
        }
    }
}
