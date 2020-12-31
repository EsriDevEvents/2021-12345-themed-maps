using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using Esri.ArcGISRuntime.Mapping;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using SharedMapView;

namespace AndroidSample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ThemeResponsiveMapView _mapView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            _mapView = FindViewById<ThemeResponsiveMapView>(Resource.Id.MainMapView);
            InitAsync();
        }

        private async void InitAsync()
        {
            var map = new Map(new Uri("https://www.arcgis.com/home/webmap/viewer.html?webmap=8b33bee8617a46abbfc055db73e9364a"));
            await map.LoadAsync();
            map.MinScale = 0;
            map.MaxScale = 0;
            _mapView.Map = map;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
