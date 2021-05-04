using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class CharacteristicProductDao : BaseDao<CharacteristicProduct>
    {
        public CharacteristicProductDao(IDbConnection connection) : base("dbo.CharacteristicProducts", connection) { }
    }
}
