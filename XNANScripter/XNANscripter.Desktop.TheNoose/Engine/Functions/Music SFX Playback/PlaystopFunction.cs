using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class PlaystopFunction : Function
    {
        public string name { get { return "playstop"; } set { } }

        public List<string> parameters { get; set; }

        public PlaystopFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            Sound.soundmanager.Stop(1, 0);

            parameters = null;
        }
    }
}