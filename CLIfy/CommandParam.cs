using System;
using System.Collections.Generic;
using System.Text;

namespace CLIfy
{
    public struct CommandParam
    {
        public Type ParamType;
        public Object DefaultValue;
        public string Key;
        public string Name;
        public bool IsOptional;
    }
}
