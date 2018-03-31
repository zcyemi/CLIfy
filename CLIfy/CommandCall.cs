using System;
using System.Collections.Generic;
using System.Text;

namespace CLIfy
{
    public class CommandCall
    {
        public string Entry;
        public Dictionary<string, string> Options = new Dictionary<string, string>();
        public List<string> ParamRequired = new List<string>();
        public string ParseError;

        public bool HasError
        {
            get { return !string.IsNullOrEmpty(ParseError); }
        }

        public void SetError(string info)
        {
            if (string.IsNullOrEmpty(ParseError))
            {
                ParseError = info;
            }
        }

        public void AddOption(string option,string value)
        {
            if (Options.ContainsKey(option))
            {
                SetError($"Duplicate option `{option}`");
            }
            else
            {
                Options.Add(option, value);
            }
        }

        public void AddRequired(string param)
        {
            ParamRequired.Add(param);
        }


    }
}
