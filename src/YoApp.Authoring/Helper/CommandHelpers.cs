using System.Linq;
using System.Reflection;

namespace YoApp.Authoring.Helper
{
    public class CommandHelpers
    {
        public static string TryExecute(ConsoleCommand command)
        {
            if (!Program.CommandLibraries[command.Library].ContainsKey(command.Name))
                return "Unknown command.";

            //Has command
            var methodDictionary = Program.CommandLibraries[command.Library];
            if (!methodDictionary.ContainsKey(command.Name))
                return "Unknown command.";

            //Do arguments count match
            if (command.Arguments.Count != methodDictionary[command.Name].Count())
                return $"Arguments do match for command {command.Name}";

            //Get the class
            var commandLibaryClass = Program.Current
                .GetType($"YoApp.Authoring.Commands.{command.Library}");

            //Create Method
            var method = commandLibaryClass.GetMethod(command.Name,
                BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public);

            //Enter arguments
            object[] inputArgs = null;
            if (command.Arguments.Count > 0)
                inputArgs = command.Arguments.ToArray();

            return method.Invoke(commandLibaryClass, inputArgs).ToString();
        }
    }
}
