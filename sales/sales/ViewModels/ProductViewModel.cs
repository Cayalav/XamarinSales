


namespace sales.ViewModels
{

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using sales.Helpers;
    using sales.Model;
    using sales.Services;
    using Xamarin.Forms;

    public class ProductViewModel : BaseViewModel
    {

        #region Atributtes

        private string filter;

        private ApiService apiService;

        private DataService dataService;

        private ObservableCollection<ProductItemViewModel> products;

        private bool isRefreshing;
        #endregion


        #region Properties

   
        public Category Category
        {
            get;
            set;
        }

        public string Filter 
        {
            get { return this.filter; }

            set { 

                this.filter = value;
                this.RefreshList();    
            }
        }

        public List<Product> MyProducts { get; set; }

        public ObservableCollection<ProductItemViewModel> Products
        {

            get { return this.products; }

            set { this.SetValue(ref this.products, value); }

        }

        public bool IsRefreshing
        {

            get { return this.isRefreshing; }

            set { this.SetValue(ref this.isRefreshing, value); }

        }
        #endregion

        #region Contructors


        public ProductViewModel(Category category)
        {

            instance = this;

            this.Category = category; 

            apiService = new ApiService();

            this.dataService = new DataService();

            this.LoadProducts();

        }

        #endregion

        #region Singleton

        private static ProductViewModel instance;

        public static ProductViewModel GetInstance() {

           return instance;
            
        }

        #endregion


        #region Methods
        private async void LoadProducts()
        {

            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var answer = await this.LoadProductsFromAPI();
            if (answer)
            {
                this.RefreshList();
            }

            this.IsRefreshing = false;
        }

        private async Task LoadProductsFromDB()
        {

            this.MyProducts = await this.dataService.GetAllProducts();
        }

        private async Task  SaveProductToDB()
        {

            await this.dataService.DeleteAllProducts(); 

            dataService.Insert(this.MyProducts);
        }

        private async Task<bool> LoadProductsFromAPI()
        {

            var url = Application.Current.Resources["UrlApi"].ToString();

            var prefix = Application.Current.Resources["UrlPrefix"].ToString();

            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await this.apiService.GetList<Product>(url, prefix, controller, this.Category.categoryId, Setting.AccessToken);

            if (!response.IsSuccess)
            {
            return false;
            }


            if ((List<Product>)response.Result != null)
            {
                this.MyProducts = (List<Product>)response.Result;
            }
            else {
                this.MyProducts = new List<Product>();
            }
           

            return true;
        }

        public void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                var myListProductItemViewModel = MyProducts.Select(p => new ProductItemViewModel
                {
                    description = p.description,
                    imageArray = p.imageArray,
                    imagePath = p.imagePath,
                    available = p.available,
                    price = p.price,
                    productId = p.productId,
                    publishOn = p.publishOn,
                    remark = p.remark,
                  category = p.category

                });

                this.Products = new ObservableCollection<ProductItemViewModel>(
              myListProductItemViewModel.OrderBy(p => p.description));
            }
            else {

                var myListProductItemViewModel = MyProducts.Select(p => new ProductItemViewModel
                {
                    description = p.description,
                    imageArray = p.imageArray,
                    imagePath = p.imagePath,
                    available = p.available,
                    price = p.price,
                    productId = p.productId,
                    publishOn = p.publishOn,
                    remark = p.remark,
                    category = p.category

                }).Where(p => p.description.ToLower().Contains(this.Filter.ToLower())).ToList();

                this.Products = new ObservableCollection<ProductItemViewModel>(
              myListProductItemViewModel.OrderBy(p => p.description));
            }

        }
        #endregion

        #region Commands

        public ICommand RefreshCommand
        {

            get
            {

                return new RelayCommand(LoadProducts);
            }
        }


        public ICommand SearchCommand
        {

            get
            {

                return new RelayCommand(RefreshList);
            }
        }
        #endregion

    }
}
