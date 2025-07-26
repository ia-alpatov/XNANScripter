using System.Collections.Generic;

namespace XNANScripter.Engine.Variables
{
    internal class NsaVariable : Variable
    {
        public NsaVariable(int p)
        {
            parameters = new List<string>();
            parameters.Add(p.ToString());
        }

        public string name { get { return "nsa"; } set { } }

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