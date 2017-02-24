using System;
using YoApp.Authoring.Commands;
using YoApp.Authoring.Helper;

namespace YoApp.Authoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConsoleHelpers.PrintHeader("YoApp Authoring");

            Run();

            var task = RegisterUsersCommand.RegisterUsers();
            task.Wait();
            if (task.IsFaulted)
                Console.WriteLine("Error. Task could not be completed!");

            Console.ReadLine();
        }

        private static void Run()
        {
            while (true)
            {
                var input = ConsoleHelpers.ReadPrompt();
                if(string.IsNullOrWhiteSpace(input))
                    continue;

            }
        }
    }
}
