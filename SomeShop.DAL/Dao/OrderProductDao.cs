using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class OrderProductDao : BaseDao<OrderProduct>
    {
        public OrderProductDao(IDbConnection connection) : base("dbo.OrderProducts", connection) { }
    }
}
