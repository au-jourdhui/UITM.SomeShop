using System.Data;

namespace SomeShop.DAL.Dao
{
    public class DataAccessObject
    {
        protected IDbConnection Connection { get; set; }

        public DataAccessObject(IDbConnection connection) => Connection = connection;
    }
}
