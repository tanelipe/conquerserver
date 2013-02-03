using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Loads the NonPlayingCharacters from database (Currently only TwinCity)
        /// </summary>
        /// <param name="Spawns"></param>
        public void Load()
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "SELECT * FROM NpcSpawns";
            SQLiteDataReader Reader = Command.ExecuteReader();

            List<NonPlayerCharacter> npcs = new List<NonPlayerCharacter>();
            while (Reader.Read())
            {
                NonPlayerCharacter NPC = new NonPlayerCharacter();
                NPC.UID = Convert.ToUInt32(Reader["ID"]);

                ushort Type = Convert.ToUInt16(Reader["Type"]);

                NPC.Angle = (ConquerAngle)(Type % 10);
                NPC.Type = (ushort)(Type - (Type % 10));

                NPC.Location = new Location();
                NPC.Location.MapID = Convert.ToUInt16(Reader["MapID"]);
                NPC.Location.X = Convert.ToUInt16(Reader["X"]);
                NPC.Location.Y = Convert.ToUInt16(Reader["Y"]);
                NPC.Flag = Convert.ToUInt16(Reader["Flag"]);
                NPC.Interaction = Convert.ToUInt16(Reader["Interaction"]);

                EntityManager.Add(NPC);
            }
           
            Reader.Close();
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
