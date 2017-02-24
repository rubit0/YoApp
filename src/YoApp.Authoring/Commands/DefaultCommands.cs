using System.Collections.Generic;
using System.Drawing;
using YoApp.Authoring.Helper;
using Console = Colorful.Console;

namespace YoApp.Authoring.Commands
{
    public class DefaultCommands
    {
        public static string Commands()
        {
            Console.WriteLine("Listing all commands:\n");

            var aviableCommands = new Dictionary<string, IEnumerable<string>>();
            foreach (var library in Program.CommandLibraries)
                aviableCommands.Add(library.Key, library.Value.Keys);

            foreach (var command in aviableCommands)
            {
                if(command.Key != "DefaultCommands")
                    Console.WriteLine(command.Key.Replace("Command", ""), Color.DeepSkyBlue);

                foreach (var function in command.Value)
                    Console.WriteLine("\t"+ function, Color.DarkOrange);
            }

            return string.Empty;
        }

        public static string Clear()
        {
            Console.Clear();
            ConsoleHelpers.PrintHeader();
            return string.Empty;
        }
    }
}
