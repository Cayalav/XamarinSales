using GalaSoft.MvvmLight.Command;
using sales.Helpers;
using sales.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace sales.ViewModels
{
    public class MenuItemViewModel
    {

        #region Attributes
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
        #endregion


   
    #region Commands

    public ICommand GotoCommand
    {
        get
        {
            return new RelayCommand(GoTo);
        }
    }

        async void  GoTo()
    {

            switch (this.PageName) {
                case "LoginPage":

                    Setting.AccessToken = string.Empty;
                    Setting.IsRemembered = false;
                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                 break;

                case "AboutPage":

                    App.Master.IsPresented = false;
                   await App.Navigator.PushAsync(new MapPage());

                break;

            }
            

            

    }
}

    #endregion
}
