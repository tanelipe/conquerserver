using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
namespace GameServer
{
    [StructLayout(LayoutKind.Explicit)]
    public struct GeneralData
    {
        [FieldOffset(0)]
        public ushort Size;
        [FieldOffset(2)]
        public ushort Type;
        [FieldOffset(4)]
        public uint Timer;
        [FieldOffset(8)]
        public uint UID;
        [FieldOffset(12)]
        public ushort ValueA;
        [FieldOffset(14)]
        public ushort ValueB;
        [FieldOffset(16)]
        public ushort ValueC;
        [FieldOffset(20)]
        public uint ValueD;
        [FieldOffset(20)]
        public ushort ValueD_High;
        [FieldOffset(22)]
        public ushort ValueD_Low;
        [FieldOffset(24)]
        public GeneralDataID DataID;

    }
    public enum GeneralDataID : uint
    {
        ViewEquipment = 0xAD,//173
        Hotkeys = 0x8A,//138
        Entity = 0xA2,//162
        //0x66 = 102 <- Newer client maybe? D:
        //SpawnEntity = 0x89,//137 (SAME AS SETLOCATION)
        RemoveEntity = 0x8D,//141
        ConfirmFriends = 0x8B,//139
        EnterPortal = 0x82,//130
        ChangeMap = 0x83,//131
        Revive = 0x94,//148
        LevelUp = 0x92,
        DeleteCharacter = 0x95,//149
        ConfirmSpells = 0x96,//150
        ConfirmGuild = 0x97,//151
        ChangePKMode = 0x98,//152
        CompleteLogin = 0x99,//153
        Dead = 0x9A,//154
        StartVend = 0xA7, //167 
        ChangeAngle = 0x7C,//124
        ChangeAction = 0x7E,//126
        Jump = 0x8E,//142
        SetLocation = 0x89,//137
        GetSurroundings = 0xAA,//170
        GUINpcInterface = 0xBA, //186
        None = 0x00,//0
        RequestFriendInfo = 0x9C, //156
        Mining = 0x9F, //159
        RequestEnemyInfo = 0xB3, //179
        ChangeAvatar = 0x9E, // 158
        EndFly = 0x78,// Wrong
        Switch = 0x74//116
    }
}