using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace SharedMapView
{
    public class SystemThemeOracle : IThemeOracle
    {
        private UIView _parentView;
        public SystemThemeOracle(UIView owningView)
        {
            _parentView = owningView;
        }

        public event EventHandler<EventArgs> SystemThemeChanged;

        public string GetCurrentSystemTheme()
        {
            switch (_parentView.TraitCollection?.UserInterfaceStyle)
            {
                case UIUserInterfaceStyle.Dark:
                    return "Dark";
                default:
                    return "Light";
            }
        }

        public void ManuallyTriggerThemeEvent()
        {
            SystemThemeChanged?.Invoke(this, new EventArgs());
        }
    }
}
