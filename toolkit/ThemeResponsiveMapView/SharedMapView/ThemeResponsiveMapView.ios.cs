using System;
using System.Collections.Generic;
using System.Text;
using Esri.ArcGISRuntime.UI.Controls;
using UIKit;

namespace SharedMapView
{
    public partial class ThemeResponsiveMapView : MapView
    {
        public ThemeResponsiveMapView()
        {
            Initialize(new SystemThemeOracle(this));
        }

        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            base.TraitCollectionDidChange(previousTraitCollection);
            if (previousTraitCollection.UserInterfaceStyle != this.TraitCollection.UserInterfaceStyle)
            {
                _themeOracle.ManuallyTriggerThemeEvent();
            }
        }
    }
}
