using System;
using System.Collections.Generic;

namespace XNANScripter.Engine.Functions
{
    internal class Chkcdfile_exFunction : Function
    {
        public string name { get { return "chkcdfile_ex"; } set { } }

        public List<string> parameters { get; set; }

        public Chkcdfile_exFunction()
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