using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XNANScripter.Engine.Functions
{
    internal class Mp3saveFunction : Function
    {
        public string name { get { return "mp3save"; } set { } }

        public List<string> parameters { get; set; }

        public Mp3saveFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            throw new NotImplementedException();

            parameters = null;
        }
    }
}