using Esri.ArcGISRuntime.UI.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedMapView
{
    public partial class ThemeResponsiveMapView : MapView
    {
        public ThemeResponsiveMapView(Android.Content.Context context) : base(context)
        {
            Initialize(new SystemThemeOracle(context));
        }

        public ThemeResponsiveMapView(Android.Content.Context context, Android.Util.IAttributeSet attrs) : base(context)
        {
            Initialize(new SystemThemeOracle(context));
        }
    }
}
