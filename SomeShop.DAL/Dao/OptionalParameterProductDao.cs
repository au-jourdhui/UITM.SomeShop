using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class OptionalParameterProductDao : BaseDao<OptionalParameterProduct>
    {
        public OptionalParameterProductDao(IDbConnection connection) : base("dbo.OptionalParameterProducts", connection) { }
    }
}
