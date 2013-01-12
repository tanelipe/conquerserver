using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
namespace AuthenticationServer.Database
{
    public class DatabaseManager
    {
        private SQLiteConnection Connection;
        private ServerCtrl ServerCtrl;
        private AccountCtrl AccountCtrl;

        public DatabaseManager()
        {
            Connection = new SQLiteConnection("Data Source=..\\..\\..\\ConquerDatabase.db;Version=3");
            Connection.Open();

            ServerCtrl = new ServerCtrl(Connection);
            AccountCtrl = new AccountCtrl(Connection);
        }


        public void CreateAccount(string Username, string Password)
        {
            AccountCtrl.CreateAccount(Username, Password);
        }

        public bool ServerExists(string Server, out string Address)
        {
            return ServerCtrl.ServerExists(Server, out Address);
        }
        public void CreateServer(string Server, string Address)
        {
            ServerCtrl.CreateServer(Server, Address);
        }
        public bool AccountExists(string Username, string Password, AuthClient Client)
        {
            return AccountCtrl.AccountExists(Username, Password, Client);
        }
    }
}
