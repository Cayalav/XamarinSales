using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using sales.Helpers;
using sales.Model;
using sales.Services;
using sales.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace sales.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {


        #region Attributes
        private bool isRunning;

        private bool isEnabled;

        private ApiService apiService; 
        #endregion

        #region Properties

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsRemembered { get; set; }

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
        #endregion


        #region Constructors

        public LoginViewModel() 
        {
            this.IsEnabled = true;

            this.IsRemembered = true;

            this.apiService = new ApiService();
        }

        #endregion

        #region Commands

        public ICommand RegisterCommand {

            get { return new RelayCommand(Register); }
        }

        private async void Register()
        {
            MainViewModel.GetInstance().Register = new RegisterViewModel();

            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        public ICommand LoginCommand
        {

            get {
                return new RelayCommand(Login);
            }

        }

        private async void Login()
        {


            if (string.IsNullOrEmpty(this.Email))
            {

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept
                    );

                return;
            }


            if (string.IsNullOrEmpty(this.Password))
            {

                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordValidation,
                    Languages.Accept
                    );

                return;
            }


            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {

                this.IsRunning = false;

                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);

                return;
            }

            var url = Application.Current.Resources["UrlApi"].ToString();

            var token = await this.apiService.GetToken(url,this.Email,this.Password);

            if (token  == null || string.IsNullOrEmpty(token.AccessToken)) 
            {
                this.IsRunning = false;

                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.SomethingWrong, Languages.Accept);

                return;
            }


            Setting.AccessToken = token.AccessToken;
            Setting.IsRemembered = this.IsRemembered;


            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlUsersController"].ToString();
            var response = await this.apiService.GetUser(url, prefix, $"{controller}/get/user", this.Email,token.AccessToken);

            if (response.IsSuccess)
            {
                var userRequest = (UserRequest)response.Result;

                MainViewModel.GetInstance().UserRequest = userRequest;

                Setting.UserRequest = JsonConvert.SerializeObject(userRequest);


            }


            MainViewModel.GetInstance().Categories = new CategoriesViewModel();
            Application.Current.MainPage = new MasterPage();


            this.IsRunning = false;

            this.IsEnabled = true;
        }


        public ICommand LoginFacebookComand
        {
            get
            {
                return new RelayCommand(LoginFacebook);
            }
        }

        private async void LoginFacebook()
        {
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(
                new LoginFacebookPage());
        }

        public ICommand LoginInstagramComand
        {
            get
            {
                return new RelayCommand(LoginInstagram);
            }
        }

        private async void LoginInstagram()
        {
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(
                new LoginInstagramPage());
        }

        public ICommand LoginTwitterComand
        {
            get
            {
                return new RelayCommand(LoginTwitter);
            }
        }

        private async void LoginTwitter()
        {
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(
                new LoginTwitterPage());
        }


        #endregion

    }
}
