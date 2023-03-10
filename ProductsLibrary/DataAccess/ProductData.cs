using ProductsLibrary.Data;
using ProductsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsLibrary.DataAccess
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _db;

        public ProductData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<SupermarketModel>> GetProducts() =>
            _db.LoadData<SupermarketModel, dynamic>(storedProcedure: "dbo.spProducts_GetAll", new { });

        public async Task<SupermarketModel?> GetProduct(int id)
        {
            var results = await _db.LoadData<SupermarketModel, dynamic>(
                storedProcedure: "dbo.spProduct_Get",
                new { Id = id });
            return results.FirstOrDefault();
        }

        public Task InsertProduct(SupermarketModel product) =>
            _db.SaveData(
                storedProcedure: "dbo.s pProduct_Insert",
                new
                {
                    product.ProductName,
                    product.Price,
                    product.Quantity,
                    product.PricePerQuantity,
                    product.Image,
                    product.AvailabilityVisibility
                });

        public Task DeleteProduct(int id) =>
            _db.SaveData(storedProcedure: "dbo.spDelete_Selected", new { Id = id });

        public Task DeleteProducts() =>
            _db.SaveData(storedProcedure: "dbo.spDelete_All", new { });
    }
}
