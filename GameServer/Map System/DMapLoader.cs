using System.Collections.Generic;
using System.IO;
using System.Text;
namespace GameServer
{
    public class GameMapLoader
    {
        private const string MAP_DIRECTORY = "..\\..\\..\\Data Files\\";
        private Dictionary<ushort, string> MapLocations;

        public GameMapLoader()
        {
            MapLocations = new Dictionary<ushort, string>();
        }
        public void Load()
        {
            using (FileStream Stream = new FileStream(MAP_DIRECTORY + "DmapFiles.dat", FileMode.Open))
            {
                using (BinaryReader Reader = new BinaryReader(Stream))
                {
                    ushort Count = Reader.ReadUInt16();
                    for (int i = 0; i < Count; i++)
                    {
                        ushort MapID = Reader.ReadUInt16();
                        string MapPath = Encoding.ASCII.GetString(Reader.ReadBytes(Reader.ReadByte()));
                        MapLocations.ThreadSafeAdd(MapID, MapPath);
                    }
                }
            }
        }
        public Dictionary<ushort, string> Maps
        {
            get { return MapLocations; }
        }
    }
  
    public struct DMapTile
    {
        public bool Access;
    }

    public class DMap
    {
        private const string MAP_DIRECTORY = "..\\..\\..\\Data Files\\";
        private ushort Height;
        private ushort Width;
        private ushort MapID;
        private string Location;
        private DMapTile[] Tiles;

        public ushort GetMapID() { return MapID; }
        public ushort GetWidth() { return Width; }
        public ushort GetHeight() { return Height; }
        public string GetLocation() { return Location; }

        public DMap(ushort MapID, string Location)
        {
            this.MapID = MapID;
            this.Location = Location;
        }

        public int GetIndex(ushort X, ushort Y)
        {
            return Width * X + Y;
        }
        public bool Accessible(ushort X, ushort Y)
        {
            int Index = GetIndex(X, Y);
            if (Index < 0 || Index > Tiles.Length)
            {
                return false;
            }
            return Tiles[Index].Access;
        }
        public void Load()
        {
            string SavePath = Location.Replace("DMap", "map");
            SavePath = SavePath.Replace("map\\map\\", "");
            using (FileStream Stream = new FileStream(MAP_DIRECTORY + "Maps\\" + SavePath, FileMode.Open))
            {
                using (BinaryReader Reader = new BinaryReader(Stream))
                {
                    Width = Reader.ReadUInt16();
                    Height = Reader.ReadUInt16();
                    Tiles = new DMapTile[Width * Height];

                    for (ushort Y = 0; Y < Height; Y++)
                    {
                        for (ushort X = 0; X < Width; X++)
                        {
                            int index = GetIndex(X, Y);
                            Tiles[index] = new DMapTile();
                            Tiles[index].Access = Reader.ReadBoolean();
                        }
                    }
                }
            }
        }
    }
    public class DMapLoader
    {
        private List<DMap> Maps;
        private GameMapLoader GameMapLoader;

        public DMapLoader()
        {
            Maps = new List<DMap>();
            GameMapLoader = new GameMapLoader();
        }
        public void LoadGameMap()
        {
            GameMapLoader.Load();
        }
        public void LoadMaps()
        {
            foreach (KeyValuePair<ushort, string> pair in GameMapLoader.Maps)
            {
                InternalLoad(pair.Key, pair.Value);
            }
        }
        private void InternalLoad(ushort MapID, string Location)
        {
            DMap Map = new DMap(MapID, Location);
            Map.Load();
            Maps.Add(Map);
        }

        public DMap GetMapData(ushort MapID)
        {
            var map = Maps.Find((item) => { return item.GetMapID() == MapID; });
            return map;
        }

        public bool GetAccessible(ushort MapID, ushort X, ushort Y)
        {
            DMap map = Maps.Find((item) =>
            {
                return item.GetMapID() == MapID;
            });
            return map.Accessible(X, Y);
        }

        public Dictionary<ushort, string> GameMaps
        {
            get { return GameMapLoader.Maps; }
        }
        public List<DMap> DMaps { get { return Maps; } }
    }
}
