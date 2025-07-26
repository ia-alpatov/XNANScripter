using System;
using System.Collections.Generic;

namespace XNANScripter.Engine.Functions
{
    internal class CdfadeoutFunction : Function
    {
        public string name { get { return "cdfadeout"; } set { } }

        public List<string> parameters { get; set; }

        public CdfadeoutFunction()
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