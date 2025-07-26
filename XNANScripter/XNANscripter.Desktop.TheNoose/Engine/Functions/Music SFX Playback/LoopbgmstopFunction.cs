using System.Collections.Generic;
using System.Threading.Tasks;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class LoopbgmstopFunction : Function
    {
        public string name { get { return "loopbgmstop"; } set { } }

        public List<string> parameters { get; set; }

        public LoopbgmstopFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            Sound.soundmanager.Stop(6, 0);

            parameters = null;
        }
    }
}