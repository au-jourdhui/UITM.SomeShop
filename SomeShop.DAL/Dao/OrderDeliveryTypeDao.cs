using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class OrderDeliveryTypeDao : BaseDao<OrderDeliveryType>
    {
        public OrderDeliveryTypeDao(IDbConnection connection) : base("dbo.OrderDeliveryTypes", connection) { }
    }
}
