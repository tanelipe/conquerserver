using System;
using System.Drawing;
using GameServer.Database;
using GameServer.Scripting;
namespace GameServer.Processors
{
    public unsafe class PacketProcessor
    {
        private CommandProcessor CommandProcessor;
        private NpcScripting NpcScriptEngine;
        private DatabaseManager Database;
        

        public PacketProcessor(DatabaseManager Database)
        {
            this.Database = Database;
            CommandProcessor = new CommandProcessor(Database);
            NpcScriptEngine = new NpcScripting();
        }

        public void Process(GameClient Client, byte[] Chunk)
        {
            Client.Packets.Push(Chunk);

            byte[] Packet = Client.Packets.Pop();
            while (Packet != null)
            {
                InternalProcess(Client, Packet);
                Packet = Client.Packets.Pop();
            }
        }


        private void InternalProcess(GameClient Client, byte[] Packet)
        {          
            fixed (byte* pPacket = Packet)
            {
                ushort *Size = (ushort*)(pPacket + 0);
                ushort* Type = (ushort*)(pPacket + 2);

                if (*Size != Packet.Length)
                {
                    SizeMismatch(Client, *Size, Packet);
                    return;
                }

                Kernel.HexDump(Packet, "Client -> Server");

                switch (*Type)
                {
                    case 0x3E9: HandleCharacterCreation(Client, pPacket); break;
                    case 0x3EC: HandleChatting(Client, pPacket); break;
                    case 0x3ED: HandleMovement(Client, pPacket); break;
                    case 0x3F1: HandleItemUsage(Client, pPacket); break;
                    case 0x3F2: HandleGeneralData(Client, pPacket); break;
                    case 0x41C: HandleTransfer(Client, pPacket); break;

                    case 0x7EF: 
                    case 0x7F0:
                        HandleNpcDialogue(Client, pPacket); 
                        break;
                    
                }
            }
        }
        private void SizeMismatch(GameClient Client, ushort Size, byte[] Packet)
        {
            int RemainingSize = Packet.Length - Size;
            if (RemainingSize > 0)
            {                
                byte[] Header = new byte[Size];
                Buffer.BlockCopy(Packet, 0, Header, 0, Size);

                byte[] Footer = new byte[RemainingSize];
                Buffer.BlockCopy(Packet, Size, Footer, 0, RemainingSize);

                Process(Client, Header);
                Process(Client, Footer);
            }
        }
   
        private void HandleNpcDialogue(GameClient Client, byte* pPacket)
        {
            PacketHeader* Header = (PacketHeader*)pPacket;
            if (Header->Type == 0x7EF)
            {
                NpcInitialize* Packet = (NpcInitialize*)pPacket;
                NpcScriptEngine.Initialize(Client, Packet->UID);
            }
            else
            {
                NpcDialog* Packet = (NpcDialog*)pPacket;
                if (Packet->OptionID != 255)
                {
                    string Input = new string(Packet->Input, 1, Packet->Input[0]);
                    NpcScriptEngine.Process(Client, Packet->OptionID, Input);
                }
                else
                {
                    Client.ActiveNPC = 0;
                }
            }
        }
        private void HandleMovement(GameClient Client, byte* pPacket)
        {
            EntityMovement* Packet = (EntityMovement*)pPacket;
            if (Packet->UID == Client.Entity.UID)
            {
                ConquerAngle Direction = (ConquerAngle)(Packet->Direction % 8);
                Client.Entity.Walk(Direction);

                Client.SendScreen(Packet, Packet->Size, true);
                Kernel.GetScreen(Client, null);                
            }
            else
            {
                Client.Disconnect();
            }
        }
        private void HandleChatting(GameClient Client, byte* pPacket)
        {
            Chat* Packet = (Chat*)pPacket;
            string[] Parameters = PacketHelper.ParseChat(Packet);

            CommandAction Action = CommandAction.None;
            if ((Action = CommandProcessor.Process(Client, Parameters)) != CommandAction.None)
            {
                if (Action == CommandAction.ClearNpcScripts)
                {
                    NpcScriptEngine.Clear();
                }
                return;
            }

            switch (Packet->ChatType)
            {
                case ChatType.Talk:
                    Client.SendScreen(Packet, Packet->Size);
                    break;
                case ChatType.Whisper:
                    GameClient Receiver = EntityManager.FindByName(Parameters[1]);
                    if (Receiver != null)
                    {
                        Receiver.Send(Packet, Packet->Size);
                    }
                    else
                    {
                        Client.Message("It appears that " + Parameters[1] + " is offline", ChatType.Top, Color.White);
                    }
                    break;
            }

           // if(Packet->ChatType == ChatType.Talk)
           //     Client.SendScreen(Packet, Packet->Size);
        }
        private unsafe void HandleItemUsage(GameClient Client, byte* pPacket)
        {
            ItemUsage* Packet = (ItemUsage*)pPacket;
            switch (Packet->UsageID)
            {
                case ItemUsageIDs.Ping: Client.Send(Packet, Packet->Size); break;
            }
        }
        private unsafe void HandleGeneralData(GameClient Client, byte* pPacket)
        {
            GeneralData* Packet = (GeneralData*)pPacket;
            switch (Packet->DataID)
            {
                case GeneralDataID.SetLocation:
                    {
                        Packet->ValueA = Client.Entity.Location.X;
                        Packet->ValueB = Client.Entity.Location.Y;
                        Packet->ValueD_High = Client.Entity.Location.MapID;
                        Client.Send(Packet, Packet->Size);
                    } break;
                case GeneralDataID.Jump:
                    {
                        ushort X1 = Packet->ValueA;
                        ushort Y1 = Packet->ValueB;

                        if ((X1 != Client.Entity.Location.X) || (Y1 != Client.Entity.Location.Y))
                        {
                            //TODO: Jump hack prevention
                            Client.Disconnect();
                        }
                        else
                        {
                            ushort X2 = Packet->ValueD_High;
                            ushort Y2 = Packet->ValueD_Low;

                           
                            Client.Entity.Angle = ConquerMath.GetAngle(Client.Entity.Location, new Location() { X = X2, Y = Y2 });

                            Client.Entity.Location.X = X2;
                            Client.Entity.Location.Y = Y2;

                            Client.Send(Packet, Packet->Size);

                            Client.SendScreen(Packet, Packet->Size);
                            Kernel.GetScreen(Client, ConquerCallbackKernel.GetScreenReply);
                            
                        }                       
                    } break;
                case GeneralDataID.GetSurroundings:
                    {
                        Database.LoadEquipment(Client);

                        Client.Screen.Wipe();
                        Kernel.GetScreen(Client, ConquerCallbackKernel.GetScreenReply);


                    } break;
                case GeneralDataID.ChangeAction:
                    Client.Entity.Action = (ConquerAction)Packet->ValueD_High;
                    Client.SendScreen(Packet, Packet->Size);
                    break;
                case GeneralDataID.ChangeAngle:
                    Client.Entity.Angle = (ConquerAngle)Packet->ValueC;
                    Client.SendScreen(Packet, Packet->Size);
                    break;
                case GeneralDataID.EnterPortal:
                   
                    Client.Teleport(1002, 400, 400);
                    break;
                case GeneralDataID.ChangeAvatar:
                    {
                        if (Client.Entity.Money >= 500)
                        {
                            Client.Entity.BeginStatusUpdates();

                            Client.Entity.Money -= 500;
                            Client.Entity.Avatar = (byte)Packet->ValueD_High;

                            Client.Entity.EndStatusUpdates();
                                             
                        }
                    } break;
                default:
                    Client.Send(Packet, Packet->Size);
                    break;
            }
        }
        private unsafe void HandleCharacterCreation(GameClient Client, byte* pPacket)
        {
            CharacterCreation* Packet = (CharacterCreation*)pPacket;

            string Username = new string(Packet->Account, 0, 16).Trim('\x00');
            string Name = new string(Packet->Name, 0, 16).Trim('\x00');
            string Password = new string(Packet->Password, 0, 16).Trim('\x00');


            Database.CreateCharacter(Client, Packet->Model, Packet->Class, Name);
            
            Client.Disconnect();
        }
        private unsafe void HandleTransfer(GameClient Client, byte* Packet)
        {
            AuthMessage* Message = (AuthMessage*)Packet;
            uint Token = Message->AccountID | 0xAABB;
            Token = Message->AccountID << 8 | Message->AccountID;
            Token = Token ^ 0x4321;
            Token = Token << 8 | Token;

            if (Token == Message->LoginToken)
            {
                Client.UID = Message->AccountID;

                Client.GenerateKeys(Message->LoginToken, Message->AccountID);
                if (Database.GetCharacterData(Client))
                {
                    Chat* Response = PacketHelper.CreateChat("SYSTEM", "ALLUSERS", "ANSWER_OK");
                    Response->ChatType = ChatType.LoginInformation;
                    Response->ID = Message->AccountID;
                    Client.Send(Response, Response->Size);
                    Memory.Free(Response);  

                    CharacterInformation* Information = PacketHelper.CreateInformation(Client);
                    Client.Send(Information, Information->Size);
                    Memory.Free(Information);

                    EntityManager.Add(Client);
                    Client.Status = LoginStatus.Complete;
                }
                else
                {
                    Chat* Response = PacketHelper.CreateChat("SYSTEM", "ALLUSERS", "NEW_ROLE");
                    Response->ChatType = ChatType.LoginInformation;
                    Response->ID = Message->AccountID;
                    Client.Send(Response, Response->Size);
                    Memory.Free(Response);
                }

            }
            else
            {
                Client.Disconnect();
            }
        }
       
    }
}
