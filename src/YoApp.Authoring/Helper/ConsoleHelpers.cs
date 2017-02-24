using System.Drawing;
using System.Reflection;
using Console = Colorful.Console;

namespace YoApp.Authoring.Helper
{
    public class ConsoleHelpers
    {
        public static void PrintHeader(string text = "YoApp Authoring")
        {
            Console.WriteAscii(text, Color.FromArgb(42, 223, 98));
            Console.WriteLine("Welcome to the wonderful YoApp Authoring Interface!", Color.FromArgb(42, 223, 48));
            Console.WriteLine("Type 'commands' to list all actions.", Color.FromArgb(42, 223, 177));
            Console.WriteLine($"Default connection: {Program.DefaultEndpoint}", Color.FromArgb(228, 67, 171));
            Console.WriteLine("----------\n");
        }

        private const string Prefix = "=> ";
        public static string ReadPrompt()
        {
            Console.Write(Prefix, Color.Beige);
            return Console.ReadLine();
        }

        public static string Execute(ConsoleCommand command)
        {
            try
            {
                return CommandHelpers.TryExecute(command);
            }
            catch (TargetInvocationException ex)
            {
                WriteExeption(ex.Message);
                return string.Empty;
            }
        }

        public static void WriteExeption(string message)
        {
            Console.WriteLine(message, Color.IndianRed);
        }
    }
}
