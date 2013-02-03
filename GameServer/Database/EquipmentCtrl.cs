using System;
using System.Data;
using System.Data.SQLite;

namespace GameServer.Database
{
    public class EquipmentCtrl : DataCtrl
    {
        public EquipmentCtrl(SQLiteConnection Connection)
            : base(Connection)
        {
        
        }

        public void LoadEquipment(GameClient Client)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "SELECT * FROM Equipments WHERE EntityUID = @UID;";
            Command.Parameters.Add("@UID", DbType.Int32).Value = Client.Entity.UID;

            SQLiteDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                ConquerItem Item = new ConquerItem(Client);
                Item.ID = Convert.ToUInt32(Reader["ItemID"]);
                Item.Plus = Convert.ToByte(Reader["Plus"]);
                Item.SocketOne = Convert.ToByte(Reader["SocketOne"]);
                Item.SocketTwo = Convert.ToByte(Reader["SocketTwo"]);
                Item.Mode = Convert.ToUInt16(Reader["Mode"]);
                Item.Durability = Convert.ToUInt16(Reader["Durability"]);
                Item.MaxDurability = Convert.ToUInt16(Reader["MaxDurability"]);
                Item.Effect = Convert.ToUInt16(Reader["Effect"]);
                Item.Position = (ItemPosition)Convert.ToByte(Reader["Position"]);

                Client.AddEquipment(Item, Item.Position);
            }
            Reader.Close();
        }

        public void SaveEquipment(GameClient Client)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();

            ConquerItem Item;
            for (ItemPosition position = ItemPosition.Headgear; position <= ItemPosition.Boots; position++)
            {
                if (Client.TryGetEquipment(position, out Item))
                {
                    if (!UpdateEquipment(Client.Entity.UID, Item))
                    {
                        if (!InsertEquipment(Client.Entity.UID, Item))
                        {
                            throw new Exception("InsertEquipment failed! Equipment are not saved!");
                        }
                    }
                }
            }
        }

        private bool UpdateEquipment(uint UID, ConquerItem Item)
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "UPDATE Equipments SET ItemID = @ID, Plus = @Plus, SocketOne = @SocketOne, SocketTwo = @SocketTwo, " +
                                  "Mode = @Mode, Durability = @Durability, MaxDurability = @Durability, Effect = @Effect, Position = @Position " +
                                  "WHERE EntityUID = @UID AND Position = @Position;";

            Command.Parameters.Add("@ID", DbType.Int32).Value = Item.ID;
            Command.Parameters.Add("@Plus", DbType.Int32).Value = Item.Plus;
            Command.Parameters.Add("@SocketOne", DbType.Int32).Value = Item.SocketOne;
            Command.Parameters.Add("@SocketTwo", DbType.Int32).Value = Item.SocketTwo;
            Command.Parameters.Add("@Mode", DbType.Int32).Value = Item.Mode;
            Command.Parameters.Add("@Durability", DbType.Int32).Value = Item.Durability;
            Command.Parameters.Add("@MaxDurability", DbType.Int32).Value = Item.MaxDurability;
            Command.Parameters.Add("@Effect", DbType.Int32).Value = Item.Effect;
            Command.Parameters.Add("@Position", DbType.Int32).Value = (int)Item.Position;
            Command.Parameters.Add("@UID", DbType.Int32).Value = UID;

            // One or more rows were updated
            return Command.ExecuteNonQuery() >= 1;
        }
        private bool InsertEquipment(uint UID, ConquerItem Item){
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "INSERT INTO Equipments (EntityUID, ItemID, Plus, SocketOne, SocketTwo, Mode, Durability, " +
                                  "MaxDurability, Effect, Position) VALUES (@UID, @ID, @Plus, @SocketOne, @SocketTwo, " +
                                  "@Mode, @Durability, @MaxDurability, @Effect, @Position);";

            Command.Parameters.Add("@ID", DbType.Int32).Value = Item.ID;
            Command.Parameters.Add("@Plus", DbType.Int32).Value = Item.Plus;
            Command.Parameters.Add("@SocketOne", DbType.Int32).Value = Item.SocketOne;
            Command.Parameters.Add("@SocketTwo", DbType.Int32).Value = Item.SocketTwo;
            Command.Parameters.Add("@Mode", DbType.Int32).Value = Item.Mode;
            Command.Parameters.Add("@Durability", DbType.Int32).Value = Item.Durability;
            Command.Parameters.Add("@MaxDurability", DbType.Int32).Value = Item.MaxDurability;
            Command.Parameters.Add("@Effect", DbType.Int32).Value = Item.Effect;
            Command.Parameters.Add("@Position", DbType.Int32).Value = (int)Item.Position;
            Command.Parameters.Add("@UID", DbType.Int32).Value = UID;

            // One or more rows were updated
            return Command.ExecuteNonQuery() >= 1;
        }

        public override string GetTableName()
        {
            return "Equipments";
        }

        public override void CreateTable()
        {
            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "CREATE TABLE Equipments(ID integer PRIMARY KEY AUTOINCREMENT, " +
                                  "EntityUID integer, ItemID integer, Plus integer, SocketOne integer, SocketTwo integer, " +
                                  "Mode integer, Durability integer, MaxDurability integer, Effect integer, Position integer);";
            Command.ExecuteNonQuery();                                  
        }
    }
}
