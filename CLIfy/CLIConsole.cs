using System;

namespace CLIfy
{


    public static class CLIConsole
    {
        public static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Error] {msg}");
            Console.ResetColor();
        }

        public static void Log(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[Log] {msg}");
            Console.ResetColor();
        }

        public static void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[Success] {msg}");
            Console.ResetColor();
        }


        public static void Exception(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Exception] {msg}");
            Console.ResetColor();
        }

        public static void Exception(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Exception] {e.Message} \n {e.StackTrace}");
            Console.ResetColor();
        }
    }
}