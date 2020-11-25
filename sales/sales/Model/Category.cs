using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace sales.Model
{
   public class Category
    {
      
        public int categoryId { get; set; }

        public string description {get; set; }
        public string imagePath { get; set; }


        [JsonIgnore]
        public virtual ICollection<Product> products { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.imagePath))
                {
                    return "icon";
                }

                return this.imagePath;
            }
        }

    }
}
