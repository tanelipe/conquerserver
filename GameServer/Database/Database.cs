using System;
using System.Data.SQLite;
namespace GameServer.Database
{
    public class DatabaseManager
    {
        private SQLiteConnection Connection;
        private CharacterDataCtrl CharacterCtrl;
        private LocationDataCtrl LocationCtrl;
        private ProfiencyCtrl ProfiencyCtrl;
        private EquipmentCtrl EquipmentCtrl;
        private ItemTypeCtrl ItemTypeCtrl;
        private NpcSpawnCtrl NpcSpawnCtrl;
        private SpellDataCtrl SpellCtrl;
        private ItemsCtrl ItemCtrl;   

        public DatabaseManager()
        {
            Connection = new SQLiteConnection("Data Source=..\\..\\..\\ConquerDatabase.db;Version=3");
            Connection.Open();

            CharacterCtrl = new CharacterDataCtrl(Connection);
            LocationCtrl = new LocationDataCtrl(Connection);
            EquipmentCtrl = new EquipmentCtrl(Connection);
            ItemTypeCtrl = new ItemTypeCtrl(Connection);
            NpcSpawnCtrl = new NpcSpawnCtrl(Connection);
            ItemCtrl = new ItemsCtrl(Connection);
            SpellCtrl = new SpellDataCtrl(Connection);
            ProfiencyCtrl = new ProfiencyCtrl(Connection);

            NpcSpawnCtrl.Load();
        }
        public ItemDetail GetItemDetail(string Name, string Quality)
        {
            ItemDetail details = new ItemDetail();
            if (ItemTypeCtrl.LoadItemDetail(Name, Quality, out details))
            {
                return details;
            }
            throw new Exception(string.Format("Item {0} Quality {1} doesn't exist", Name, Quality));
        }

        public void LoadEquipment(GameClient Client)
        {
            EquipmentCtrl.LoadEquipment(Client);
        }
        public void DropCharacterTable()
        {
            CharacterCtrl.DestroyTable();
        }
        public void SaveCharacter(GameClient Client)
        {
            CharacterCtrl.UpdateCharacter(Client);
            EquipmentCtrl.SaveEquipment(Client);
            LocationCtrl.UpdateLocation(Client);
            SpellCtrl.SaveSpells(Client);
            ProfiencyCtrl.SaveProfiencys(Client);
        }
        public void CreateCharacter(GameClient Client, ushort Model, ushort Class, string Name)
        {
            CharacterCtrl.CreateCharacter(Client, Model, Class, Name);
            LocationCtrl.AddLocation(Client);
        }
        public bool GetCharacterData(GameClient Client)
        {
            bool Result = CharacterCtrl.GetCharacterData(Client);
            Result &= LocationCtrl.GetLocation(Client);
            return Result;
        }
        public void LoadSpells(GameClient Client)
        {
            SpellCtrl.LoadSpells(Client);
        }
        public void LoadProfiencys(GameClient Client)
        {
            ProfiencyCtrl.LoadProfiencys(Client);
        }
    }
}
