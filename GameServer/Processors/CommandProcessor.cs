using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Processors
{
    public class CommandProcessor
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

            switch (Command[0])
            {
                case "@quit":
                    {
                        Client.Disconnect();
                    } break;
            }
            return CommandProcessed;
        }
    }
}
