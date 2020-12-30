using System;
using System.Collections.Generic;
using System.Text;

namespace SharedMapView
{
    public interface IThemeOracle
    {
        string GetCurrentSystemTheme();
        event EventHandler<EventArgs> SystemThemeChanged;
        void ManuallyTriggerThemeEvent();
    }
}
