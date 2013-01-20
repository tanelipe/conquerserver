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
            SQLiteTransaction Transaction = GetConnection().BeginTransaction();
            try
            {
                SQLiteCommand Command = GetConnection().CreateCommand();
                Command.CommandText = "CREATE TABLE NpcSpawns(ID integer PRIMARY KEY, Type integer, MapID integer, " +
                                      "X integer, Y integer, Flag integer, Interaction integer);";
                Command.ExecuteNonQuery();

                InsertNpcSpawns();

                Transaction.Commit();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Transaction.Rollback();
            }
        }
        private void InsertNpcSpawns()
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "INSERT INTO NpcSpawns(ID, Type, MapID, X, Y, Flag, Interaction) VALUES (" +
                                  "@ID, @Type, @MapID, @X, @Y, @Flag, @Interaction);";

            NpcSpawnLoader Loader = new NpcSpawnLoader();

            NpcSpawnFile[] Spawns = Loader.Spawns;
            foreach (NpcSpawnFile Spawn in Spawns)
            {
                Command.Parameters.Add("@ID", DbType.UInt32).Value = Spawn.UID;
                Command.Parameters.Add("@Type", DbType.UInt16).Value = Spawn.Type;
                Command.Parameters.Add("@MapID", DbType.UInt16).Value = Spawn.Location.MapID;
                Command.Parameters.Add("@X", DbType.UInt16).Value = Spawn.Location.X;
                Command.Parameters.Add("@Y", DbType.UInt16).Value = Spawn.Location.Y;
                Command.Parameters.Add("@Flag", DbType.UInt16).Value = Spawn.Flag;
                Command.Parameters.Add("@Interaction", DbType.UInt16).Value = Spawn.Interaction;

                Command.ExecuteNonQuery();
                Command.Parameters.Clear();
            }

        }
    }
}
