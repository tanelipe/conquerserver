using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
namespace GameServer.Database
{
    public class ItemsCtrl : DataCtrl
    {
        public ItemsCtrl(SQLiteConnection Connection)
            : base(Connection)
        {

        }

        public override string GetTableName()
        {
            return "Items";
        }

        public void Add(ConquerItem Item)
        {

        }

        public override void CreateTable()
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "CREATE TABLE Items(UID integer PRIMARY KEY AUTOINCREMENT, " +
                                  "ClientID integer, ItemID integer, Plus integer, SocketOne integer, SocketTwo integer, " +
                                  "Position integer);";
            Command.ExecuteNonQuery();
        }
    }
}
