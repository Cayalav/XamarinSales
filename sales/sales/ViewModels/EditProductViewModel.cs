using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using sales.Helpers;
using sales.Model;
using sales.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace sales.ViewModels
{
    public class EditProductViewModel : BaseViewModel
    {
        #region Attributes
        private Product product;

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


        public Product Product
        {

            get { return this.product; }

            set { this.SetValue(ref this.product, value); }

        }
        #endregion





        #region Constructors
        public EditProductViewModel(Product product)
        {
            this.product = product;
            this.IsEnabled = true;
            this.apiService = new ApiService();
            this.ImageSource = product.ImageFullPath;
            this.LoadCategories();
        }
        #endregion


        #region Commands

        public ICommand SaveCommand
        {

            get
            {

                return new RelayCommand(Save);
            }
        }

        public ICommand ChangeImageCommand
        {
            get
            {

                return new RelayCommand(ChangeImage);
            }
        }
        #endregion

        public ICommand DeleteCommand 
        {
            get {
                return new RelayCommand(DeleteProduct);
            }
        }



        #region Methods

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

            this.Category = this.MyCategories.FirstOrDefault(c => c.categoryId == this.Product.category.categoryId);

            this.IsRunning = false;
            this.IsEnabled = true;
        }


        private void RefreshList()
        {
            this.Categories = new ObservableCollection<Category>(this.MyCategories.OrderBy(c => c.description));
        }

        private async Task<bool> LoadCategoriesFromAPI()
        {
            var url = Application.Current.Resources["UrlApi"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlCategoriesController"].ToString();
            var response = await this.apiService.GetList<Category>(url, prefix, controller, Setting.AccessToken);
            if (!response.IsSuccess)
            {
                return false;
            }

            this.MyCategories = (List<Category>)response.Result;
            return true;
        }


        private async void DeleteProduct()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(Languages.Confirm,
                Languages.DeleteConfirmation
                , Languages.Yes,
                Languages.No);


            if (!answer)
            {
                return;
            }

            this.IsRunning = true;
            this.isEnabled = false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {

                this.IsRunning = false;
                this.isEnabled = true;

                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);

                return;
            }


            var url = Application.Current.Resources["UrlApi"].ToString();

            var prefix = Application.Current.Resources["UrlPrefix"].ToString();

            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await this.apiService.Delete(url, prefix, controller, this.Product.productId,Setting.AccessToken);

            if (!response.IsSuccess)
            {



                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);

                return;
            }

            var viewModel = ProductViewModel.GetInstance();

          

            var deleteProduct = viewModel.MyProducts.Where(p => p.productId == this.Product.productId).FirstOrDefault();

            if (deleteProduct != null)
            {

                viewModel.MyProducts.Remove(deleteProduct);
            }

            viewModel.RefreshList();

            this.IsRunning = false;
            this.isEnabled = true;

            await App.Navigator.PopAsync();


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
            if (string.IsNullOrEmpty(this.Product.description))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error,
                    Languages.DescriptionError,
                    Languages.Accept);
                return;
            }


            if (this.product.price < 0)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error,
                    Languages.PriceError,
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

            if (this.file != null)
            {

                imageArray = FileHelper.ReadFully(this.file.GetStream());

                this.Product.imageArray = imageArray;

            }

            this.Product.category = this.Category;

            var url = Application.Current.Resources["UrlApi"].ToString();

            var prefix = Application.Current.Resources["UrlPrefix"].ToString();

            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await this.apiService.Put(url, prefix, controller, this.Product,this.Product.productId,Setting.AccessToken);

            if (!response.IsSuccess)
            {

                this.IsRunning = false;

                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);

                return;
            }

            var productResult = (Product)response.Result;

            var viewModel = ProductViewModel.GetInstance();

            var oldProduct = viewModel.MyProducts.Where(p => p.productId == this.Product.productId).FirstOrDefault();

            if (oldProduct != null) 
            {

                viewModel.MyProducts.Remove(oldProduct);

            }

            viewModel.MyProducts.Add(productResult);


            this.IsRunning = false;

            this.IsEnabled = true;

            await App.Navigator.PopAsync();

            viewModel.RefreshList();

        }

        #endregion

    }


}
