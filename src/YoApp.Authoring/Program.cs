using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using YoApp.Authoring.Helper;
using Console = Colorful.Console;

namespace YoApp.Authoring
{
    public class Program
    {
        public static Assembly Current { get; private set; }
        public static Dictionary<string, Dictionary<string, IEnumerable<ParameterInfo>>> CommandLibraries
        { get; private set; }

        public static Uri DefaultEndpoint { get; } 
            = new Uri("http://localhost:5000/");

        public static void Main(string[] args)
        {
            Init();
            Run();
        }

        private static void Init()
        {
            CommandLibraries = 
                new Dictionary<string, Dictionary<string, IEnumerable<ParameterInfo>>>();

            Current = typeof(Program).GetTypeInfo().Assembly;

            var commandClasses = Current
                .GetTypes()
                .Where((type, i) =>
                {
                    var info = type.GetTypeInfo();
                    return info.IsClass && info.Namespace == "YoApp.Authoring.Commands";
                });

            foreach (var commandClass in commandClasses)
            {
                var methods = commandClass.GetMethods(BindingFlags.Static | BindingFlags.Public);
                var methodCollection = new Dictionary<string, IEnumerable<ParameterInfo>>();

                foreach (var method in methods)
                    methodCollection.Add(method.Name, method.GetParameters());

                CommandLibraries.Add(commandClass.Name, methodCollection);
            }
        }

        private static void Run()
        {
            ConsoleHelpers.PrintHeader();

            while (true)
            {
                var input = ConsoleHelpers.ReadPrompt();
                if(string.IsNullOrWhiteSpace(input))
                    continue;

                try
                {
                    var command = new ConsoleCommand(input);
                    var result = ConsoleHelpers.Execute(command);

                    Console.WriteLine(result, Color.Beige);
                }
                catch (Exception ex)
                {
                    ConsoleHelpers.WriteExeption(ex.Message);
                }
            }
        }
    }
}
