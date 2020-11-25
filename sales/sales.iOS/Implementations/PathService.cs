


[assembly: Xamarin.Forms.Dependency(typeof(sales.iOS.Implementations.PathService))]
namespace sales.iOS.Implementations
{

    using sales.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    class PathService : IPathService
    {

        public string GetDataBasePath()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (Directory.Exists(libFolder)) {

                Directory.CreateDirectory(libFolder);
            }


            return Path.Combine(libFolder, "sales.db3");
        }
    }
}