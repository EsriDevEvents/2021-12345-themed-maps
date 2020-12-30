using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SharedMapView
{
    public partial class ThemeResponsiveMapView : INotifyPropertyChanged
    {
        private IList<string> _availableThemes;
        public IList<string> AvailableThemes
        {
            get => _availableThemes;
            private set
            {
                if (value != _availableThemes)
                {
                    _availableThemes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AvailableThemes)));
                }
            }
        }

        private string _selectedTheme;
        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (value != _selectedTheme)
                {
                    _selectedTheme = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTheme)));
                    _ = BuildAndApplySubsetMap();
                }
            }
        }

        private string _fallbackTheme;
        public string FallbackTheme
        {
            get => _fallbackTheme;
            set
            {
                if (value != _fallbackTheme)
                {
                    _fallbackTheme = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FallbackTheme)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
