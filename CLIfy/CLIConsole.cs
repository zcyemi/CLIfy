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

        public static bool? Query(string msg,string yes = "Y",string no = "N"){
            if(!msg.EndsWith('?'))msg = msg+"?";
            Console.Write($"- {msg} ({yes}/{no}): ");
            var input = Console.ReadLine();
            input = input.Trim();
            if(input.ToLower() == yes.ToLower()) return true;
            if(input.ToLower() == no.ToLower()) return false;
            return null;
        }
    }
}