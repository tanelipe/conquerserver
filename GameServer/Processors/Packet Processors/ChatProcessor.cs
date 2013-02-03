using GameServer.Database;
using GameServer.Processors;
using GameServer.Scripting;
using System.Drawing;
namespace GameServer
{
    public unsafe class ChatProcessor : IPacketProcessor
    {
        private CommandProcessor CommandProcessor;
        private NpcScripting NpcScriptEngine;

        public ChatProcessor(DatabaseManager Database, CommandProcessor CommandProcessor, NpcScripting NpcScriptEngine)
            : base(Database)
        {
            this.CommandProcessor = CommandProcessor;
            this.NpcScriptEngine = NpcScriptEngine;
        }
        public override void Execute(GameClient Client, byte* pPacket)
        {
            Chat* Packet = (Chat*)pPacket;
            string[] Parameters = PacketHelper.ParseChat(Packet);

            CommandAction Action = CommandAction.None;
            if ((Action = CommandProcessor.Process(Client, Parameters)) != CommandAction.None)
            {
                if (Action == CommandAction.Processed)
                {

                }
                else if (Action == CommandAction.ClearNpcScripts)
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
        }
    }
}
