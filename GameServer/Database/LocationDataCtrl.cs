using System;
using System.Data;
using System.Data.SQLite;
namespace GameServer.Database
{
    public class LocationDataCtrl : DataCtrl
    {
        public LocationDataCtrl(SQLiteConnection Connection)
            : base(Connection)
        {

        }

        public void AddLocation(GameClient Client)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "INSERT INTO Locations(UID, MapID, X, Y) VALUES (@UID, @MapID, @X, @Y);";
            Command.Parameters.Add("@UID", DbType.UInt32).Value = Client.Entity.UID;
            Command.Parameters.Add("@MapID", DbType.UInt16).Value = Client.Entity.Location.MapID;
            Command.Parameters.Add("@X", DbType.UInt16).Value = Client.Entity.Location.X;
            Command.Parameters.Add("@Y", DbType.UInt16).Value = Client.Entity.Location.Y;
            Command.ExecuteNonQuery();
        }
        public void UpdateLocation(GameClient Client)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "UPDATE Locations SET MapID = @MapID, X = @X, Y = @Y WHERE UID = @UID;";
            Command.Parameters.Add("@UID", DbType.UInt32).Value = Client.Entity.UID;
            Command.Parameters.Add("@MapID", DbType.UInt16).Value = Client.Entity.Location.MapID;
            Command.Parameters.Add("@X", DbType.UInt16).Value = Client.Entity.Location.X;
            Command.Parameters.Add("@Y", DbType.UInt16).Value = Client.Entity.Location.Y;
            Command.ExecuteNonQuery();
        }
        public bool GetLocation(GameClient Client)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "SELECT * FROM Locations WHERE UID = @UID;";
            Command.Parameters.Add("@UID", DbType.UInt32).Value = Client.Entity.UID;

            bool Exists = false;
            SQLiteDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                Client.Entity.Location.MapID = Convert.ToUInt16(Reader["MapID"]);
                Client.Entity.Location.X = Convert.ToUInt16(Reader["X"]);
                Client.Entity.Location.Y = Convert.ToUInt16(Reader["Y"]);
                Exists = true;
            }
            Reader.Close();
            return Exists;
        }
        public override string GetTableName()
        {
            return "Locations";
        }

        public override void CreateTable()
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "CREATE TABLE Locations(ID integer PRIMARY KEY AUTOINCREMENT, UID integer, " +
                                  "MapID integer, X integer, Y integer);";
            Command.ExecuteNonQuery();
        }
    }
}
