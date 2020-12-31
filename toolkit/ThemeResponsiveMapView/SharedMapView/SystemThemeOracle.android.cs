using System;
using System.Collections.Generic;
using System.Text;

namespace SharedMapView
{
    public class SystemThemeOracle : IThemeOracle
    {
        private Android.Content.Context _context;
        public SystemThemeOracle(Android.Content.Context context)
        {
            _context = context;
            // TODO - Set up listener
        }

        public event EventHandler<EventArgs> SystemThemeChanged;

        public string GetCurrentSystemTheme()
        {
            if (_context.Resources.Configuration.UiMode.HasFlag(Android.Content.Res.UiMode.NightYes))
            {
                return "Dark";
            }
            return "Light";
        }

        public void ManuallyTriggerThemeEvent()
        {
            SystemThemeChanged?.Invoke(this, new EventArgs());
        }
    }
}
