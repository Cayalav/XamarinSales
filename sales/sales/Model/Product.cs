using System;
using System.Collections.Generic;
using System.Text;

namespace sales.Model
{
    public class Product
    {

        public int productId { get; set; }

        public string description { get; set; }
        public string imagePath { get; set; }

        public string remark{ get; set; }

        public Decimal price { get; set; }

        public bool available { get; set; }

        public byte[] imageArray { get; set; }


        public double latitude { get; set; }

        public double longitude { get; set; }

        public DateTime publishOn { get; set; }

        public virtual Category category { get; set; }


        public UserRequest user { get; set; }

        public string ImageFullPath
        {
            get {

                if (string.IsNullOrEmpty(this.imagePath)) {

                    return "icon";
                }

                return imagePath;
            
            }
        }

        public override string ToString()
        {
            return this.description;
        }

    }
}
