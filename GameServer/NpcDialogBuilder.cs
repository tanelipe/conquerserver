
namespace GameServer
{
    public unsafe class NpcDialogBuilder
    {
        public static void Avatar(GameClient Client, ushort ID)
        {
            NpcDialog* Packet = PacketHelper.NpcPacket();
            Packet->OptionType = NpcOptionType.Avatar;
            Packet->Extra = ID;
            Client.Send(Packet, Packet->Size);
            Memory.Free(Packet);
        }
        public static void Text(GameClient Client, string Text)
        {
            NpcDialog* Packet = PacketHelper.NpcPacket(Text);
            Packet->OptionType = NpcOptionType.Dialogue;
            Client.Send(Packet, Packet->Size);
            Memory.Free(Packet);
        }
        public static void Finish(GameClient Client)
        {
            NpcDialog* Packet = PacketHelper.NpcPacket();
            Packet->OptionType = NpcOptionType.Finish;
            Client.Send(Packet, Packet->Size);
            Memory.Free(Packet);
        }
        public static void Input(GameClient Client, byte OptionID, string Text)
        {
            NpcDialog* Packet = PacketHelper.NpcPacket(Text);
            Packet->OptionType = NpcOptionType.Input;
            Packet->OptionID = OptionID;
            Client.Send(Packet, Packet->Size);
            Memory.Free(Packet);
        }
        public static void Option(GameClient Client, byte OptionID, string Text)
        {
            NpcDialog* Packet = PacketHelper.NpcPacket(Text);
            Packet->OptionType = NpcOptionType.Option;
            Packet->OptionID = OptionID;
            Client.Send(Packet, Packet->Size);
            Memory.Free(Packet);
        }
    }
}
