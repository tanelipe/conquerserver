using GameServer.Database;

namespace GameServer
{
    public unsafe class GeneralDataProcessor : IPacketProcessor
    {
        public GeneralDataProcessor(DatabaseManager Database)
            : base(Database)
        {

        }

        public override void Execute(GameClient Client, byte* pPacket)
        {
            GeneralData* Packet = (GeneralData*)pPacket;
            switch (Packet->DataID)
            {
                case GeneralDataID.SetLocation: HandleSetLocation(Client, Packet); break;
                case GeneralDataID.Jump: HandleJumping(Client, Packet); break;
                case GeneralDataID.GetSurroundings: HandleGetSurroundings(Client, Packet); break;
                case GeneralDataID.ChangeAction: HandleChangeAction(Client, Packet); break;
                case GeneralDataID.ChangeAngle: HandleChangeAngle(Client, Packet); break;
                case GeneralDataID.EnterPortal: HandlePortal(Client, Packet); break;
                case GeneralDataID.ChangeAvatar: HandleChangeAvatar(Client, Packet); break;
                default:
                    Client.Send(Packet, Packet->Size);
                    break;
            }
        }
        private void HandleChangeAvatar(GameClient Client, GeneralData* Packet)
        {
            if (Client.Entity.Money >= 500)
            {
                Client.Entity.BeginStatusUpdates();

                Client.Entity.Money -= 500;
                Client.Entity.Avatar = (byte)Packet->ValueD_High;

                Client.Entity.EndStatusUpdates();

            }
        }
        private void HandlePortal(GameClient Client, GeneralData* Packet)
        {
            Client.Teleport(1002, 400, 400);
        }
        private void HandleChangeAngle(GameClient Client, GeneralData* Packet)
        {
            Client.Entity.Angle = (ConquerAngle)Packet->ValueC;
            Client.SendScreen(Packet, Packet->Size);
        }
        private void HandleChangeAction(GameClient Client, GeneralData* Packet)
        {
            Client.Entity.Action = (ConquerAction)Packet->ValueD_High;
            Client.SendScreen(Packet, Packet->Size);
        }
        private void HandleGetSurroundings(GameClient Client, GeneralData* Packet)
        {
            Database.LoadEquipment(Client);

            Client.Screen.Wipe();
            Kernel.GetScreen(Client, ConquerCallbackKernel.GetScreenReply);
        }
        private void HandleSetLocation(GameClient Client, GeneralData* Packet)
        {
            Packet->ValueA = Client.Entity.Location.X;
            Packet->ValueB = Client.Entity.Location.Y;
            Packet->ValueD_High = Client.Entity.Location.MapID;
            Client.Send(Packet, Packet->Size);
        }
        private void HandleJumping(GameClient Client, GeneralData* Packet)
        {
            ushort X1 = Packet->ValueA;
            ushort Y1 = Packet->ValueB;

            if ((X1 != Client.Entity.Location.X) || (Y1 != Client.Entity.Location.Y))
            {
                Client.Disconnect();
            }
            else
            {
                ushort X2 = Packet->ValueD_High;
                ushort Y2 = Packet->ValueD_Low;

                Client.Entity.Angle = ConquerMath.GetAngle(Client.Entity.Location, new Location() { X = X2, Y = Y2 });

                Client.Entity.Location.X = X2;
                Client.Entity.Location.Y = Y2;

                if (!Kernel.IsWalkable(Client.Entity.Location.MapID,
                    Client.Entity.Location.X, Client.Entity.Location.Y))
                {
                    Client.Disconnect();
                }
                else
                {

                    Client.Send(Packet, Packet->Size);

                    Client.SendScreen(Packet, Packet->Size);
                    Kernel.GetScreen(Client, ConquerCallbackKernel.GetScreenReply);
                }
            }
        }
    }
}
