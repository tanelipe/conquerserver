using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
namespace GameServer
{
    public class NpcSpawnLoader
    {
        const string NPC_HEADER_ID =            "NpcID";
        const string NPC_HEADER_TYPE =          "NpcType";
        const string NPC_HEADER_MAP =           "MapID";
        const string NPC_HEADER_X =             "X";
        const string NPC_HEADER_Y =             "Y";
        const string NPC_HEADER_FLAG =          "Flag";
        const string NPC_HEADER_INTERACTION =   "Interaction";

        private Dictionary<uint, NpcSpawnFile> NpcSpawns;
        private const string NPC_SPAWN_LOCATION = "..\\..\\..\\Data Files\\Npc Spawns\\";

        public NpcSpawnLoader()
        {
            NpcSpawns = new Dictionary<uint, NpcSpawnFile>();
            Load();
        }

        private void Load()
        {
            NpcSpawns.Clear();
            DirectoryInfo info = new DirectoryInfo(NPC_SPAWN_LOCATION);
            FileInfo[] NpcFiles = info.GetFiles("*.ini");
         
            Parallel.ForEach<FileInfo>(NpcFiles, file =>
            {
                ProcessNpcInformation(file.FullName);
            });
        }

        private void ProcessNpcInformation(string Location)
        {
            NpcSpawnFile Spawn = new NpcSpawnFile();
            Spawn.Location = new Location();
            string[] Contents = File.ReadAllLines(Location);            
            
            foreach (string Line in Contents)
            {
                if (Line.StartsWith(NPC_HEADER_ID))
                    Spawn.UID = uint.Parse(Line.Remove(0, NPC_HEADER_ID.Length + 1));
                if (Line.StartsWith(NPC_HEADER_TYPE))
                    Spawn.Type = ushort.Parse(Line.Remove(0, NPC_HEADER_TYPE.Length + 1));
                if (Line.StartsWith(NPC_HEADER_MAP))
                    Spawn.Location.MapID = ushort.Parse(Line.Remove(0, NPC_HEADER_MAP.Length + 1));
                if (Line.StartsWith(NPC_HEADER_X))
                    Spawn.Location.X = ushort.Parse(Line.Remove(0, NPC_HEADER_X.Length + 1));
                if (Line.StartsWith(NPC_HEADER_Y))
                    Spawn.Location.Y = ushort.Parse(Line.Remove(0, NPC_HEADER_Y.Length + 1));
                if (Line.StartsWith(NPC_HEADER_FLAG))
                    Spawn.Flag = ushort.Parse(Line.Remove(0, NPC_HEADER_FLAG.Length + 1));
                if (Line.StartsWith(NPC_HEADER_INTERACTION))
                    Spawn.Interaction = ushort.Parse(Line.Remove(0, NPC_HEADER_INTERACTION.Length + 1));
            }
            if (!NpcSpawns.ContainsKey(Spawn.UID))
                NpcSpawns.ThreadSafeAdd(Spawn.UID, Spawn);
        }

        public NpcSpawnFile[] Spawns
        {
            get
            {
                return NpcSpawns.Values.ToArray();
            }
        }
    }
}
