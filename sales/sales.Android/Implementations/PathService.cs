using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;

using Android.Runtime;
using Android.Views;
using Android.Widget;
using sales.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(sales.Droid.Implementations.PathService))]
namespace sales.Droid.Implementations
{
   
    public class PathService: IPathService
    {

      
    public string GetDataBasePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            return Path.Combine(path, "sales.db3");
        }
    }
}