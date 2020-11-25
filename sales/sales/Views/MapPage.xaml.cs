using Plugin.Geolocator;
using sales.Helpers;
using sales.Model;
using sales.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace sales.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.Locator();
        }


        private async void Locator()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var location = await locator.GetPositionAsync();
            var position = new Position(location.Latitude, location.Longitude);
            this.MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));

            try
            {
                this.MyMap.IsShowingUser = true;

            }
            catch (Exception ex)
            {
                ex.ToString();
            }


            var pins = await this.GetPins();
            this.ShowPins(pins);

        }

        private void ShowPins(List<Pin> pins)
        {
            foreach (var pin in pins)
            {
                this.MyMap.Pins.Add(pin);
            }
        }

        private async Task<List<Pin>> GetPins()
        {
            var pins = new List<Pin>();
            var apiService = new ApiService();
            var url = Application.Current.Resources["UrlApi"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await apiService.GetList<Product>(url, prefix, controller, Setting.AccessToken);
            var products = (List<Product>)response.Result;
            foreach (var product in products.Where(p => p.latitude != 0 && p.longitude != 0).ToList())
            {
                var position = new Position(product.latitude, product.longitude);
                pins.Add(new Pin
                {
                    Address = product.remark,
                    Label = product.description,
                    Position = position,
                    Type = PinType.Place,
                });
            }

            return pins;
        }
    }
}