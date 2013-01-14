using System.Collections.Generic;

namespace GameServer
{
    public unsafe class PacketHelper
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
            
            byte[] Payload = StringPayload(Client.Entity.Name, Client.Entity.Spouse);

            int Size = 66 + Payload.Length;
            CharacterInformation* Packet = (CharacterInformation*)Memory.Alloc(Size);
            
            Packet->Size = (ushort)Size;
            Packet->Type = 0x3EE;

            Packet->ID = Client.Entity.UID;
            Packet->Model = Client.Entity.Mesh;
            Packet->HairStyle = Client.Entity.HairStyle;
            Packet->Gold = Client.Entity.Gold;
            Packet->Experience = Client.Entity.Experience;
            Packet->StatPoints = Client.Entity.StatusPoints.Free;
            Packet->Strength = Client.Entity.StatusPoints.Strength;
            Packet->Dexterity = Client.Entity.StatusPoints.Dexterity;
            Packet->Vitality = Client.Entity.StatusPoints.Vitality;
            Packet->Spirit = Client.Entity.StatusPoints.Spirit;
            Packet->HitPoints = Client.Entity.HitPoints;
            Packet->ManaPoints = Client.Entity.ManaPoints;
            Packet->PKPoints = Client.Entity.PKPoints;
            Packet->Level = Client.Entity.Level;
            Packet->Class = Client.Entity.Class;
            Packet->Reborn = Client.Entity.Reborn;
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
        public static string[] ParseChat(Chat* Packet)
        {
            List<string> Parameters = new List<string>();
            for (int i = 0, Index = 0; i < Packet->Count; i++)
            {
                string Parameter = new string(Packet->Data, Index + 1, Packet->Data[Index]);
                Index = Index + (Parameter.Length + 1);
                Parameters.Add(Parameter);
            }
            return Parameters.ToArray();
        }
    }
}
