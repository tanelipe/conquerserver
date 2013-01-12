using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
namespace AuthenticationServer.Database
{
    public abstract class DataCtrl
    {
        public abstract string GetTableName();
        public abstract void CreateTable();

        private SQLiteConnection Connection;

        public DataCtrl(SQLiteConnection Connection)
        {
            this.Connection = Connection;

            if (!TableExists())
                CreateTable();
        }

        protected SQLiteConnection GetConnection()
        {
            return Connection;
        }

        private bool TableExists()
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name=@Name";
            Command.Parameters.Add("@Name", DbType.AnsiString).Value = GetTableName();

            int Count = Convert.ToInt32(Command.ExecuteScalar());

            return Count > 0;
        }
    }
}
