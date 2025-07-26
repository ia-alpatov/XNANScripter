using System.Collections.Generic;
using System.Threading.Tasks;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class SaveoffFunction : Function
    {
        public string name { get { return "saveoff"; } set { } }

        public List<string> parameters { get; set; }

        public SaveoffFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            SaveSystem.IsSaveEnabled = false;

            parameters = null;
        }
    }
}