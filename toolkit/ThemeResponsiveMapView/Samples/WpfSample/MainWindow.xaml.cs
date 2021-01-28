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

namespace WpfSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }
        private async void Initialize()
        {
            MainMapView.Map = new Esri.ArcGISRuntime.Mapping.Map(new Uri("https://www.arcgis.com/home/webmap/viewer.html?webmap=5199e65ba2f74d618e4c68287e59bdaa"));
            await MainMapView.Map.LoadAsync();
            MainMapView.Map.MinScale = 0;
            MainMapView.Map.MaxScale = 0;
            MainMapView.SetViewpoint(new Esri.ArcGISRuntime.Mapping.Viewpoint(0, 0, 100_000_000));
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            MainMapView.SelectedTheme = (sender as RadioButton).Content.ToString();
        }
    }
}
