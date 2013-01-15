using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                ConquerItem Item = new ConquerItem();
                Item.ID = Convert.ToUInt32(Reader["ItemID"]);
                Item.Plus = Convert.ToByte(Reader["Plus"]);
                Item.SocketOne = Convert.ToByte(Reader["SocketOne"]);
                Item.SocketTwo = Convert.ToByte(Reader["SocketTwo"]);
                Item.Mode = Convert.ToUInt16(Reader["Mode"]);
                Item.Durability = Convert.ToUInt16(Reader["Durability"]);
                Item.MaxDurability = Convert.ToUInt16(Reader["MaxDurability"]);
                Item.Effect = Convert.ToUInt16(Reader["Effect"]);
                Item.Position = (ItemPosition)Convert.ToByte(Reader["Position"]);

                Client.Entity.Equipment.ThreadSafeAdd(Item.Position, Item);
            }
            Reader.Close();
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

                                  
                                  /*
                                   *  public uint UID { get; set; }
        public uint ID { get; set; }
        public byte Plus { get; set; }
        public byte SocketOne { get; set; }
        public byte SocketTwo { get; set; }
        public ushort Mode { get; set; }
        public ushort Durability { get; set; }
        public ushort MaxDurability { get; set; }
        public ushort Effect { get; set; }
        public ItemPosition Position { get; set; }
                                   * */

                                  
        }
    }
}
