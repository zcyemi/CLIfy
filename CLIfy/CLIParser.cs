using System;
using System.Collections.Generic;
using System.Text;

namespace CLIfy
{
    public abstract class CLIParserBase
    {
        protected CLIApp m_app;

        public CLIParserBase(CLIApp app)
        {
            this.m_app = app;
        }

        public abstract CommandCall ParseLine(string entry, string[] splits);

    }
}
