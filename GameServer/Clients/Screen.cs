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
        private void Remove(Entity Entity, bool Mutual = true)
        {
            if (Players.ContainsKey(Entity.UID))
            {
                if (Mutual)
                {
                    Entity Other = Players[Entity.UID];
                    Other.Owner.Screen.Remove(Client.Entity, false);
                }
                Players.ThreadSafeRemove(Entity.UID);
            }
        }
        private void Remove(NonPlayerCharacter NPC)
        {
            if (NonPlayingCharacters.ContainsKey(NPC.UID))
            {
                Players.ThreadSafeRemove(NPC.UID);
            }
        }

        public void Update(bool FullCleanup = false)
        {
            if(FullCleanup)
            {
                Clean();
            }
            try
            {
                EntityManager.AcquireLock();

                GameClient[] Clients = EntityManager.Clients;

                Parallel.ForEach<GameClient>(Clients, client =>
                {
                    if (client.UID != Client.UID)
                    {
                        if (client.Entity.Location.MapID == Client.Entity.Location.MapID)
                        {
                            if (ConquerMath.CalculateDistance(client.Entity.Location, Client.Entity.Location) < 17)
                            {
                                Add(client.Entity);
                            }
                            else
                            {
                                Remove(client.Entity);
                            }
                        }
                    }
                });

                NonPlayerCharacter[] NPCs = EntityManager.NonPlayingCharacters;
                Parallel.ForEach<NonPlayerCharacter>(NPCs, npc =>
                {
                    if (npc.Location.MapID == Client.Entity.Location.MapID)
                    {
                        if (ConquerMath.CalculateDistance(npc.Location, Client.Entity.Location) < 17)
                        {
                            Add(npc);
                        }
                        else
                        {
                            Remove(npc);
                        }
                    }
                });
            }
            finally
            {
                EntityManager.ReleaseLock();
            }
        }
    }
}
