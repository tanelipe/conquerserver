using System;
using System.Data;
using System.Data.SQLite;
namespace GameServer.Database
{
    public class ItemTypeCtrl : DataCtrl
    {
        public ItemTypeCtrl(SQLiteConnection Connection)
            : base(Connection)
        {

        }

        public override string GetTableName()
        {
            return "ItemType";
        }

        public override void CreateTable()
        {
            SQLiteTransaction Transaction = GetConnection().BeginTransaction();

            try
            {

                SQLiteCommand Command = GetConnection().CreateCommand();
                Command.CommandText = "CREATE TABLE ItemType(ID integer PRIMARY KEY, Name TEXT, Description TEXT, " +
                                      "Price integer, Class integer, Profiency integer, Level integer, Strength integer, " +
                                      "Dexterity integer, Vitality integer, Spirit integer, MaxDamage integer, MinDamage integer, " +
                                      "Defence integer, DexterityBonus integer, DodgeBonus integer, HitPointBonus integer, " +
                                      "ManaPointBonus integer, MagicAttack integer, MagicDefenceBonus integer, Durability integer, " +
                                      "MaxDurability integer, Frequency integer, Range integer, TradeType integer);";

                Command.ExecuteNonQuery();

                InsertItemDetails();

                Transaction.Commit();
            }
            catch (SQLiteException exception)
            {
                Transaction.Rollback();
                Console.Write(exception.ToString());
            }
        }

        private ItemDetail LoadItemDetail(uint UID)
        {
            ItemDetail Detail = default(ItemDetail);

            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "SELECT * FROM ItemType WHERE ID = @ID";
            Command.Parameters.Add("@ID", DbType.UInt32).Value = UID;

            SQLiteDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                Detail.ID = Convert.ToUInt32(Reader["ID"]);
                Detail.Name = Reader["Name"].ToString();
                Detail.Description = Reader["Description"].ToString();
                Detail.Price = Convert.ToUInt32(Reader["Price"]);
                Detail.Class = Convert.ToUInt16(Reader["Class"]);
                Detail.Profiency = Convert.ToUInt16(Reader["Profiency"]);
                Detail.Level = Convert.ToUInt16(Reader["Level"]);
                Detail.Strength = Convert.ToUInt16(Reader["Strength"]);
                Detail.Dexterity = Convert.ToUInt16(Reader["Dexterity"]);
                Detail.Vitality = Convert.ToUInt16(Reader["Vitality"]);
                Detail.Spirit = Convert.ToUInt16(Reader["Spirit"]);
                Detail.MaxDamage = Convert.ToUInt16(Reader["MaxDamage"]);
                Detail.MinDamage = Convert.ToUInt16(Reader["MinDamage"]);
                Detail.Defence = Convert.ToUInt16(Reader["Defence"]);
                Detail.DexterityBonus = Convert.ToUInt16(Reader["DexterityBonus"]);
                Detail.DodgeBonus = Convert.ToUInt16(Reader["DodgeBonus"]);
                Detail.HitPointBonus = Convert.ToUInt16(Reader["HitPointBonus"]);
                Detail.ManaPointBonus = Convert.ToUInt16(Reader["ManaPointBonus"]);
                Detail.MagicAttack = Convert.ToUInt16(Reader["MagicAttack"]);
                Detail.MagicDefenceBonus = Convert.ToUInt16(Reader["MagicDefenceBonus"]);
                Detail.Durability = Convert.ToUInt16(Reader["Durability"]);
                Detail.MaxDurability = Convert.ToUInt16(Reader["MaxDurability"]);
                Detail.Frequency = Convert.ToUInt16(Reader["Frequency"]);
                Detail.Range = Convert.ToByte(Reader["Range"]);
                Detail.TradeType = Convert.ToByte(Reader["TradeType"]);
            }
            Reader.Close();
            /*
"111303","IronHelmet","None","150","21","0","15","0","0","0","0","0","0","3","0","0","0","0","0","0","3998","3998","1000","1","0"
"111304","IronHelmet","None","150","21","0","15","0","0","0","0","0","0","4","0","0","0","0","0","0","3998","3998","1000","1","0"
"111305","IronHelmet","None","150","21","0","15","0","0","0","0","0","0","5","0","0","0","0","0","0","3998","3998","1000","1","0"
"111306","IronHelmet","None","150","21","0","15","0","0","0","0","0","0","6","0","0","0","0","0","0","3998","3998","1000","1","0"
"111307","IronHelmet","None","150","21","0","15","0","0","0","0","0","0","7","0","0","0","0","0","0","3998","3998","1000","1","0"
"111308","IronHelmet","None","150","21","0","15","0","0","0","0","0","0","8","0","0","0","0","0","0","3998","3998","1000","1","0"
"111309","IronHelmet","None","150","21","0","15","0","0","0","0","0","0","9","0","0","0","0","0","0","3998","3998","1000","1","0"
             * */

            return Detail;
        }
        public bool LoadItemDetail(string Name, string Quality, out ItemDetail Detail)
        {
            Detail = default(ItemDetail);

            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "SELECT ID FROM ItemType WHERE Name = @Name LIMIT 1";
            Command.Parameters.Add("@Name", DbType.AnsiString).Value = Name.ToUpper();

            uint ID = 0;
            SQLiteDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                ID = Convert.ToUInt32(Reader["ID"]);
                ID -= ID % 10;

                switch (Quality.ToLower())
                {
                    case "super": ID += 9; break;
                    case "elite": ID += 8; break;
                    case "unique": ID += 7; break;
                    case "refined": ID += 6; break;
                    case "normalv3": ID += 5; break;
                    case "normalv2": ID += 4; break;
                    case "normalv1": ID += 3; break;
                }
            }

            bool Exists = ID != 0;
            if (Exists)
                Detail = LoadItemDetail(ID);

            Reader.Close();
            return Exists;
        }

        private void InsertItemDetails()
        {
            ItemTypeLoader Loader = new ItemTypeLoader();
            Loader.LoadItems();
            ItemDetail[] Details = Loader.ItemDetails;

            SQLiteCommand Command = GetConnection().CreateCommand();
            Command.CommandText = "INSERT INTO ItemType(ID, Name, Description, Price, Class, Profiency, Level, Strength, Dexterity, " +
                                  "Vitality, Spirit, MaxDamage, MinDamage, Defence, DexterityBonus, DodgeBonus, HitPointBonus, " +
                                  "ManaPointBonus, MagicAttack, MagicDefenceBonus, Durability, MaxDurability, Frequency, Range, TradeType) " +
                                  "VALUES (@ID, @Name, @Description, @Price, @Class, @Profiency, @Level, @Strength, @Dexterity, @Vitality, " +
                                  "@Spirit, @MaxDamage, @MinDamage, @Defence, @DexterityBonus, @DodgeBonus, @HitPointBonus, @ManaPointBonus, " +
                                  "@MagicAttack, @MagicDefenceBonus, @Durability, @MaxDurability, @Frequency, @Range, @TradeType);";

            for (int i = 0; i < Details.Length; i++)
            {
                ItemDetail Detail = Details[i];

                Command.Parameters.Add("@ID", DbType.Int32).Value = Detail.ID;
                Command.Parameters.Add("@Name", DbType.AnsiString).Value = Detail.Name.ToUpper();
                Command.Parameters.Add("@Description", DbType.AnsiString).Value = Detail.Description;
                Command.Parameters.Add("@Price", DbType.Int32).Value = Detail.Price;
                Command.Parameters.Add("@Class", DbType.Int32).Value = Detail.Class;
                Command.Parameters.Add("@Profiency", DbType.Int32).Value = Detail.Profiency;
                Command.Parameters.Add("@Level", DbType.Int32).Value = Detail.Level;
                Command.Parameters.Add("@Strength", DbType.Int32).Value = Detail.Strength;
                Command.Parameters.Add("@Dexterity", DbType.Int32).Value = Detail.Dexterity;
                Command.Parameters.Add("@Vitality", DbType.Int32).Value = Detail.Vitality;
                Command.Parameters.Add("@Spirit", DbType.Int32).Value = Detail.Spirit;
                Command.Parameters.Add("@MaxDamage", DbType.Int32).Value = Detail.MaxDamage;
                Command.Parameters.Add("@MinDamage", DbType.Int32).Value = Detail.MinDamage;
                Command.Parameters.Add("@Defence", DbType.Int32).Value = Detail.Defence;
                Command.Parameters.Add("@DexterityBonus", DbType.Int32).Value = Detail.DexterityBonus;
                Command.Parameters.Add("@DodgeBonus", DbType.Int32).Value = Detail.DodgeBonus;
                Command.Parameters.Add("@HitPointBonus", DbType.Int32).Value = Detail.HitPointBonus;
                Command.Parameters.Add("@ManaPointBonus", DbType.Int32).Value = Detail.ManaPointBonus;
                Command.Parameters.Add("@MagicAttack", DbType.Int32).Value = Detail.MagicAttack;
                Command.Parameters.Add("@MagicDefenceBonus", DbType.Int32).Value = Detail.MagicDefenceBonus;
                Command.Parameters.Add("@Durability", DbType.Int32).Value = Detail.Durability;
                Command.Parameters.Add("@MaxDurability", DbType.Int32).Value = Detail.MaxDurability;
                Command.Parameters.Add("@Frequency", DbType.Int32).Value = Detail.Frequency;
                Command.Parameters.Add("@Range", DbType.Int32).Value = Detail.Range;
                Command.Parameters.Add("@TradeType", DbType.Int32).Value = Detail.TradeType;

                Command.ExecuteNonQuery();
                Command.Parameters.Clear();
            }
        }
    }
}