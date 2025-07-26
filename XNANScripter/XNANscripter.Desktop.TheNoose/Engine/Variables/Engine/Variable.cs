using System.Collections.Generic;

namespace XNANScripter.Engine.Variables
{
    internal interface Variable
    {
        string name
        {
            get;
            set;
        }

        List<string> parameters { get; set; }

        string Get();

        bool Set(string _params);
    }
}