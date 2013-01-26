using System.Collections.Generic;
using System.Threading;
namespace GameServer
{
    public class EntityManager
    {
        public static Dictionary<uint, GameClient> GameClients;
        public static Dictionary<ushort, List<NonPlayerCharacter>> NPCs;
        private static object Lock;
        
        static EntityManager()
        {
            GameClients = new Dictionary<uint, GameClient>();
            NPCs = new Dictionary<ushort, List<NonPlayerCharacter>>();
           
            Lock = new object();
        }

        public static void AcquireLock()
        {
            if (!Monitor.IsEntered(Lock))
                Monitor.Enter(Lock);
        }
        public static void ReleaseLock()
        {
            if (Monitor.IsEntered(Lock))
                Monitor.Exit(Lock);
        }

        public static void Add(NonPlayerCharacter NPC)
        {
            ushort Map = NPC.Location.MapID;
            lock (NPCs)
            {
                List<NonPlayerCharacter> npcs;
                if (NPCs.ContainsKey(Map))
                {
                    npcs = NPCs[Map];
                    npcs.Add(NPC);
                    NPCs[Map] = npcs;
                }
                else
                {
                    npcs = new List<NonPlayerCharacter>();
                    npcs.Add(NPC);
                    NPCs.Add(Map, npcs);
                }
            }
            //NPCs.ThreadSafeAdd(NPC.UID, NPC);
        }
        public static void Remove(NonPlayerCharacter NPC)
        {
            lock (NPCs)
            {
                ushort MapID = NPC.Location.MapID;
                List<NonPlayerCharacter> npcs;
                if (NPCs.ContainsKey(MapID))
                {
                    npcs = NPCs[MapID];
                    npcs.Remove(NPC);
                    NPCs[MapID] = npcs;
                }
            }
            // NPCs.ThreadSafeRemove(NPC.UID);
        }

        public static void Add(GameClient Client)
        {
            GameClients.ThreadSafeAdd(Client.UID, Client);
        }
        public static void Remove(GameClient Client)
        {
            GameClients.ThreadSafeRemove(Client.UID);
        }
        public static GameClient[] Clients
        {
            get
            {
                GameClient[] tmp = new GameClient[GameClients.Count];
                GameClients.Values.CopyTo(tmp, 0);
                return tmp;
            }
        }

        public static NonPlayerCharacter[] GetNonPlayingCharacters(ushort MapID)
        {
            if (NPCs.ContainsKey(MapID))
            {
                List<NonPlayerCharacter> npcs = NPCs[MapID];
                return npcs.ToArray();
            }
            return null;
        }
    }
}
