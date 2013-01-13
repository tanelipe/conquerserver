using System.Collections.Generic;

namespace GameServer
{
    public unsafe class PacketConstructor
    {
        public static byte[] StringPayload(params string[] Messages)
        {
            List<byte> Payload = new List<byte>();

            for (int i = 0; i < Messages.Length; i++)
            {
                string Message = Messages[i];

                Payload.Add((byte)Message.Length);
                for (int j = 0; j < Message.Length; j++)
                {
                    Payload.Add((byte)Message[j]);
                }
            }
            return Payload.ToArray();
        }

        public static CharacterInformation *CreateInformation(GameClient Client)
        {
            Character Character = Client.Character;
            byte[] Payload = StringPayload(Character.Name, Character.Spouse);

            int Size = 66 + Payload.Length;
            CharacterInformation* Packet = (CharacterInformation*)Memory.Alloc(Size);
            
            Packet->Size = (ushort)Size;
            Packet->Type = 0x3EE;

            Packet->ID = Character.ID;
            Packet->Model = Character.Model;
            Packet->HairStyle = Character.HairStyle;
            Packet->Gold = Character.Gold;
            Packet->Experience = Character.Experience;
            Packet->StatPoints = Character.StatPoints;
            Packet->Strength = Character.Strength;
            Packet->Dexterity = Character.Dexterity;
            Packet->Vitality = Character.Vitality;
            Packet->Spirit = Character.Spirit;
            Packet->HitPoints = Character.HitPoints;
            Packet->ManaPoints = Character.ManaPoints;
            Packet->PKPoints = Character.PKPoints;
            Packet->Level = Character.Level;
            Packet->Class = Character.Class;
            Packet->Reborn = Character.Reborn;
            Packet->DisplayName = true;
            Packet->NameCount = 2;

            fixed (byte* pPayload = Payload)
            {
                Memory.Copy(pPayload, Packet->Names, Payload.Length);
            }    
            return Packet;
        }

        public static Chat* CreateChat(string From, string To, string Message)
        {
            int Size = 24 + From.Length + To.Length + Message.Length;
            Chat* Packet = (Chat*)Memory.Alloc(Size+1);
            Packet->Size = (ushort)Size;
            Packet->Type = 0x3EC;
            Packet->Count = 4;

            byte[] Payload = StringPayload(From, "", To, Message);

            fixed (byte* pPayload = Payload)
            {
                Memory.Copy(pPayload, Packet->Data, Payload.Length);
            }
            return Packet;
        }
    }
}
