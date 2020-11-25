

namespace sales.ViewModels
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using sales.Helpers;
    using sales.Model;
    using sales.Services;
    using sales.Views;
    using Xamarin.Forms;

    public class ProductItemViewModel :Product
    {

        #region Attributes
        private ApiService apiService;
        #endregion

        #region Constructos
        public ProductItemViewModel() {
            this.apiService = new ApiService();
         
        }
        #endregion

        #region Commands


        public ICommand EditProductCommand
        {

            get
            {

                return new RelayCommand(EditProduct);
            }
        }

        private async void EditProduct()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this); 
            await App.Navigator.PushAsync(new EditProductPage());

        }

        public ICommand DeleteProductCommand { 

            get
            {

                return new RelayCommand(DeleteProduct);
            }
        }

        private async void DeleteProduct()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(Languages.Confirm,
                Languages.DeleteConfirmation
                ,Languages.Yes,
                Languages.No);


            if (!answer) {
                return;
            }

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);

                return;
            }


            var url = Application.Current.Resources["UrlApi"].ToString();

            var prefix = Application.Current.Resources["UrlPrefix"].ToString();

            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await this.apiService.Delete(url, prefix, controller, this.productId, Setting.AccessToken);

            if (!response.IsSuccess)
            {

           

                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);

                return;
            }

            var viewModel = ProductViewModel.GetInstance();

            var deleteProduct = viewModel.MyProducts.Where(p => p.productId == this.productId).FirstOrDefault();

            if (deleteProduct != null) {

                viewModel.MyProducts.Remove(deleteProduct);
            }

            viewModel.RefreshList();

        

            await App.Navigator.PopAsync();


        }
        #endregion
    }
}
