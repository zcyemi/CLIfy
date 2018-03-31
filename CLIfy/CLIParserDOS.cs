using System;
using System.Collections.Generic;
using System.Text;

namespace CLIfy
{
    public class CLIParserDOS : CLIParserBase
    {
        public CLIParserDOS(CLIApp app) : base(app) { }

        public override CommandCall ParseLine(string entry, string[] splits)
        {
            var cmdCall = new CommandCall();
            cmdCall.Entry = entry;

            string lastOptStr = null;
            bool firstOptionMeet = false;

            for (var i = 1; i < splits.Length; i++)
            {
                var si = splits[i];

                if (!si.StartsWith('/'))
                {
                    if (string.IsNullOrEmpty(lastOptStr))
                    {
                        if (firstOptionMeet)
                        {
                            cmdCall.SetError($"Can not parse params `{si}`");
                        }
                        else
                        {
                            cmdCall.AddRequired(si);
                        }

                    }
                    else
                    {
                        cmdCall.AddOption(lastOptStr, si);
                        lastOptStr = null;
                    }

                }
                else
                {
                    firstOptionMeet = true;
                    si = si.Substring(1, si.Length - 1);
                    if (!string.IsNullOrEmpty(lastOptStr))
                    {
                        if (lastOptStr.Contains(":"))
                        {
                            var optsplit = lastOptStr.Split(':');
                            if (optsplit.Length != 2 || optsplit[1].Length == 0)
                            {
                                cmdCall.SetError($"Can not parse `{lastOptStr}`");
                            }
                            else
                            {
                                cmdCall.AddOption(optsplit[0], optsplit[1]);
                            }
                        }
                        else
                        {
                            cmdCall.AddOption(lastOptStr, null);
                        }
                    }
                    lastOptStr = si;
                }
            }

            if (!string.IsNullOrEmpty(lastOptStr))
            {
                if (lastOptStr.Contains(":"))
                {
                    var optsplit = lastOptStr.Split(':');
                    if (optsplit.Length != 2 || optsplit[1].Length == 0)
                    {
                        cmdCall.SetError($"Can not parse `{lastOptStr}`");
                    }
                    else
                    {
                        cmdCall.AddOption(optsplit[0], optsplit[1]);
                    }
                }
                else
                {
                    cmdCall.AddOption(lastOptStr, null);
                }
            }
            return cmdCall;
        }
    }
}
