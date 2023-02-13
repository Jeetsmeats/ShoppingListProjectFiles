using System.Windows.Media.Imaging;
namespace ProductsLibrary.Models
{
    public class SupermarketModel
    {

        /// <summary>
        /// Database key
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Name of the product as listed
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Product retail price
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// Product retail price per unit of quantity
        /// </summary>
        public string PricePerQuantity { get; set; }
        /// <summary>
        /// Weight, volume or number of items of the product
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        /// Sets bitmap image of product icon
        /// </summary>
        public BitmapImage Image { get; set; }
        /// <summary>
        /// Checks if the product is available on stores
        /// </summary>
        public string AvailabilityVisibility { get; set; }
    }
}
