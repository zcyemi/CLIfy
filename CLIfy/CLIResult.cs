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
            Failed,

            Exit,
        }

        private string m_msg;
        public int ErrorCode{get;private set;} = 0;
        private ResultType m_type;

        public ResultType Type{get{return m_type;}}

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

        private CLIResult(ResultType type,string msg,int errcode)
        {
            m_type = type;
            m_msg = msg;
            this.ErrorCode = errcode;
        }

        public static CLIResult Error(string msg,CLICommandInfo cmd = null,int errCode = -1)
        {
            if(cmd != null)
            {
                msg += $"\nusage: {cmd.GetCommandFormat()}";
            }
            return new CLIResult(ResultType.Error,msg,errCode);
        }

        public static CLIResult Success(object result = null)
        {
            return new CLIResult(ResultType.Success,result == null ? null : result.ToString(),0);
        }

        public static CLIResult Failed(int errCode = -1){
            return new CLIResult(ResultType.Failed,null,errCode);
        }

        public static CLIResult Exit(int errCode = -1){
            return new CLIResult(ResultType.Exit,null,errCode);
        }


    }
}
