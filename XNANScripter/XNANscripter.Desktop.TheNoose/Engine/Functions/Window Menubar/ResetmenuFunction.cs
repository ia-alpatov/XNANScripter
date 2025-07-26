using System.Collections.Generic;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Window_Menubar;

namespace XNANScripter.Engine.Functions
{
    internal class ResetmenuFunction : Function
    {
        public string name { get { return "resetmenu"; } set { } }

        public List<string> parameters { get; set; }

        public ResetmenuFunction()
        {
        }

        public string Parse(string _parameters)
        {
            return _parameters;
        }

        public async System.Threading.Tasks.Task Run()
        {
            TopMenu.MenuList = new List<MenuItem>();

            parameters = null;
        }
    }
}