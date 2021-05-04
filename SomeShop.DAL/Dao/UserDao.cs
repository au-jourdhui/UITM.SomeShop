using SomeShop.DAL.Models;
using Dapper;
using System;
using System.Data;

namespace SomeShop.DAL.Dao
{
    public class UserDao : BaseDao<User>
    {
        public UserDao(IDbConnection connection) : base("dbo.Users", connection) { }

        public User FindByToken(Guid token) => Connection.QueryFirstOrDefault<User>($"select top 1 * from {TableName} where Token = '{token}'");

        public override bool Update(User item)
        {
            item.Token = Guid.NewGuid();
            return base.Update(item);
        }

        public bool UpdateUserPassword(int id, string password)
        {
            return Connection.Execute($"update {TableName} set Password = dbo.MD5HashPassword('{password}') where Id = '{id}'") > 0;
        }
    }
}
