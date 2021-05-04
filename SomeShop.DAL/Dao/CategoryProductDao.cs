using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class CategoryProductDao : BaseDao<CategoryProduct>
    {
        public CategoryProductDao(IDbConnection connection) : base("dbo.CategoryProducts", connection) { }
    }
}
