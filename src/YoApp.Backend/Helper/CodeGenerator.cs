using System;

namespace YoApp.Backend.Helper
{
    public static class CodeGenerator
    {
        private static readonly Random RandomGenerator;

        static CodeGenerator()
        {
            RandomGenerator = new Random();
        }

        /// <summary>
        /// Returns a 2x3 digit long code as string
        /// </summary>
        /// <returns></returns>
        public static string GetCode()
        {
            return $"{RandomGenerator.Next(100, 1000)}{RandomGenerator.Next(100, 1000)}";
        }
    }
}
