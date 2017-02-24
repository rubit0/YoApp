using System.Collections.Generic;
using System.Linq;

namespace YoApp.Authoring.Helper
{
    public class ConsoleCommand
    {
        public string Library { get; private set; }
        public string Name { get; private set; }
        public List<string> Arguments { get; private set; }

        public ConsoleCommand(string input)
        {
            Arguments = new List<string>();
            var split = input.Split(' ');

            for (int i = 0; i < split.Length; i++)
            {
                split[i] = UpperCaseFirst(split[i]);

                //Check if is library
                if (i == 0)
                {
                    if (!Program.CommandLibraries.ContainsKey(split[0] + "Command"))
                    {
                        Library = "DefaultCommands";
                        Name = split[0];
                    }
                    else
                    {
                        Library = split[0] + "Command";
                    }

                    continue;
                }

                if (i == 1 && Library != "DefaultCommands")
                {
                    Name = split[1];
                    continue;
                }

                Arguments.Add(split[i]);
            }
        }

        private string UpperCaseFirst(string input)
        {
            return char.ToUpper(input[0]) + input.Substring(1);
        }

        public override string ToString()
        {
            var args = "";
            Arguments.ForEach((a) => args += a);

            return $"Command: {Name} Arguments: {args}";
        }
    }
}