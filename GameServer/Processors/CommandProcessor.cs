using System;
using System.Drawing;
using GameServer.Database;
using System.Collections;
using System.Collections.Generic;
namespace GameServer.Processors
{
    public enum CommandAction
    {
        None,
        Processed,
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


            CommandAction Action = CommandAction.None;
            if (Command[0].StartsWith("@"))
                Action = CommandAction.Processed;

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

                            Client.Teleport(MapID, X, Y);

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
                    case "@spell":
                        {
                            LearnSpell Spell = LearnSpell.Create();
                            Spell.ID = ushort.Parse(Command[1]);
                            Spell.Level = ushort.Parse(Command[2]);
                            Client.Send(&Spell, Spell.Size);
                        } break;
                    case "@job":
                        {
                            byte Job = byte.Parse(Command[1]);
 
                    

                            Client.Entity.BeginStatusUpdates();
                            Client.Entity.AddStatusUpdate(StatusUpdateEntry.Create(ConquerStatusIDs.Job, Job));
                            Client.Entity.EndStatusUpdates();


                            /*
Trojan = [10, 15]
Warrior = [20, 25]
Archer = [40, 45]
Taoist = 100 | 101 | [132 - 135] | [142 - 145]
                             * */
                        } break;
                    case "@reload_npc":
                        {
                            return CommandAction.ClearNpcScripts;
                        }
                }
            }
            catch (Exception exception)
            {
                Client.Message(exception.Message, ChatType.Top, Color.White);
                Console.WriteLine(exception.ToString());
            }
            return Action;
        }
    }
}
