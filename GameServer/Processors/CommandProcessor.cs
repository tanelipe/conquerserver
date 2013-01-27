using System;
using System.Drawing;
using GameServer.Database;
namespace GameServer.Processors
{
    public unsafe class CommandProcessor
    {
        private DatabaseManager Database;
        public CommandProcessor(DatabaseManager Database)
        {
            this.Database = Database;
        }

        public bool Process(GameClient Client, string[] Input)
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
                            if(Command.Length >= 3) {
                                Client.Teleport(ushort.Parse(Command[1]), ushort.Parse(Command[2]), ushort.Parse(Command[3]));
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
                }
            }
            catch (Exception exception)
            {
                Client.Message(exception.Message, ChatType.Top, Color.White);
                Console.WriteLine(exception.ToString());
            }
            return CommandProcessed;
        }
    }
}
