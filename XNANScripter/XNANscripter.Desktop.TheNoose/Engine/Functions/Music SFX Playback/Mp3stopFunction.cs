using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class Mp3stopFunction : Function
    {
        public string name { get { return "mp3stop"; } set { } }

        public List<string> parameters { get; set; }

        public Mp3stopFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            Sound.soundmanager.Stop(3, 0);
            Sound.soundmanager.Stop(5, 0);

            parameters = null;
        }
    }
}