using System;
using System.Collections.Generic;

namespace XNANScripter.Engine.Functions
{
    internal class ChkcdfileFunction : Function
    {
        public string name { get { return "chkcdfile"; } set { } }

        public List<string> parameters { get; set; }

        public ChkcdfileFunction()
        {
        }

        public string Parse(string _parameters)
        {
            throw new Exception("Невозможно выполнить функцию");
        }

        public async System.Threading.Tasks.Task Run()
        {
        }
    }
}