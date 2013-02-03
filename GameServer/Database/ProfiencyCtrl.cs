using System;
using System.Data;
using System.Data.SQLite;
namespace GameServer.Database
{
    public class ProfiencyCtrl : DataCtrl
    {
        public ProfiencyCtrl(SQLiteConnection Connection)
            : base(Connection)
        {

        }

        public void LoadProfiencys(GameClient Client)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "SELECT * FROM Profiencys WHERE ClientID = @UID;";
            Command.Parameters.Add("@UID", DbType.Int32).Value = Client.UID;

            LearnProfiency Profiency;

            SQLiteDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                Profiency = LearnProfiency.Create();
                Profiency.ID = Convert.ToUInt16(Reader["ID"]);
                Profiency.Level = Convert.ToUInt16(Reader["Level"]);

                Client.LearnProfiency(Profiency);
            }
            Reader.Close();
        }
        public void SaveProfiencys(GameClient Client)
        {
            foreach (LearnProfiency Profiency in Client.Profiencys)
            {
                if (!UpdateProfiency(Client, Profiency))
                {
                    if (!InsertProfiency(Client, Profiency))
                    {
                        throw new Exception("InsertSpell failed! Spells are not saved!");
                    }
                }
            }
        }

        private bool UpdateProfiency(GameClient Client, LearnProfiency Profiency)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "UPDATE Profiencys SET ID = @ID, Level = @Level WHERE ClientID = @UID AND ID = @ID;";
            Command.Parameters.Add("@ID", DbType.Int32).Value = Profiency.ID;
            Command.Parameters.Add("@UID", DbType.Int32).Value = Client.UID;
            Command.Parameters.Add("@Level", DbType.Int32).Value = Profiency.Level;
            return Command.ExecuteNonQuery() >= 1;
        }
        private bool InsertProfiency(GameClient Client, LearnProfiency Profiency)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "INSERT INTO Profiencys(ClientID, ID, Level) VALUES(@UID, @ID, @Level);";
            Command.Parameters.Add("@UID", DbType.Int32).Value = Client.UID;
            Command.Parameters.Add("@ID", DbType.Int32).Value = Profiency.ID;
            Command.Parameters.Add("@Level", DbType.Int32).Value = Profiency.Level;
            return Command.ExecuteNonQuery() >= 1;
        }

        public override string GetTableName()
        {
            return "Profiencys";
        }

        public override void CreateTable()
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "CREATE TABLE Profiencys(UID integer PRIMARY KEY AUTOINCREMENT, ClientID integer, ID integer, " +
                                  "Level integer);";
            Command.ExecuteNonQuery();
        }
    }
}
