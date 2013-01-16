using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                Command.Parameters.Add("@Name", DbType.AnsiString).Value = Detail.Name;
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