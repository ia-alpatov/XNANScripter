using System.Collections.Generic;
using System.Threading.Tasks;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class WavestopFunction : Function
    {
        public string name { get { return "wavestop"; } set { } }

        public List<string> parameters { get; set; }

        public WavestopFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            Sound.soundmanager.Stop(2, 0);

            parameters = null;
        }
    }
}