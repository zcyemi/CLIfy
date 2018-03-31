using System;
using System.Collections.Generic;
using System.Text;

namespace CLIfy
{

    public class CLIOptional : Attribute
    {
        public string Key { get; private set; }
        public CLIOptional(string key = null)
        {
            this.Key = key;
        }
    }

    public class CLIHelp: Attribute
    {
        public CLIHelp(string helpinfo)
        {

        }
    }


}
