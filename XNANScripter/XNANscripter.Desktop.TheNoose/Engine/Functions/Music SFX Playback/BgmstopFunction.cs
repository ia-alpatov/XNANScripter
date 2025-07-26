using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class BgmstopFunction : Function
    {
        public string name { get { return "bgmstop"; } set { } }

        public List<string> parameters { get; set; }

        public BgmstopFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            Sound.soundmanager.Stop(5, 0);

            parameters = null;
        }
    }
}