using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class OrderDao : BaseDao<Order>
    {
        public OrderDao(IDbConnection connection) : base("dbo.Orders", connection) { }
    }
}
