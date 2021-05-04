using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class SupportMessageDao : BaseDao<SupportMessage>
    {
        public SupportMessageDao(IDbConnection connection) : base("dbo.SupportMessages", connection) { }
    }
}
