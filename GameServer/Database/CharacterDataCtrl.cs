using System;
using System.Data;
using System.Data.SQLite;
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
            Client.Entity.UID = 1000000 + Client.UID;

            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "INSERT INTO Characters(ID, Avatar, Mesh, HairStyle, Gold, Experience, Strength, Dexterity, Vitality," +
                                  "Spirit, StatPoints, HitPoints, ManaPoints, PKPoints, Level, Class, Reborn, Name, Spouse) VALUES (" +
                                  "@ID, @Avatar, @Mesh, @Hairstyle, @Gold, @Experience, @Strength, @Dexterity, @Vitality, @Spirit, @StatPoints, " +
                                  "@HitPoints, @ManaPoints, @PKPoints, @Level, @Class, @Reborn, @Name, @Spouse);";

            Command.Parameters.Add("@ID", DbType.Int32).Value = Client.Entity.UID;
            Command.Parameters.Add("@Avatar", DbType.Int32).Value = 10;
            Command.Parameters.Add("@Mesh", DbType.Int32).Value = Model;
            Command.Parameters.Add("@HairStyle", DbType.Int32).Value = 310;
            Command.Parameters.Add("@Gold", DbType.Int32).Value = 0;
            Command.Parameters.Add("@Experience", DbType.Int32).Value = 0;
            Command.Parameters.Add("@Strength", DbType.Int32).Value = 0;
            Command.Parameters.Add("@Dexterity", DbType.Int32).Value = 0;
            Command.Parameters.Add("@Vitality", DbType.Int32).Value = 0;
            Command.Parameters.Add("@Spirit", DbType.Int32).Value = 0;
            Command.Parameters.Add("@StatPoints", DbType.Int32).Value = 397;
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
        public void UpdateCharacter(GameClient Client)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "UPDATE Characters SET Avatar = @Avatar, Mesh = @Mesh, HairStyle = @Hairstyle, Gold =  @Gold," +
                                  "Experience = @Experience, Strength = @Strength, Dexterity = @Dexterity, Vitality = @Vitality," +
                                  "Spirit = @Spirit, StatPoints = @StatPoints, HitPoints = @HitPoints, ManaPoints = @ManaPoints, " +
                                  "PKPoints = @PKPoints, Level = @Level, Class = @Class, Reborn = @Reborn, Name = @Name, Spouse = @Spouse " +
                                  "WHERE ID = @ID;";

            Command.Parameters.Add("@ID", DbType.Int32).Value = Client.Entity.UID;
            Command.Parameters.Add("@Avatar", DbType.Int32).Value = Client.Entity.Avatar;
            Command.Parameters.Add("@Mesh", DbType.Int32).Value = Client.Entity.Mesh;
            Command.Parameters.Add("@HairStyle", DbType.Int32).Value = Client.Entity.HairStyle;
            Command.Parameters.Add("@Gold", DbType.Int32).Value = Client.Entity.Money;
            Command.Parameters.Add("@Experience", DbType.Int32).Value = Client.Entity.Experience;
            Command.Parameters.Add("@Strength", DbType.Int32).Value = Client.Entity.StatusPoints.Strength;
            Command.Parameters.Add("@Dexterity", DbType.Int32).Value = Client.Entity.StatusPoints.Dexterity;
            Command.Parameters.Add("@Vitality", DbType.Int32).Value = Client.Entity.StatusPoints.Vitality;
            Command.Parameters.Add("@Spirit", DbType.Int32).Value = Client.Entity.StatusPoints.Spirit;
            Command.Parameters.Add("@StatPoints", DbType.Int32).Value = Client.Entity.StatusPoints.Free;
            Command.Parameters.Add("@HitPoints", DbType.Int32).Value = Client.Entity.HitPoints;
            Command.Parameters.Add("@ManaPoints", DbType.Int32).Value = Client.Entity.ManaPoints;
            Command.Parameters.Add("@PKPoints", DbType.Int32).Value = Client.Entity.PKPoints;
            Command.Parameters.Add("@Level", DbType.Byte).Value = Client.Entity.Level;
            Command.Parameters.Add("@Class", DbType.Byte).Value = Client.Entity.Class;
            Command.Parameters.Add("@Reborn", DbType.Byte).Value = Client.Entity.Reborn;
            Command.Parameters.Add("@Name", DbType.AnsiString).Value = Client.Entity.Name;
            Command.Parameters.Add("@Spouse", DbType.AnsiString).Value = Client.Entity.Spouse;

            Command.ExecuteNonQuery();
        }
        public bool GetCharacterData(GameClient Client)
        {
            uint ID = 1000000 + Client.UID;

            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "SELECT * FROM Characters WHERE ID = @ID;";
            Command.Parameters.Add("@ID", DbType.Int32).Value = ID;

            bool Exists = false;
            SQLiteDataReader Reader = Command.ExecuteReader();
            if (Reader.Read())
            {
                Client.Entity.UID = ID;
                Client.Entity.Avatar = Convert.ToByte(Reader["Avatar"]);
                Client.Entity.Mesh = Convert.ToUInt16(Reader["Mesh"]);
                Client.Entity.HairStyle = Convert.ToUInt16(Reader["HairStyle"]);
                Client.Entity.Money = Convert.ToUInt32(Reader["Gold"]);
                Client.Entity.Experience = Convert.ToUInt32(Reader["Experience"]);
                Client.Entity.StatusPoints.Strength = Convert.ToUInt16(Reader["Strength"]);
                Client.Entity.StatusPoints.Dexterity = Convert.ToUInt16(Reader["Dexterity"]);
                Client.Entity.StatusPoints.Vitality = Convert.ToUInt16(Reader["Vitality"]);
                Client.Entity.StatusPoints.Spirit = Convert.ToUInt16(Reader["Spirit"]);
                Client.Entity.StatusPoints.Free = Convert.ToUInt16(Reader["StatPoints"]);
                Client.Entity.HitPoints = Convert.ToUInt16(Reader["HitPoints"]);
                Client.Entity.ManaPoints = Convert.ToUInt16(Reader["ManaPoints"]);
                Client.Entity.PKPoints = Convert.ToUInt16(Reader["PKPoints"]);
                Client.Entity.Level = Convert.ToByte(Reader["Level"]);
                Client.Entity.Class = Convert.ToByte(Reader["Class"]);
                Client.Entity.Reborn = Convert.ToByte(Reader["Reborn"]);
                Client.Entity.Name = Reader["Name"] as string;
                Client.Entity.Spouse = Reader["Spouse"] as string;
                Exists = true;
            }
            Reader.Close();
            return Exists;
        }
        public override string GetTableName()
        {
            return "Characters";
        }

        public override void CreateTable()
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "CREATE TABLE Characters(ID integer PRIMARY KEY, Avatar integer, Mesh integer, HairStyle integer, Gold integer, Experience integer, " +
                                  "Strength integer, Dexterity integer, Vitality integer, Spirit integer, StatPoints integer, HitPoints integer, ManaPoints integer, " +
                                  "PKPoints integer, Level integer, Class integer, Reborn integer, Name TEXT, Spouse TEXT);";

            Command.ExecuteNonQuery();
        }
        public void DestroyTable()
        {
            DropTable();
        }
    }
}
