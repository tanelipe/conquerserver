
namespace GameServer
{
    public enum ChatType : uint
    {
        Talk = 2000,
        Whisper = 2001,
        Action = 2002,
        Team = 2003,
        Guild = 2004,
        Top = 2005,
        Spouse = 2006,
        Yell = 2008,
        Friend = 2009,
        Broadcast = 2010,
        Center = 2011,
        Ghost = 2013,
        Service = 2014,
        Dialog = 2100,
        LoginInformation = 2101,
        VendorHawk = 2104,
        TopRight = 2109,
        ClearTopRight = 2108,
        FriendsOfflineMessage = 2110,
        GuildBulletin = 2111,
        TradeBoard = 2201,
        FriendBoard = 2202,
        NewBroadcast = 2500,
        TeamBoard = 2203,
        GuildBoard = 2204,
        OthersBoard = 2205
    }
    public unsafe struct Chat
    {
        public ushort Size;
        public ushort Type;
        public uint Color;
        public ChatType ChatType;
        public uint ID;
        public byte Count;
        public fixed sbyte Data[256];
    }
 
}