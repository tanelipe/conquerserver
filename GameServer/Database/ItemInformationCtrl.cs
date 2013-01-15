using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
namespace GameServer.Database
{
    public class ItemInformationCtrl : DataCtrl
    {
        public ItemInformationCtrl(SQLiteConnection Connection)
            : base(Connection)
        {

        }

        public override string GetTableName()
        {
            return "ItemInformation";
        }

        public override void CreateTable()
        {
            throw new NotImplementedException();
        }
    }
}
