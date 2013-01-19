using System;

namespace GameServer
{
    public class Monster : IEntity<EntitySpawn>
    {
        public override EntitySpawn GetSpawnPacket()
        {
            throw new NotImplementedException();
        }
        public override EntityFlag GetFlag()
        {
            return EntityFlag.Monster;
        }
    }
}
