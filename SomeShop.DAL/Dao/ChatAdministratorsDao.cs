using System.Data;
using SomeShop.DAL.Models;

namespace SomeShop.DAL.Dao
{
    public class ChatAdministratorsDao : BaseDao<ChatAdministrator>
    {
        public ChatAdministratorsDao(IDbConnection connection) : base("dbo.ChatAdministrators", connection) { }
    }
}