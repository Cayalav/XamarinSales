using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace sales
{
    using Newtonsoft.Json;
    using sales.Helpers;
    using sales.Model;
    using sales.Model.util;
    using sales.Services;
    using sales.ViewModels;
    using sales.Views;
    using System.Threading.Tasks;
    using Views;
    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }

        public static MasterPage Master { get; internal set; }

        public App()
        {

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzA1ODU4QDMxMzgyZTMyMmUzMElCU1pJM2h4d1VieWxCRy8wd0NyTE1DMXJ0b0JQbEprQm5TZXlMVEQ0T2c9");

            InitializeComponent();

            var mainViewModel = MainViewModel.GetInstance();

            if (Setting.IsRemembered)
            {

                if (!string.IsNullOrEmpty(Setting.UserRequest))
                {
                    mainViewModel.UserRequest = JsonConvert.DeserializeObject<UserRequest>(Setting.UserRequest);
               
                }

                mainViewModel.Categories = new CategoriesViewModel();
                this.MainPage = new MasterPage();
            }
            else
            {
                mainViewModel.Login = new LoginViewModel();
                this.MainPage = new NavigationPage(new LoginPage());
            }


        }

        #region Methods
        public static Action HideLoginView
        {
            get
            {
                return new Action(() => Current.MainPage = new NavigationPage(new LoginPage()));
            }
        }

       

        public static async Task NavigateToProfile(TokenResponse token)
        {
            if (token == null)
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                return;
            }

            Setting.IsRemembered = true;
            Setting.AccessToken = token.AccessToken;
            

            var apiService = new ApiService();
            var url = Application.Current.Resources["UrlApi"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlUsersController"].ToString();
            var response = await apiService.GetUser(url, prefix, $"{controller}/get/user", token.UserRequest.username, token.AccessToken);
            if (response.IsSuccess)
            {
                var userASP = (UserRequest)response.Result;
                MainViewModel.GetInstance().UserRequest = userASP;
            
                Setting.UserRequest = JsonConvert.SerializeObject(userASP);
            }

            MainViewModel.GetInstance().Categories = new CategoriesViewModel();
            Application.Current.MainPage = new MasterPage();
        }
        #endregion

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
