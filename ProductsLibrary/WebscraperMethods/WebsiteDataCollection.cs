using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProductsLibrary.WebscraperMethods
{
    public class WebsiteDataCollection
    {
        /// <summary>
        /// List of website product names
        /// </summary>
        public List<string> pdNames { get; set; }
        /// <summary>
        /// List of website product prices
        /// </summary>
        public List<decimal>? pdPrices { get; set; }
        /// <summary>
        /// List of website product quantity
        /// </summary>
        public List<string> pdQuantity { get; set; }
        /// <summary>
        /// List of website relative product prices
        /// </summary>
        public List<string> pdPricePerQuantity { get; set; }
        /// <summary>
        /// List of website bitmap images of product icons
        /// </summary>
        public List<BitmapImage> bitmapImages { get; set; }
        /// <summary>
        /// List of all product availabilities
        /// </summary>
        public List<string> productAvailabilities { get; set; }
    }
}
