using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
namespace AuthenticationServer.Database
{
    public class AccountCtrl : DataCtrl
    {
        public AccountCtrl(SQLiteConnection Connection)
            : base(Connection)
        {

        }

        public override string GetTableName()
        {
            return "Accounts";
        }
        public override void CreateTable()
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "CREATE TABLE Accounts(ID integer PRIMARY KEY AUTOINCREMENT, Username TEXT, Password TEXT);";
            Command.ExecuteNonQuery();
        }

        public void CreateAccount(string Username, string Password)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "INSERT INTO Accounts(Username, Password) VALUES(@Username, @Password);";
            Command.Parameters.Add("@Username", DbType.AnsiString).Value = Username;
            Command.Parameters.Add("@Password", DbType.AnsiString).Value = Password;
            Command.ExecuteNonQuery();
        }
        public bool AccountExists(string Username, string Password, AuthClient Client)
        {
            bool Exists = false;

            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "SELECT * FROM Accounts WHERE Username = @Username AND Password = @Password";
            Command.Parameters.Add("@Username", DbType.AnsiString).Value = Username;
            Command.Parameters.Add("@Password", DbType.AnsiString).Value = Password;

            SQLiteDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                Client.SetAccountID(Convert.ToUInt32(Reader["ID"]));
                Exists = true;
            }

            return Exists;
        }
    }
}
