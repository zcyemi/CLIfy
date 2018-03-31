using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
namespace CLIfy
{

    public enum CLIstyle
    {
        DOS,
        Unix,
    }


    public class CLIApp
    {

        private List<CLICommandInfo> m_cmds = new List<CLICommandInfo>();
        private CLIParserBase m_parser;
        public CLIstyle Style { get; private set; }
        public string Name { get; private set; }
        public bool StrictMode;

        public List<CLICommandInfo> Commands { get { return m_cmds; } }

        public CLIApp(string name,bool strict = false,CLIstyle style = CLIstyle.Unix)
        {
            this.Name = name;
            this.Style = style;
            this.StrictMode = strict;

            if(style == CLIstyle.Unix)
            {
                m_parser = new CLIParserUnix(this);
            }
            else
            {
                m_parser = new CLIParserDOS(this);
            }
        }


        public string GetAppHelp()
        {
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.AppendLine("Commands:");
            foreach (var cmd in m_cmds)
            {
                strbuilder.AppendLine(cmd.GetCommandFormat());
            }

            return strbuilder.ToString();
        }


        internal void RegisterCLI<T>(Func<CLIApp,T,CLIResult> internalcmd)
        {
            Register(internalcmd.GetMethodInfo());
        }

        internal void RegisterCLI(Func<CLIApp,CLIResult> cmd)
        {
            Register(cmd.GetMethodInfo());
        }



        public void Register<P0, P1>(Action<P0, P1> method)
        {
            Register(method.GetMethodInfo());
        }

        public void Register<P0, P1, P2>(Action<P0, P1,P2> method)
        {
            Register(method.GetMethodInfo());
        }

        public void Register<P0>(Action<P0> method)
        {
            Register(method.GetMethodInfo());
        }
        public void RegisterFunction<T0,T1,T2>(Func<T0,T1,T2> method)
        {
            Register(method.GetMethodInfo());
        }



        public void Register(MethodInfo methodinfo)
        {
            var method = CLICommandInfo.Parse(methodinfo);
            m_cmds.Add(method);

        }

        public void Run(string[] args)
        {
            Console.WriteLine($"[{Name}] - ({Style.ToString()})");

            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                if (CheckExit(input))
                {
                    break;
                }
                else
                {
                    var cmdcall = ParseLine(input);
                    ExecuteCommandCall(cmdcall);
                }
            }
        }


        private void ExecuteCommandCall(CommandCall call)
        {
            if (call == null)
            {
                //helper or other things
                CLIResult.Success(GetAppHelp()).PrintWithColor(ConsoleColor.DarkGray);
            }
            else
            {
                foreach (var m in m_cmds)
                {
                    if (m.IsMatch(call,StrictMode))
                    {
                        if (call.HasError)
                        {
                            Print(call.ParseError, m);
                            return;
                        }

                        var result = m.Execute(call);
                        Print(result);
                        return;
                    }
                }
                Print(CLIResult.Error($"Command not found '{call.Entry}'"));
            }
        }

        private void Print(string error, CLICommandInfo cmd)
        {
            var cliresult = CLIResult.Error(error);
            cliresult.Print();
        }


        private void Print(CLIResult result)
        {
            result.Print();
        }



        private bool CheckExit(string input)
        {
            return false;
        }

        private CommandCall ParseLine(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            var splits = input.Split(' ');
            if (splits.Length == 0) return null;

            var entry = splits[0];

            return m_parser.ParseLine(entry, splits);
        }




    }
}
