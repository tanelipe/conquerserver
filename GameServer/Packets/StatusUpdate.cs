using System.Runtime.InteropServices;
namespace GameServer
{
    public unsafe struct StatusUpdate
    {
        public ushort Size;
        public ushort Type;
        public uint UID;
        public uint Amount;
        public fixed byte Data[1];
    }
    
    [StructLayout(LayoutKind.Explicit)]
    public struct StatusUpdateEntry
    {
        [FieldOffset(0)]
        public ConquerStatusIDs Type;
        [FieldOffset(4)]
        public uint Value;

        public static StatusUpdateEntry Create(ConquerStatusIDs ID, uint Value)
        {
            StatusUpdateEntry Entry = new StatusUpdateEntry();
            Entry.Type = ID;
            Entry.Value = Value;
            return Entry;
        }

        public static StatusUpdateEntry Gold(uint Amount)
        {
            return new StatusUpdateEntry() { Type = ConquerStatusIDs.Money, Value = Amount };
        }
    }
}
