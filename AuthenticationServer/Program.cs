using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            AuthServerSocket Server = new AuthServerSocket();
            Server.Initialize();

            bool Running = true;
            while (Running)
            {
                Console.Write("> ");
                string Command = Console.ReadLine();

                if (CheckCommand("quit", Command))
                    Running = false;
            }
        }
        private static bool CheckCommand(string Command, string Input)
        {
            return Command.ToLower() == Input.ToLower();
        }
    }
}
