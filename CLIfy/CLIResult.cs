using System;
using System.Collections.Generic;
using System.Text;

namespace CLIfy
{
    public class CLIResult
    {
        public enum ResultType
        {
            Success,
            Error,
        }

        private string m_msg;
        private ResultType m_type;

        public void Print()
        {
            var color = m_type == ResultType.Error ? ConsoleColor.DarkRed : ConsoleColor.White;
            PrintWithColor(color);
        }

        public void PrintWithColor(ConsoleColor color)
        {
            var lastcolor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            if (!string.IsNullOrEmpty(m_msg))
            {
                Console.WriteLine(m_msg);
            }

            Console.ForegroundColor = lastcolor;
        }

        private CLIResult(ResultType type,string msg)
        {
            m_type = type;
            m_msg = msg;
        }

        public static CLIResult Error(string msg,CLICommandInfo cmd = null)
        {
            if(cmd != null)
            {
                msg += $"\nusage: {cmd.GetCommandFormat()}";
            }
            return new CLIResult(ResultType.Error,msg);
        }

        public static CLIResult Success(object result = null)
        {
            return new CLIResult(ResultType.Success,result == null ? null : result.ToString());
        }


    }
}
