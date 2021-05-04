using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class CartDao : BaseDao<Cart>
    {
        public CartDao(IDbConnection connection) : base("dbo.Cart", connection) { }
    }
}
