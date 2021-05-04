using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class OptionalParameterDao : BaseDao<OptionalParameter>
    {
        public OptionalParameterDao(IDbConnection connection) : base("dbo.OptionalParameters", connection) { }
    }
}
