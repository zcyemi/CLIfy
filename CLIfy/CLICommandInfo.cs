using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace CLIfy
{
    public class CLICommandInfo
    {

        public string CommandName { get; private set; }

        public List<CommandParam> Params = new List<CommandParam>();

        private MethodInfo m_method;


        private CLICommandInfo()
        {

        }

        public string GetCommandFormat()
        {
            var fmt = $"    {CommandName} ";

            List<string> fmtparam = new List<string>();
            foreach(var p in Params)
            {
                if (!p.IsOptional)
                {
                    fmtparam.Add($"<{p.Key}>");
                }
                else
                {
                    if(p.ParamType == typeof(bool))
                    {
                        fmtparam.Add($"[-{p.Key}]");
                    }
                    else
                    {
                        fmtparam.Add($"[-{p.Key} <{p.Name}>]");
                    }
                    
                }
            }
            fmt += string.Join(' ', fmtparam.ToArray());
            return fmt;
        }


        public static CLICommandInfo Parse(MethodInfo minfo)
        {
            var cmdinfo = new CLICommandInfo();
            cmdinfo.CommandName = minfo.Name;
            cmdinfo.m_method = minfo;

            var parameters = minfo.GetParameters();
            foreach (var p in parameters)
            {
                var cmdparam = new CommandParam();
                cmdparam.Key = p.Name;
                cmdparam.Name = p.Name;
                cmdparam.ParamType = p.ParameterType;
                var optionalAttr = Attribute.GetCustomAttribute(p, typeof(CLIOptional)) as CLIOptional;
                cmdparam.IsOptional = optionalAttr != null;
                if(optionalAttr != null && optionalAttr.Key != null)
                {
                    cmdparam.Key = optionalAttr.Key;
                }
                if (p.HasDefaultValue)
                {
                    cmdparam.DefaultValue = p.DefaultValue;
                }
                else
                {
                    if (cmdparam.IsOptional) throw new Exception("Optional parameter must have default value.");
                }
                cmdinfo.Params.Add(cmdparam);

            }
            return cmdinfo;
        }


        public bool IsMatch(CommandCall call,bool strict = false)
        {
            return IsEntryMatch(call.Entry, strict);
        }

        public bool IsEntryMatch(string entry,bool strict = false)
        {
            if (strict)
            {
                return entry == CommandName;
            }
            if (entry.ToLower() == CommandName.ToLower()) return true;
            return false;
        }


        public CLIResult Execute(CommandCall call)
        {
            List<object> fill = new List<object>();

            var callreqIndex = 0;

            foreach (var p in Params)
            {
                if (!p.IsOptional)
                {
                    if (callreqIndex < call.ParamRequired.Count)
                    {
                        var fillobj = call.ParamRequired[callreqIndex];

                        object ret = null;
                        var convsuc = ConvertParameter(fillobj, p.ParamType, out ret);
                        if (convsuc)
                        {
                            fill.Add(ret);
                            callreqIndex++;
                        }
                        else
                        {
                            return CLIResult.Error($"Can not convert '{fillobj}' to parameter [{p.Key}({p.ParamType.ToString()})]");
                        }

                    }
                    else
                    {
                        return CLIResult.Error($"Param '{p.Key}' is required.",this);
                    }
                }
                else
                {
                    var options = call.Options;
                    var opt = options.Keys.FirstOrDefault((o) => { return o.ToLower() == p.Key.ToLower(); });
                    if(opt == null)
                    {
                        fill.Add(p.DefaultValue);
                    }
                    else
                    {
                        var calloption = options[opt];

                        if(p.ParamType == typeof(bool))
                        {
                            if (string.IsNullOrEmpty(calloption))
                            {
                                fill.Add(true);
                            }
                            else
                            {
                                if(calloption.ToLower() == "true")
                                {
                                    fill.Add(true);
                                }
                                else if(calloption.ToLower() == "false")
                                {
                                    fill.Add(false);
                                }
                                else
                                {
                                    return CLIResult.Error($"Parse optional parameter '{p.Key}' failed width '{calloption}'");
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(calloption))
                            {
                                return CLIResult.Error($"Optional parameter '{p.Key}' missing values.",this);
                            }

                            object ret = null;
                            var suc = ConvertParameter(calloption, p.ParamType, out ret);
                            if (suc)
                            {
                                fill.Add(ret);
                            }
                            else
                            {
                                return CLIResult.Error($"Can not convert '{calloption}' to parameter [{p.Key}({p.ParamType.ToString()})]");
                            }
                        }
                    }
                }
            }

            object callresult = null;
            try
            {
                callresult = m_method.Invoke(null, fill.ToArray());
            }
            catch(Exception e)
            {
                return CLIResult.Error($"[Exception]Command <{CommandName}> : {e.Message}",this);
            }

            if (callresult == null)
            {
                return CLIResult.Success();
            }
            
            if(callresult is CLIResult)
            {
                return (CLIResult)callresult;
            }

            return CLIResult.Success(callresult);
        }


        private static bool ConvertParameter(string rawobj, Type t, out object ret)
        {
            ret = null;

            if(t == typeof(int))
            {
                int res = 0;
                if(int.TryParse(rawobj, out res))
                {
                    ret = res;
                    return true;
                }
                return false;
            }
            else if(t == typeof(float))
            {
                float res = 0;
                if (float.TryParse(rawobj, out res))
                {
                    ret = res;
                    return true;
                }
                return false;
            }
            else if (t == typeof(double))
            {
                double res = 0;
                if (double.TryParse(rawobj, out res))
                {
                    ret = res;
                    return true;
                }
                return false;
            }
            else if (t == typeof(bool))
            {
                bool res = false;
                if (bool.TryParse(rawobj, out res))
                {
                    ret = res;
                    return true;
                }
                return false;
            }
            else if(t == typeof(string))
            {
                ret = rawobj;
                return true;
            }


            return false;
        }


    }
}
