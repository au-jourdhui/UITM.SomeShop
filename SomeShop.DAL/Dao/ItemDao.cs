using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class ItemDao : BaseDao<Item>
    {
        public ItemDao(IDbConnection connection) : base("dbo.Items", connection) { }
    }
}
