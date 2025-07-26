using System.Collections.Generic;

namespace XNANScripter.Engine.Variables
{
    internal class TextspeedVariable : Variable
    {
        public TextspeedVariable(string p)
        {
            parameters = new List<string>();
            parameters.Add(p);
        }

        public string name { get { return "textspeed"; } set { } }

        public List<string> parameters { get; set; }

        public string Get()
        {
            return parameters[0];
        }

        public bool Set(string _params)
        {
            parameters[0] = _params;
            return true;
        }
    }
}