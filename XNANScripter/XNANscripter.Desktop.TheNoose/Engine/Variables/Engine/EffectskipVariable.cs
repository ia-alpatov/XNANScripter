using System.Collections.Generic;

namespace XNANScripter.Engine.Variables
{
    internal class EffectskipVariable : Variable
    {
        public EffectskipVariable(string p)
        {
            parameters = new List<string>();
            parameters.Add(p);
        }

        public string name { get { return "effectskip"; } set { } }

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