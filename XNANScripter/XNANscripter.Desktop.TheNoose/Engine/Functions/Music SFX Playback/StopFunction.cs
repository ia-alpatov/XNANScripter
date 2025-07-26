using System.Collections.Generic;
using System.Threading.Tasks;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class StopFunction : Function
    {
        public string name { get { return "stop"; } set { } }

        public List<string> parameters { get; set; }

        public StopFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            Sound.soundmanager.Stop(0, 0);

            parameters = null;
        }
    }
}