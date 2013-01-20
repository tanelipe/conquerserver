using System.Collections.Generic;
using System.Threading;
namespace GameServer
{
    public class EntityManager
    {
        public static Dictionary<uint, GameClient> GameClients;
        public static Dictionary<uint, NonPlayerCharacter> NPCs;
        private static object Lock;
        
        static EntityManager()
        {
            GameClients = new Dictionary<uint, GameClient>();
            NPCs = new Dictionary<uint, NonPlayerCharacter>();
           
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
            NPCs.ThreadSafeAdd(NPC.UID, NPC);
        }
        public static void Remove(NonPlayerCharacter NPC)
        {
            NPCs.ThreadSafeRemove(NPC.UID);
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
        public static NonPlayerCharacter[] NonPlayingCharacters
        {
            get
            {
                NonPlayerCharacter[] tmp = new NonPlayerCharacter[NPCs.Count];
                NPCs.Values.CopyTo(tmp, 0);
                return tmp;
            }
        }
    }
}
