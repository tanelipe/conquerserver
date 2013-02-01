
namespace GameServer
{
    public enum NpcOptionType : byte
    {
        None = 0x00,
        Dialogue = 0x01,
        Option = 0x02,
        Input = 0x03,
        Avatar = 0x04,
        Finish = 0x64
    }

    public unsafe struct NpcDialog
    {
        public ushort Size;
        public ushort Type;
        public uint Timer;
        public ushort Extra;
        public byte OptionID;
        public NpcOptionType OptionType;
        public bool DontDisplay;
        public fixed sbyte Input[1];


        public static NpcDialog Create()
        {
            NpcDialog Dialog = new NpcDialog();
            Dialog.Size = 16;
            Dialog.Type = 0x7F0;
            Dialog.Timer = (uint)System.Environment.TickCount;
            Dialog.OptionID = 255;
            Dialog.OptionType = NpcOptionType.None;
            Dialog.DontDisplay = true;
            return Dialog;
        }
        /*
         * public static void Say(string Text)
        {
            Text = Text.Replace("~", " ") + "\n";
            NpcPacket Dialog = new NpcPacket((byte)Text.Length);
            byte[] Pac = PacketKernel.Serialize(Dialog, Dialog.Size);
            fixed (byte* Buffer = Pac)
            {
                PacketKernel.Encode(Buffer, Text, 14);
                NpcPacket* SNpc = NpcPacket.Alloc(Buffer, (byte)Text.Length);
                SNpc->ID = NpcType.Dialogue;
                Client.Send(Pac);
            }
        }
        public static void Link(string Text, byte DialogLink)
        {
            Text = Text.Replace("~", " ") + "\n";
            NpcPacket Dialog = new NpcPacket((byte)Text.Length);
            byte[] Pac = PacketKernel.Serialize(Dialog, Dialog.Size);
            fixed (byte* Buffer = Pac)
            {
                PacketKernel.Encode(Buffer, Text, 14);
                NpcPacket* LNpc = NpcPacket.Alloc(Buffer, (byte)Text.Length);
                LNpc->ID = NpcType.Option;
                LNpc->OptionID = DialogLink;
                Client.Send(Pac);
            }
        }
        public static void Input(string Text, byte DialogLink)
        {
            NpcPacket Dialog = new NpcPacket((byte)Text.Length);
            byte[] Pac = Dialog.Serialize();
            fixed (byte* Buffer = Pac)
            {
                PacketKernel.Encode(Buffer, Text, 14);
                NpcPacket* LNpc = NpcPacket.Alloc(Buffer, (byte)Text.Length);
                LNpc->ID = NpcType.Input;
                LNpc->OptionID = DialogLink;
                Client.Send(Pac);
            }
        }
        public static void Face(ushort FaceID)
        {
            NpcPacket Dialog = new NpcPacket(0);
            Dialog.wParam = FaceID;
            Dialog.ID = NpcType.Avatar;
            Client.Send(Dialog.Serialize());
        }
        public static void Finish()
        {
            NpcPacket Dialog = new NpcPacket(0);
            Dialog.ID = NpcType.Finish;
            Client.Send(Dialog.Serialize());
        }
         * */
    }
}
