using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class BrandDao : BaseDao<Brand>
    {
        public BrandDao(IDbConnection connection) : base("dbo.Brands", connection) { }
    }
}
