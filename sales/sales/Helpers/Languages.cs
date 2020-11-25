

namespace sales.Helpers
{
    using sales.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xamarin.Forms;
    public static class Languages
    {

        static Languages()
        {

            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();

            Resources.Resource.Culture = ci;

            DependencyService.Get<ILocalize>().SetLocale(ci);

        }

        public static string Accept
        {

            get { return Resources.Resource.Accept; }
        }

        public static string Error
        {

            get { return Resources.Resource.Error; }
        }

        public static string NoInternet
        {

            get { return Resources.Resource.NoInternet; }
        }

        public static string Product
        {

            get { return Resources.Resource.Product; }
        }

        public static string TurnOnInternet
        {

            get { return Resources.Resource.TurnOnInternet; }
        }

        public static string AddProduct
        {

            get { return Resources.Resource.AddProduct; }
        }

        public static string Description
        {

            get { return Resources.Resource.Description; }
        }

        public static string DescriptionPlaceHolder
        {

            get { return Resources.Resource.DescriptionPlaceHolder; }
        }

        public static string Price
        {

            get { return Resources.Resource.Price; }
        }


        public static string PricePlaceholder
        {

            get { return Resources.Resource.PricePlaceholder; }
        }

        public static string Remark
        {

            get { return Resources.Resource.Remark; }
        }


        public static string Save
        {

            get { return Resources.Resource.Save; }
        }


        public static string ChangeImage
        {

            get { return Resources.Resource.ChangeImage; }
        }

        public static string DescriptionError
        {

            get { return Resources.Resource.DescriptionError; }
        }


        public static string PriceError
        {

            get { return Resources.Resource.PriceError; }
        }


        public static string ImageSource
        {

            get { return Resources.Resource.ImageSource; }
        }


        public static string FromGallery
        {

            get { return Resources.Resource.FromGallery; }
        }


        public static string NewPicture
        {

            get { return Resources.Resource.NewPicture; }
        }


        public static string Cancel
        {

            get { return Resources.Resource.Cancel; }
        }

        public static string Delete
        {

            get { return Resources.Resource.Delete; }
        }

        public static string Edit
        {

            get { return Resources.Resource.Edit; }
        }

        public static string DeleteConfirmation
        {

            get { return Resources.Resource.DeleteConfirmation; }
        }

        public static string Yes
        {

            get { return Resources.Resource.Yes; }
        }

        public static string No
        {

            get { return Resources.Resource.No; }
        }

        public static string Confirm
        {

            get { return Resources.Resource.Confirm; }
        }


        public static string EditProduct
        {

            get { return Resources.Resource.EditProduct; }
        }

        public static string IsAvailable
        {

            get { return Resources.Resource.IsAvailable; }
        }

        public static string Search
        {

            get { return Resources.Resource.Search; }
        }


        public static string Login
        {

            get { return Resources.Resource.Login; }
        }


        public static string Email
        {

            get { return Resources.Resource.Email; }
        }

        public static string EmailPlaceHolder
        {

            get { return Resources.Resource.EmailPlaceHolder; }
        }

        public static string Password
        {

            get { return Resources.Resource.Password; }
        }

        public static string PasswordPlaceHolder
        {

            get { return Resources.Resource.PasswordPlaceHolder; }
        }

        public static string Rememberme
        {

            get { return Resources.Resource.Rememberme; }
        }

        public static string Forgot
        {

            get { return Resources.Resource.Forgot; }
        }

        public static string Register
        {

            get { return Resources.Resource.Register; }
        }

        public static string EmailValidation
        {

            get { return Resources.Resource.EmailValidation; }
        }

        public static string PasswordValidation
        {

            get { return Resources.Resource.PasswordValidation; }
        }

        public static string SomethingWrong
        {

            get { return Resources.Resource.SomethingWrong; }
        }

        public static string Menu
        {

            get { return Resources.Resource.Menu; }
        }

        public static string About
        {

            get { return Resources.Resource.About; }
        }

        public static string Setup
        {

            get { return Resources.Resource.Setup; }
        }

        public static string Exit
        {

            get { return Resources.Resource.Exit; }
        }

        public static string NoProductsMessage
        {
            get { return Resources.Resource.NoProductsMessage; }
        }

        public static string FirstName
        {
            get { return Resources.Resource.FirstName; }
        }

        public static string FirstNamePlaceholder
        {
            get { return Resources.Resource.FirstNamePlaceholder; }
        }

        public static string LastName
        {
            get { return Resources.Resource.LastName; }
        }

        public static string LastNamePlaceholder
        {
            get { return Resources.Resource.LastNamePlaceholder; }
        }

        public static string Phone
        {
            get { return Resources.Resource.Phone; }
        }

        public static string PhonePlaceHolder
        {
            get { return Resources.Resource.PhonePlaceHolder; }
        }

        public static string PasswordConfirm
        {
            get { return Resources.Resource.PasswordConfirm; }
        }

        public static string PasswordConfirmPlaceHolder
        {
            get { return Resources.Resource.PasswordConfirmPlaceHolder; }
        }

        public static string Address
        {
            get { return Resources.Resource.Address; }
        }

        public static string AddressPlaceHolder
        {
            get { return Resources.Resource.AddressPlaceHolder; }
        }

        public static string FirstNameError
        {
            get { return Resources.Resource.FirstNameError; }
        }

        public static string LastNameError
        {
            get { return Resources.Resource.LastNameError; }
        }

        public static string EMailError
        {
            get { return Resources.Resource.EMailError; }
        }

        public static string PhoneError
        {
            get { return Resources.Resource.PhoneError; }
        }

        public static string PasswordError
        {
            get { return Resources.Resource.PasswordError; }
        }

        public static string PasswordConfirmError
        {
            get { return Resources.Resource.PasswordConfirmError; }
        }

        public static string PasswordsNoMatch
        {
            get { return Resources.Resource.PasswordsNoMatch; }
        }

        public static string RegisterConfirmation
        {
            get { return Resources.Resource.RegisterConfirmation; }
        }



        public static string Categories
        {
            get { return Resources.Resource.Categories; }
        }

        public static string Category
        {
            get { return Resources.Resource.Category; }
        }

        public static string CategoryError
        {
            get { return Resources.Resource.CategoryError; }
        }

        public static string CategoryPlaceholder
        {
            get { return Resources.Resource.CategoryPlaceholder; }
        }
    }
}
