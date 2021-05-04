using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class ProductDao : BaseDao<Product>
    {
        public ProductDao(IDbConnection connection) : base("dbo.Products", connection) { }
    }
}
