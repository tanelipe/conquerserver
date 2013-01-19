
namespace GameServer
{
    public abstract class IEntity<T> where T : struct
    {
        public uint UID;
        public uint Mesh;
        public uint Status;
        public Location Location;
        public ConquerAngle Angle;
        public ConquerAction Action;

        public IEntity()
        {
            Location = new Location();
            Location.X = 400;
            Location.Y = 400;
            Location.MapID = 1002;

            Action = ConquerAction.Stand;
            Angle = ConquerAngle.Unknown;
        }

        public abstract EntityFlag GetFlag();
        public abstract T GetSpawnPacket();

        public void Walk(ConquerAngle Angle)
        {
            int dx = 0, dy = 0;
            switch (Angle)
            {
                case ConquerAngle.North: dx = -1; dy = -1; break;
                case ConquerAngle.South: dx = 1; dy = 1; break;
                case ConquerAngle.East: dx = 1; dy = -1; break;
                case ConquerAngle.West: dx = -1; dy = 1; break;
                case ConquerAngle.NorthWest: dx = -1; break;
                case ConquerAngle.SouthWest: dy = 1; break;
                case ConquerAngle.NorthEast: dy = -1; break;
                case ConquerAngle.SouthEast: dx = 1; break;
            }
            Location.X = (ushort)(Location.X + dx);
            Location.Y = (ushort)(Location.Y + dy);
        }
    }
}
