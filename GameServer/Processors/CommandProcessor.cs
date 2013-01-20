using System;
namespace GameServer.Processors
{
    public unsafe class CommandProcessor
    {
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
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            return CommandProcessed;
        }
    }
}
