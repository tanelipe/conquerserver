using System;
using System.Drawing;
using GameServer.Database;
namespace GameServer.Processors
{
    public enum CommandAction
    {
        None,
        ClearNpcScripts
    }
    public unsafe class CommandProcessor
    {
        private DatabaseManager Database;
        public CommandProcessor(DatabaseManager Database)
        {
            this.Database = Database;
        }

        public CommandAction Process(GameClient Client, string[] Input)
        {
            string From = Input[0];
            string To = Input[1];
            string Message = Input[3];

            string[] Command = Message.Split(' ');


            bool CommandProcessed = false;
            if (Command[0].StartsWith("@"))
                CommandProcessed = true;

            try
            {
                switch (Command[0])
                {
                    case "@quit":
                        {
                            Client.Disconnect();
                        } break;
                    case "@mm":
                        {
                            ushort MapID = ushort.Parse(Command[1]);
                            ushort X = ushort.Parse(Command[2]);
                            ushort Y = ushort.Parse(Command[3]);

                            if (Kernel.IsWalkable(MapID, X, Y))
                            {
                                Client.Teleport(MapID, X, Y);
                            }
                            else
                            {
                                Client.Message(string.Format("You can't teleport to {0} {1} {2}", MapID, X, Y), ChatType.Top, Color.Red);
                            }
                        } break;
                    case "@gold":
                        {
                            Client.Entity.Money = uint.Parse(Command[1]);
                        } break;
                    case "@item":
                        {
                            if (Command.Length > 2)
                            {
                                ConquerItem Item = new ConquerItem(Client, Database.GetItemDetail(Command[1], Command[2]));
                                Item.Position = ItemPosition.Inventory;
                                if (Command.Length > 3)
                                {
                                    Item.Plus = byte.Parse(Command[3]);
                                    if (Command.Length > 4)
                                    {
                                        Item.SocketOne = byte.Parse(Command[4]);
                                        if (Command.Length > 5)
                                        {
                                            Item.SocketTwo = byte.Parse(Command[5]);
                                        }
                                    }
                                }
                                Client.AddInventory(Item);
                            }
                        } break;
                    case "@prof":
                        {
                            LearnProfiency Profiency = LearnProfiency.Create();
                            Profiency.ID = uint.Parse(Command[1]);
                            Profiency.Level = uint.Parse(Command[2]);
                            Client.Send(&Profiency, Profiency.Size);
                        } break;
                    case "@reload_npc":
                        {
                            return CommandAction.ClearNpcScripts;
                        } break;
                }
            }
            catch (Exception exception)
            {
                Client.Message(exception.Message, ChatType.Top, Color.White);
                Console.WriteLine(exception.ToString());
            }
            return CommandAction.None;
        }
    }
}
