using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyMapView.Map = new Esri.ArcGISRuntime.Mapping.Map(new Uri("https://www.arcgis.com/home/webmap/viewer.html?webmap=8b33bee8617a46abbfc055db73e9364a"));
            MyMapView.CurrentlySelectedTheme = "Light";

            MyMapView.CurrentlySelectedTheme = null;

            ThemeSelection.SelectionChanged += ThemeSelection_SelectionChanged;
        }

        private void ThemeSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ThemeSelection.SelectedIndex)
            {
                case 0:
                    MyMapView.EnableAutomaticTheming = false;
                    MyMapView.CurrentlySelectedTheme = "Light";
                    break;
                case 1:
                    MyMapView.EnableAutomaticTheming = false;
                    MyMapView.CurrentlySelectedTheme = "Dark";
                    break;
                case 2:
                    MyMapView.EnableAutomaticTheming = false;
                    MyMapView.CurrentlySelectedTheme = "High Contrast";
                    break;
                case 3:
                    MyMapView.EnableAutomaticTheming = true;
                    MyMapView.CurrentlySelectedTheme = null;
                    break;
            }
        }
    }
}
