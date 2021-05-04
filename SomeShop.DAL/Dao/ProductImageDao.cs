using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class ProductImageDao : BaseDao<ProductImage>
    {
        public ProductImageDao(IDbConnection connection) : base("dbo.ProductImages", connection) { }
    }
}
