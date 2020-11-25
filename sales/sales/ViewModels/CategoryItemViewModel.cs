using GalaSoft.MvvmLight.Command;
using sales.Model;
using sales.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace sales.ViewModels
{
    public class CategoryItemViewModel: Category
    {

        #region Commands

        public ICommand GotoCategoryCommand
        {
            get
            {
                return new RelayCommand(GotoCategory);
            }
        }

        private async void GotoCategory()
        {
            MainViewModel.GetInstance().Products = new ProductViewModel(this);
            await App.Navigator.PushAsync(new ProductsPage());

        }

        #endregion
    }
}
