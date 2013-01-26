namespace GameServer
{
    public class NonPlayerCharacter : IEntity<NpcSpawn>
    {
        public ushort Type;
        public ushort Flag;
        public ushort Interaction;

        public override NpcSpawn GetSpawnPacket()
        {
            NpcSpawn Spawn = new NpcSpawn();
            Spawn.Size = 0x14;
            Spawn.Type = 0x7EE;
            Spawn.UID = UID;
            Spawn.X = Location.X;
            Spawn.Y = Location.Y;
            Spawn.SubType = Type;
            Spawn.Interaction = Interaction;
            Spawn.Direction = (ushort)Angle;
            return Spawn;
        }
        public override EntityFlag GetFlag()
        {
            return EntityFlag.NPC;
        }
    }
}
