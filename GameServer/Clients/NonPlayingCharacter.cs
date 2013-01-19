namespace GameServer
{
    public class NonPlayerCharacter : IEntity<NpcSpawn>
    {
        public ushort Type;
        public ushort Direction;
        public ushort Interaction;

        public override NpcSpawn GetSpawnPacket()
        {
            NpcSpawn Spawn = new NpcSpawn();
            Spawn.Size = 0x14;
            Spawn.Type = 0x7EE;
            Spawn.UID = UID;
            Spawn.X = Location.X;
            Spawn.Y = Location.Y;
            Spawn.TypeDirection = (ushort)(Type + Direction);
            Spawn.Interaction = Interaction;
            return Spawn;
        }
        public override EntityFlag GetFlag()
        {
            return EntityFlag.NPC;
        }
    }
}
