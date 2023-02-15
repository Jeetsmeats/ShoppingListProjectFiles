using ProductsLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsLibrary.DataAccess
{
    public interface IProductData
    {
        Task DeleteProduct(int id);
        Task DeleteProducts();
        Task<SupermarketModel?> GetProduct(int id);
        Task<IEnumerable<SupermarketModel>> GetProducts();
        Task InsertProduct(SupermarketModel product);
    }
}