using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
namespace GameServer.Database
{
    public class NpcSpawnCtrl : DataCtrl
    {
        public NpcSpawnCtrl(SQLiteConnection Connection)
            : base(Connection)
        {

        }


        public override string GetTableName()
        {
            return "NpcSpawns";
        }

        public override void CreateTable()
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "CREATE TABLE NpcSpawns(ID integer PRIMARY KEY, Type integer, MapID integer, " +
                                  "X integer, Y integer, Flag integer, Interaction integer);";
            Command.ExecuteNonQuery();
        }
    }
}
