using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class CharacteristicDao : BaseDao<Characteristic>
    {
        public CharacteristicDao(IDbConnection connection) : base("dbo.Characteristics", connection) { }
    }
}
