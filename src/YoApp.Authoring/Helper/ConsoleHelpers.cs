using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using Console = Colorful.Console;

namespace YoApp.Authoring.Helper
{
    public class ConsoleHelpers
    {
        public static void PrintHeader(string text)
        {
            Console.WriteAscii(text, Color.FromArgb(42, 223, 98));
            Console.WriteLine("Welcome to the wonderful YoApp Authoring Interface!", Color.FromArgb(42, 223, 48));
            Console.WriteLine("Try 'help' in case of panic.", Color.FromArgb(42, 223, 177));
            Console.WriteLine("Default connection: http://localhost:5000", Color.FromArgb(228, 67, 171));
            Console.WriteLine("----------\n");
        }

        public static void PrintInfo()
        {
            Console.WriteLine("Connection", Color.FromArgb(42, 223, 48));
            Console.WriteLine("Try 'help' in case of panic.", Color.FromArgb(42, 223, 177));
            Console.WriteLine("----------\n");
        }

        private const string _prefix = "=>";
        public static string ReadPrompt()
        {
            Console.Write(_prefix, Color.Beige);
            return Console.ReadLine();
        }

        public static void Execute(string command)
        {
            Console.WriteLine("Executing some command...");
        }
    }
}
