using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Esri.ArcGISRuntime.Mapping;

namespace SharedMapView
{
    public static class ThemeResponsiveMapUtilities
    {
        public static readonly string ThemeLayerNameSuffix = "Theme: ";

        public static async Task<List<string>> ThemesFromMap(Map map)
        {
            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            await map.LoadAsync();

            var potentialThemeLayers = map.OperationalLayers.OfType<GroupLayer>().Where(layer => layer.Name.StartsWith(ThemeLayerNameSuffix));

            return potentialThemeLayers.Select(layer => layer.Name.Substring(ThemeLayerNameSuffix.Length)).ToList();
        }

        /// <summary>
        /// Note: this is not aware of system theme or user preference; only supply a theme-aware map and a theme that is known to be in the map.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="selectedTheme">A theme that is in the input map; do not provide 'Automatic' as this method is not aware of system theme or use preference.</param>
        /// <returns></returns>
        public static async Task<Map> ThemedSubsetFromThemeAwareMap(Map map, string selectedTheme)
        {
            if (map == null || string.IsNullOrEmpty(selectedTheme))
            {
                throw new ArgumentException("null map or null/empty theme selection");
            }

            await map.RetryLoadAsync();

            // Clone the map (keeps other properties)
            var subsetMap = map.Clone();

            // Remember identity of target group layer
            GroupLayer targetThemeLayer = null;

            // Remove all theme group layers
            foreach(var themeLayer in subsetMap.OperationalLayers.OfType<GroupLayer>().Where(layer => layer.Name.StartsWith(ThemeLayerNameSuffix)).ToList())
            {
                // TODO - should these be case-sensitive comparisons?
                if (themeLayer.Name.Substring(ThemeLayerNameSuffix.Length) != selectedTheme)
                {
                    subsetMap.OperationalLayers.Remove(themeLayer);
                }
                else
                {
                    targetThemeLayer = themeLayer;
                }
            }

            if (targetThemeLayer == null)
            {
                throw new ArgumentException("Input map does not contain selected theme");
            }

            // Remember new position of theme layer
            int layerPositionIndex = subsetMap.OperationalLayers.IndexOf(targetThemeLayer);

            subsetMap.OperationalLayers.Remove(targetThemeLayer);

            foreach(var childLayer in targetThemeLayer.Layers.Reverse())
            {
                subsetMap.OperationalLayers.Insert(layerPositionIndex, childLayer.Clone());
            }

            return subsetMap;
        }
    }
}
