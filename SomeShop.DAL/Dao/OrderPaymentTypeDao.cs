using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class OrderPaymentTypeDao : BaseDao<OrderPaymentType>
    {
        public OrderPaymentTypeDao(IDbConnection connection) : base("dbo.OrderPaymentTypes", connection) { }
    }
}
