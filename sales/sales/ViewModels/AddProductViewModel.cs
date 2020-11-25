

namespace sales.ViewModels
{

   
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;
    using GalaSoft.MvvmLight.Command;
    using sales.Helpers;
    using sales.Services;
    using sales.Model;
    using System.Linq;
    using Plugin.Media.Abstractions;
    using Plugin.Media;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Plugin.Geolocator;
    using Plugin.Geolocator.Abstractions;

    public class AddProductViewModel: BaseViewModel
    {

        #region Attributes

        private ApiService apiService;

        private bool isRunning;

        private bool isEnabled;

        private MediaFile file;

        private ImageSource imageSource;

        private ObservableCollection<Category> categories;

        private Category category;

        #endregion

        #region Properties

        public List<Category> MyCategories { get; set; }

        public Category Category
        {
            get { return this.category; }
            set { this.SetValue(ref this.category, value); }
        }

        public ObservableCollection<Category> Categories
        {
            get { return this.categories; }
            set { this.SetValue(ref this.categories, value); }
        }

        public string Description { get; set; }

        public string Price { get; set; }

        public string Remark { get; set; }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }

        #endregion


        #region Constructor

        public AddProductViewModel() {

           

            this.IsEnabled = true;
            this.apiService = new ApiService();
            this.ImageSource = "icon";

            this.LoadCategories();
        }

        private async void LoadCategories()
        {
            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var answer = await this.LoadCategoriesFromAPI();
            if (answer)
            {
                this.RefreshList();

            
            }

            this.IsRunning = false;
            this.IsEnabled = true;
        }

        #endregion


        #region Commands

        public ICommand SaveCommand {

            get {

                return new RelayCommand(Save);
            }
        }

        public ICommand ChangeImageCommand {
            get
            {

                return new RelayCommand(ChangeImage);
            }
        }
        #endregion


        #region Methods


        private async Task<bool> LoadCategoriesFromAPI()
        {
            var url = Application.Current.Resources["UrlApi"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlCategoriesController"].ToString();
            var response = await this.apiService.GetList<Category>(url, prefix, controller,  Setting.AccessToken);
            if (!response.IsSuccess)
            {
                return false;
            }

            this.MyCategories = (List<Category>)response.Result;
            return true;
        }

       

        private void RefreshList()
        {
            this.Categories = new ObservableCollection<Category>(this.MyCategories.OrderBy(c => c.description));
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture);

            if (source == Languages.Cancel)
            {
                this.file = null;
                return;
            }

            if (source == Languages.NewPicture)
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = this.file.GetStream();
                    return stream;
                });
            }
        }

        private async void Save()
        {
            if(string.IsNullOrEmpty(this.Description)){
                await Application.Current.MainPage.DisplayAlert(Languages.Error, 
                    Languages.DescriptionError,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }

            var price = decimal.Parse(this.Price);

            if (price < 0)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }

            if (this.Category == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.CategoryError,
                    Languages.Accept);
                return;
            }



            this.IsRunning = true;

            this.IsEnabled = false;



            var connection = await this.apiService.CheckConnection();




            if (!connection.IsSuccess)
            {
                this.IsRunning = true;

                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);

                return;
            }

            byte[] imageArray = null;

            if (this.file != null) {

                imageArray = FileHelper.ReadFully(this.file.GetStream());
            }



            var location = await this.GetLocation();

            var product = new Product
            {
                description = this.Description,
                price = price,
                remark = this.Remark,
                imageArray = imageArray,
                category = this.Category,
                user = MainViewModel.GetInstance().UserRequest,
                latitude = location == null? 0 : location.Latitude,
                longitude = location == null? 0 : location.Longitude

            };

            var url = Application.Current.Resources["UrlApi"].ToString();

            var prefix = Application.Current.Resources["UrlPrefix"].ToString();

            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await this.apiService.Post(url, prefix, controller, product,Setting.AccessToken);

            if (!response.IsSuccess)
            {

                this.IsRunning = true;

                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);

                return;
            }

            var productResult = (Product)response.Result;

            var viewModel = ProductViewModel.GetInstance();

            viewModel.MyProducts.Add(productResult);

            viewModel.RefreshList();

            this.IsRunning = true;

            this.IsEnabled = true;

            await App.Navigator.PopAsync();

        }

        private async Task<Position> GetLocation()
        {
            var locator = CrossGeolocator.Current;

            locator.DesiredAccuracy = 50;

            var location = await locator.GetPositionAsync();

            return location;

        }

        #endregion

    }
    }
