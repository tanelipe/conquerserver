namespace GameServer
{
    public enum AttackTypes : uint
    {
        None = 0x00,
        Physical = 0x02,
        Magic = 0x15,
        Archer = 0x19,
        RequestMariage = 0x08,
        AcceptMariage = 0x09,
        Kill = 0xE
    }
    public struct AttackRequest
    {
        public ushort Size;
        public ushort Type;
        public uint TimeStamp;
        public uint AttackerUID;
        public uint TargetUID;
        public ushort TargetX;
        public ushort TargetY;
        public AttackTypes AttackType;
        public ushort SpellID;
        public ushort SpellLevel;

        public void Decrypt()
        {
            TargetUID = (uint)Assembler.RollRight(TargetUID, 13, 32);
            TargetUID = (TargetUID ^ 0x5F2D2463 ^ AttackerUID) - 0x746F4AE6;

            SpellID = (ushort)(SpellID ^ (AttackerUID ^ 0x915D));
            SpellID = (ushort)(Assembler.RollLeft(SpellID, 3, 16) - 0xEB42);

            TargetX = (ushort)(TargetX ^ (AttackerUID ^ 0x2ED6));
            TargetX = (ushort)(Assembler.RollLeft(TargetX, 1, 16) - 0x22ee);

            TargetY = (ushort)(TargetY ^ (AttackerUID ^ 0xB99B));
            TargetY = (ushort)(Assembler.RollLeft(TargetY, 5, 16) - 0x8922);
        }
    }
}
