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
        ViewEquipment = 0xAD,
        Hotkeys = 0x8A,
        Entity = 0xA2,
        RemoveEntity = 0x8D,
        ConfirmFriends = 0x8B,
        EnterPortal = 0x82,
        ChangeMap = 0x83,
        Revive = 0x94,
        LevelUp = 0x92,
        DeleteCharacter = 0x95,
        ConfirmSpells = 0x96,
        ConfirmGuild = 0x97,
        ChangePKMode = 0x98,
        CompleteLogin = 0x99,
        Dead = 0x9A,
        StartVend = 0xA7,  
        ChangeAngle = 0x7C,
        ChangeAction = 0x7E,
        Jump = 0x8E,
        SetLocation = 0x89,
        GetSurroundings = 0xAA,
        GUINpcInterface = 0xBA,
        None = 0x00,
        RequestFriendInfo = 0x9C,
        Mining = 0x9F, 
        RequestEnemyInfo = 0xB3, 
        ChangeAvatar = 0x9E, 
        EndFly = 0x78,
        Switch = 0x74
    }
}