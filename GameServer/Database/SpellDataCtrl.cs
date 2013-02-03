using System;
using System.Data;
using System.Data.SQLite;
namespace GameServer.Database
{
    public class SpellDataCtrl : DataCtrl
    {
        public SpellDataCtrl(SQLiteConnection Connection)
            : base(Connection)
        {

        }

        public void LoadSpells(GameClient Client)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "SELECT * FROM Spells WHERE ClientID = @UID;";
            Command.Parameters.Add("@UID", DbType.Int32).Value = Client.UID;

            LearnSpell Spell;

            SQLiteDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                Spell = LearnSpell.Create();
                Spell.ID = Convert.ToUInt16(Reader["ID"]);
                Spell.Level = Convert.ToUInt16(Reader["Level"]);

                Client.LearnSpell(Spell);
            }
            Reader.Close();
        }
        public void SaveSpells(GameClient Client)
        {
            foreach (LearnSpell Spell in Client.Spells)
            {
                if (!UpdateSpell(Client, Spell))
                {
                    if (!InsertSpell(Client, Spell))
                    {
                        throw new Exception("InsertSpell failed! Spells are not saved!");
                    }
                }
            }
        }

        private bool UpdateSpell(GameClient Client, LearnSpell Spell)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "UPDATE Spells SET ID = @ID, Level = @Level WHERE ClientID = @UID AND ID = @ID;";
            Command.Parameters.Add("@ID", DbType.Int32).Value = Spell.ID;
            Command.Parameters.Add("@UID", DbType.Int32).Value = Client.UID;
            Command.Parameters.Add("@Level", DbType.Int32).Value = Spell.Level;
            return Command.ExecuteNonQuery() >= 1;
        }
        private bool InsertSpell(GameClient Client, LearnSpell Spell)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "INSERT INTO Spells(ClientID, ID, Level) VALUES(@UID, @ID, @Level);";
            Command.Parameters.Add("@UID", DbType.Int32).Value = Client.UID;
            Command.Parameters.Add("@ID", DbType.Int32).Value = Spell.ID;
            Command.Parameters.Add("@Level", DbType.Int32).Value = Spell.Level;
            return Command.ExecuteNonQuery() >= 1;
        }

        public override string GetTableName()
        {
            return "Spells";
        }

        public override void CreateTable()
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "CREATE TABLE Spells(UID integer PRIMARY KEY AUTOINCREMENT, ClientID integer, ID integer, " +
                                  "Level integer);";
            Command.ExecuteNonQuery();
        }
    }
}
