using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class FavouriteProductDao : BaseDao<FavouriteProduct>
    {
        public FavouriteProductDao(IDbConnection connection) : base("dbo.FavouriteProducts", connection) { }
    }
}
