using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
namespace GameServer.Database
{
    public class CharacterDataCtrl : DataCtrl
    {
        public CharacterDataCtrl(SQLiteConnection Connection)
            : base(Connection)
        {

        }

        public void CreateCharacter(GameClient Client, ushort Model, ushort Class, string Name)
        {
            uint ID = 100000 + Client.UID;

            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "INSERT INTO Characters(ID, Model, HairStyle, Gold, Experience, Strength, Dexterity, Vitality," +
                                  "Spirit, StatPoints, HitPoints, ManaPoints, PKPoints, Level, Class, Reborn, Name, Spouse) VALUES (" +
                                  "@ID, @Model, @Hairstyle, @Gold, @Experience, @Strength, @Dexterity, @Vitality, @Spirit, @StatPoints, " +
                                  "@HitPoints, @ManaPoints, @PKPoints, @Level, @Class, @Reborn, @Name, @Spouse);";

            Command.Parameters.Add("@ID", DbType.Int32).Value = ID;
            Command.Parameters.Add("@Model", DbType.Int32).Value = Model;
            Command.Parameters.Add("@HairStyle", DbType.Int32).Value = 420;
            Command.Parameters.Add("@Gold", DbType.Int32).Value = 0;
            Command.Parameters.Add("@Experience", DbType.Int32).Value = 0;
            Command.Parameters.Add("@Strength", DbType.Int32).Value = 1;
            Command.Parameters.Add("@Dexterity", DbType.Int32).Value = 2;
            Command.Parameters.Add("@Vitality", DbType.Int32).Value = 3;
            Command.Parameters.Add("@Spirit", DbType.Int32).Value = 4;
            Command.Parameters.Add("@StatPoints", DbType.Int32).Value = 0;
            Command.Parameters.Add("@HitPoints", DbType.Int32).Value = 10;
            Command.Parameters.Add("@ManaPoints", DbType.Int32).Value = 0;
            Command.Parameters.Add("@PKPoints", DbType.Int32).Value = 0;
            Command.Parameters.Add("@Level", DbType.Byte).Value = 1;
            Command.Parameters.Add("@Class", DbType.Byte).Value = Class;
            Command.Parameters.Add("@Reborn", DbType.Byte).Value = 0;
            Command.Parameters.Add("@Name", DbType.AnsiString).Value = Name;
            Command.Parameters.Add("@Spouse", DbType.AnsiString).Value = "NONE";

            Command.ExecuteNonQuery();
            

        }
        public bool GetCharacterData(GameClient Client)
        {
            uint ID = 100000 + Client.UID;

            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "SELECT * FROM Characters WHERE ID = @ID;";
            Command.Parameters.Add("@ID", DbType.Int32).Value = ID;

            SQLiteDataReader Reader = Command.ExecuteReader();
            if (Reader.Read())
            {
                Client.Character.ID = ID;
                Client.Character.Model = Convert.ToUInt16(Reader["Model"]);
                Client.Character.HairStyle = Convert.ToUInt16(Reader["HairStyle"]);
                Client.Character.Gold = Convert.ToUInt32(Reader["Gold"]);
                Client.Character.Experience = Convert.ToUInt32(Reader["Experience"]);
                Client.Character.Strength = Convert.ToUInt16(Reader["Strength"]);
                Client.Character.Dexterity = Convert.ToUInt16(Reader["Dexterity"]);
                Client.Character.Vitality = Convert.ToUInt16(Reader["Vitality"]);
                Client.Character.Spirit = Convert.ToUInt16(Reader["Spirit"]);
                Client.Character.StatPoints = Convert.ToUInt16(Reader["StatPoints"]);
                Client.Character.HitPoints = Convert.ToUInt16(Reader["HitPoints"]);
                Client.Character.ManaPoints = Convert.ToUInt16(Reader["ManaPoints"]);
                Client.Character.PKPoints = Convert.ToUInt16(Reader["PKPoints"]);
                Client.Character.Level = Convert.ToByte(Reader["Level"]);
                Client.Character.Class = Convert.ToByte(Reader["Class"]);
                Client.Character.Reborn = Convert.ToByte(Reader["Reborn"]);
                Client.Character.Name = Reader["Name"] as string;
                Client.Character.Spouse = Reader["Spouse"] as string;

                Reader.Close();
                return true;
            }
            return false;
        }
        public override string GetTableName()
        {
            return "Characters";
        }

        public override void CreateTable()
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "CREATE TABLE Characters(ID integer PRIMARY KEY, Model integer, HairStyle integer, Gold integer, Experience integer, " +
                                  "Strength integer, Dexterity integer, Vitality integer, Spirit integer, StatPoints integer, HitPoints integer, ManaPoints integer, " +
                                  "PKPoints integer, Level integer, Class integer, Reborn integer, Name TEXT, Spouse TEXT);";

            Command.ExecuteNonQuery();
        }
    }
}
