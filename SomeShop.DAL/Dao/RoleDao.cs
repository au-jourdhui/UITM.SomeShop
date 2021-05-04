using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class RoleDao : BaseDao<Role>
    {
        public RoleDao(IDbConnection connection) : base("dbo.Roles", connection) { }
    }
}
