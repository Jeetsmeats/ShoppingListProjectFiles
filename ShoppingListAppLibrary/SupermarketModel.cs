using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace ShoppingListAppLibrary
{
    public class SupermarketModel
    {
        /// <summary>
        /// Name of the product as listed
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Database key
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Product retail price
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// Product retail price per unit of quantity
        /// </summary>
        public string PricePerQuantity { get; set; }
        /// <summary>
        /// Company enum
        /// </summary>
        public Company Company { get; set; }
        /// <summary>
        /// Weight, volume or number of items of the product
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        /// Sets bitmap image of product icon
        /// </summary>
        public BitmapImage Image { get; set; }
    }
}
