using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SharedMapView;
using UIKit;

namespace iOSSample
{
    class MainViewController : UIViewController
    {
        private ThemeResponsiveMapView _mapView;
        private UIToolbar _toolbar;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _mapView.Map = new Esri.ArcGISRuntime.Mapping.Map(new Uri("https://www.arcgis.com/home/webmap/viewer.html?webmap=8b33bee8617a46abbfc055db73e9364a"));
        }

        private void SetAutomatic(object sender, EventArgs e) => SetTheme("Automatic");
        private void SetDark(object sender, EventArgs e) => SetTheme("Dark");
        private void SetLight(object sender, EventArgs e) => SetTheme("Light");
        private void SetHighContrast(object sender, EventArgs e) => SetTheme("High Contrast");

        private void SetTheme(string themeName) => _mapView.SelectedTheme = themeName;

        public override void LoadView()
        {
            View = new UIView { BackgroundColor = UIColor.SystemBackgroundColor };

            _mapView = new ThemeResponsiveMapView { TranslatesAutoresizingMaskIntoConstraints = false };
            View.AddSubview(_mapView);

            _toolbar = new UIToolbar { TranslatesAutoresizingMaskIntoConstraints = false };
            _toolbar.Items = new[]
            {
                new UIBarButtonItem("Auto", UIBarButtonItemStyle.Plain, SetAutomatic),
                new UIBarButtonItem("Dark", UIBarButtonItemStyle.Plain, SetDark),
                new UIBarButtonItem("Light", UIBarButtonItemStyle.Plain, SetLight),
                new UIBarButtonItem("High Contrast", UIBarButtonItemStyle.Plain, SetHighContrast)
            };
            View.AddSubview(_toolbar);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _mapView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor),
                _mapView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor),
                _mapView.TopAnchor.ConstraintEqualTo(View.TopAnchor),
                _mapView.BottomAnchor.ConstraintEqualTo(_toolbar.TopAnchor),
                _toolbar.BottomAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.BottomAnchor),
                _toolbar.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor),
                _toolbar.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor)
            });
        }
    }
}