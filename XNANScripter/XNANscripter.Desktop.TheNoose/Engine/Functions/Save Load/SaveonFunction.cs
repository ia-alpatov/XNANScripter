using System.Collections.Generic;
using System.Threading.Tasks;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class SaveonFunction : Function
    {
        public string name { get { return "saveon"; } set { } }

        public List<string> parameters { get; set; }

        public SaveonFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            SaveSystem.IsSaveEnabled = true;

            parameters = null;
        }
    }
}