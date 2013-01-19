using System.Collections.Generic;
using System.Threading;
namespace GameServer
{
    public class ClientManager
    {
        private static Dictionary<uint, GameClient> GameClients;
        private static object Lock;
        
        static ClientManager()
        {
            GameClients = new Dictionary<uint, GameClient>();
            Lock = new object();
        }

        public static void AcquireLock()
        {
            Monitor.Enter(Lock);            
        }
        public static void ReleaseLock()
        {
            if (Monitor.IsEntered(Lock))
                Monitor.Exit(Lock);
        }

        public static void Add(GameClient Client)
        {
            GameClients.ThreadSafeAdd(Client.UID, Client);
        }
        public static void Remove(GameClient Client)
        {
            GameClients.ThreadSafeRemove(Client.UID);
        }
        public static GameClient Get(uint UID)
        {
            if (GameClients.ContainsKey(UID))
                return GameClients[UID];
            else
                return null; 
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
    }
}
