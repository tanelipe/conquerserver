using System.Collections.Generic;
using System;

namespace GameServer
{
    public unsafe class Screen
    {
        private GameClient Client;

        private Dictionary<uint, Entity> PlayerDictionary;
        private Dictionary<uint, NonPlayerCharacter> NPCDictionary;

        private Entity[] ScreenPlayers;
        private NonPlayerCharacter[] ScreenNPCs;

        public Screen(GameClient Client)
        {
            this.Client = Client;

            PlayerDictionary = new Dictionary<uint, Entity>();
            NPCDictionary = new Dictionary<uint, NonPlayerCharacter>();

            ScreenPlayers = new Entity[0];
            ScreenNPCs = new NonPlayerCharacter[0];
        }
        public Entity[] Players { get { return ScreenPlayers; } }
        public NonPlayerCharacter[] NPCs { get { return ScreenNPCs; } }

        public void Wipe()
        {
            PlayerDictionary.Clear();
            ScreenPlayers = new Entity[0];

            NPCDictionary.Clear();
            ScreenNPCs = new NonPlayerCharacter[0];
        }
        public bool Add(Entity Entity)
        {
            lock (PlayerDictionary)
            {
                if (!PlayerDictionary.ContainsKey(Entity.UID))
                {
                    EntitySpawn Spawn = PacketHelper.EntitySpawn(Entity);
                    Client.Send(&Spawn, Spawn.Size);

                    PlayerDictionary.Add(Entity.UID, Entity);
                    Entity[] tmp = new Entity[PlayerDictionary.Count];
                    PlayerDictionary.Values.CopyTo(tmp, 0);
                    ScreenPlayers = tmp;

                    Console.WriteLine("Added {0} To {1} Screen", Entity.Name, Client.Entity.Name);
                    return true;
                }
            }
            return false;
        }
        public bool Add(NonPlayerCharacter NPC)
        {
            lock (NPCDictionary)
            {
                if (!NPCDictionary.ContainsKey(NPC.UID))
                {
                    NPCDictionary.Add(NPC.UID, NPC);
                    NonPlayerCharacter[] tmp = new NonPlayerCharacter[NPCDictionary.Count];
                    NPCDictionary.Values.CopyTo(tmp, 0);
                    ScreenNPCs = tmp;
                    return true;
                }
            }
            return false;
        }
        public bool Remove(Entity Entity)
        {
            lock (PlayerDictionary)
            {
                if (PlayerDictionary.Remove(Entity.UID))
                {
                    Entity[] tmp = new Entity[PlayerDictionary.Count];
                    PlayerDictionary.Values.CopyTo(tmp, 0);
                    ScreenPlayers = tmp;

                    Console.WriteLine("Removed {0} From {1} Screen", Entity.Name, Client.Entity.Name);
                    return true;
                }
            }
            return false;
        }
        public bool Remove(NonPlayerCharacter NPC)
        {
            lock (NPCDictionary)
            {
                if (NPCDictionary.Remove(NPC.UID))
                {
                    NonPlayerCharacter[] tmp = new NonPlayerCharacter[NPCDictionary.Count];
                    NPCDictionary.Values.CopyTo(tmp, 0);
                    ScreenNPCs = tmp;
                    return true;
                }
            }
            return false;
        }

        public void Cleanup()
        {
            bool remove;
            lock (PlayerDictionary)
            {
                foreach (Entity Entity in ScreenPlayers)
                {
                    remove = ConquerMath.CalculateDistance(Client.Entity.Location, Entity.Location) > Kernel.ScreenView;
                    if (remove)
                    {
                        GameClient Owner = Entity.Owner;
                        Owner.Screen.Remove(Client.Entity);

                        Remove(Entity);
                    }
                }
            }
            lock (NPCDictionary)
            {
                foreach (NonPlayerCharacter NPC in ScreenNPCs)
                {
                    remove = ConquerMath.CalculateDistance(Client.Entity.Location, NPC.Location) > Kernel.ScreenView;
                    if (remove)
                    {
                        NPCDictionary.Remove(NPC.UID);
                    }
                }
            }
        }
    }
}
