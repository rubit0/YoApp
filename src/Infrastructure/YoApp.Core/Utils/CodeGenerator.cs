using System;

namespace YoApp.Core.Utils
{
    public class CodeGenerator
    {
        private static readonly Random RandomGenerator;

        static CodeGenerator()
        {
            RandomGenerator = new Random();
        }

        /// <summary>
        /// Returns a 2x3 digit long code string
        /// </summary>
        /// <returns></returns>
        public static string GetCode()
        {
            return $"{RandomGenerator.Next(100, 1000)}{RandomGenerator.Next(100, 1000)}";
        }
    }
}
