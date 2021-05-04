using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class OrderStatusDao : BaseDao<OrderStatus>
    {
        public OrderStatusDao(IDbConnection connection) : base("dbo.OrderStatuses", connection) { }
    }
}
