using System;

namespace HRMapp.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            for (;;)
            {
                CommandHandler.HandleCommand(Console.ReadLine());
            }
        }
    }
}
