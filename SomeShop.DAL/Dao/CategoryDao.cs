using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class CategoryDao : BaseDao<Category>
    {
        public CategoryDao(IDbConnection connection) : base("dbo.Categories", connection) { }
    }
}
