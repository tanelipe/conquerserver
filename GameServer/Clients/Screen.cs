using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public unsafe class Screen
    {
        private GameClient Client;
        public Dictionary<uint, Entity> Players;
        public Dictionary<uint, Monster> Monsters;
        public Dictionary<uint, NonPlayerCharacter> NonPlayingCharacters;

        public Screen(GameClient Client)
        {
            this.Client = Client;

            Players = new Dictionary<uint, Entity>();
            Monsters = new Dictionary<uint, Monster>();
            NonPlayingCharacters = new Dictionary<uint, NonPlayerCharacter>();
        }

   
        private void Clean()
        {
            Players.Clear();
            Monsters.Clear();
            NonPlayingCharacters.Clear();
        }
        private void Add(Entity Entity, bool Mutual = true)
        {
            if (!Players.ContainsKey(Entity.UID))
            {
                EntitySpawn SpawnPacket = PacketHelper.EntitySpawn(Entity);
                Client.Send(&SpawnPacket, SpawnPacket.Size);

            }
            Players.ThreadSafeAdd(Entity.UID, Entity);
        
            if (Mutual)
            {
                // If someone sees you, you see them.
                Entity.Owner.Screen.Add(Client.Entity, false);
            }
        }
        private void Add(Monster Monster)
        {
            Monsters.ThreadSafeAdd(Monster.UID, Monster);
        }
        private void Add(NonPlayerCharacter NPC)
        {
            NonPlayingCharacters.ThreadSafeAdd(NPC.UID, NPC);
        }

        private void Remove(uint UID, bool Mutual = true)
        {
            if (Players.ContainsKey(UID))
            {

                if (Mutual)
                {
                    Entity Entity = Players[UID];
                    Entity.Owner.Screen.Remove(UID, false);
                }
                Players.ThreadSafeRemove(UID);
            }
            if (Monsters.ContainsKey(UID))
                Monsters.ThreadSafeRemove(UID);
            if (NonPlayingCharacters.ContainsKey(UID))
                NonPlayingCharacters.ThreadSafeRemove(UID);
        }

        public void Update(bool FullCleanup = false)
        {
            if(FullCleanup)
            {
                Clean();
            }
            try
            {
                ClientManager.AcquireLock();

                GameClient[] Clients = ClientManager.Clients;
                foreach (GameClient Other in Clients)
                {
                    if (Other.UID == Client.UID) continue;
                    if (Other.Entity.Location.MapID != Client.Entity.Location.MapID) continue;

                    if (ConquerMath.CalculateDistance(Client.Entity.Location, Other.Entity.Location) < 17)
                    {
                        Add(Other.Entity);
                    }
                    else
                    {
                        Remove(Other.Entity.UID);
                    }
                }
            }
            finally
            {
                ClientManager.ReleaseLock();
            }
        }
    }
}
