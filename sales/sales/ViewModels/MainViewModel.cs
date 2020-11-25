using GalaSoft.MvvmLight.Command;
using sales.Helpers;
using sales.Model;
using sales.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace sales.ViewModels
{
    public class MainViewModel
    {

        #region Properties

        public ProductViewModel Products { get; set; }

        public AddProductViewModel AddProduct { get; set; }

        public EditProductViewModel EditProduct { get; set; }

        public LoginViewModel Login { get; set; }

        public RegisterViewModel Register { get; set; }

        public ObservableCollection<MenuItemViewModel> Menu { get; set; }

        public CategoriesViewModel Categories { get; set; }

        public UserRequest UserRequest { get; set; }

        public string UserFullName {
            get 
            {
                if ( this.UserRequest != null) 
                {
                    return $"{this.UserRequest.firstName} {this.UserRequest.lastName} "; 
                }

                return null;
            }
        }

        public string UserImageFullPath
        {
            get
            {
                if (this.UserRequest != null)
                {
                    return $"{UserRequest.imagePath} ";
                }

                return null;
            }
        }


        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;


            this.LoadMenu();
        }

        private void LoadMenu()
        {

            this.Menu = new ObservableCollection<MenuItemViewModel>();

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_info",
                PageName = "AboutPage",
                Title = Languages.About
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_phonelink_setup",
                PageName = "SetupPage",
                Title = Languages.Setup
            });


            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app.png",
                PageName = "LoginPage",
                Title = Languages.Exit
            }); ;
        }

        #endregion

        #region Singleton

        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {

            if (instance == null)
            {
                return new MainViewModel();
            }


            return instance;


        }

        #endregion


        #region Commands
        public ICommand AddProductCommand
        {

            get
            {
                return new RelayCommand(GoToAddProduct);
            }
        }

        private async void GoToAddProduct()
        {

            AddProduct = new AddProductViewModel();

            await App.Navigator.PushAsync(new AddProductPage());

        }
        #endregion
    }
}
