using SomeShop.DAL.Models;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class ContactInfoDao : BaseDao<ContactInfo>
    {
        public ContactInfoDao(IDbConnection connection) : base("dbo.ContactInfo", connection) { }
    }
}
