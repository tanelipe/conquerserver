using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
namespace AuthenticationServer.Database
{
    public class ServerCtrl : DataCtrl
    {
        public ServerCtrl(SQLiteConnection Connection) : base(Connection)
        {
            
        }
        public override string GetTableName()
        {
            return "Servers";
        }
        public override void CreateTable()
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "CREATE TABLE Servers(ID integer PRIMARY KEY AUTOINCREMENT, Name TEXT, Address TEXT);";
            Command.ExecuteNonQuery();
        }

        public void CreateServer(string Server, string Address)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "INSERT INTO Servers (Name, Address) VALUES(@Name, @Address);";
            Command.Parameters.Add("@Name", DbType.AnsiString).Value = Server;
            Command.Parameters.Add("@Address", DbType.AnsiString).Value = Address;
            Command.ExecuteNonQuery();
        }
        public bool ServerExists(string Server, out string Address)
        {
            Address = "";
            bool Exists = false;

            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "SELECT * FROM Servers WHERE Name = @Name";
            Command.Parameters.Add("@Name", DbType.AnsiString).Value = Server;

            SQLiteDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                Address = Reader["Address"] as string;
                Exists = true;
            }

            return Exists;
        }
    }
}
