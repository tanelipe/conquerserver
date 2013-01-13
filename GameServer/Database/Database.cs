using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
namespace GameServer.Database
{
    public class DatabaseManager
    {
        private SQLiteConnection Connection;
        private CharacterDataCtrl CharacterCtrl;

        public DatabaseManager()
        {
            Connection = new SQLiteConnection("Data Source=..\\..\\..\\ConquerDatabase.db;Version=3");
            Connection.Open();

            CharacterCtrl = new CharacterDataCtrl(Connection);
        }

        public void CreateCharacter(GameClient Client, ushort Model, ushort Class, string Name)
        {
            CharacterCtrl.CreateCharacter(Client, Model, Class, Name);
        }
        public bool GetCharacterData(GameClient Client)
        {
            return CharacterCtrl.GetCharacterData(Client);
        }
    }
}
