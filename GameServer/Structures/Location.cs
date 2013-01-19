namespace GameServer
{
    public struct Location
    {
        public ushort X;
        public ushort Y;
        public ushort MapID;

        public override string ToString()
        {
            return MapID + " " + X + " " + Y;
        }
    }
}
