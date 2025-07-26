using XNANScripter.Engine.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class SystemcallFunction : Function
    {
        public string name { get { return "systemcall"; } set { } }

        public List<string> parameters { get; set; }

        public SystemcallFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("NAME");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[1];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            switch(parameters[0].ToLower())
            {
                case "skip":
                    // skip - same as "skip to next choice"
                    break;
                case "reset":
                    //reset - reset program
                    break;
                case "resetdlg":
                    //resetdlg - same as reset?
                    break;
                case "save":
                    //save - go to save menu
                    break;
                case "load":
                    //load - go to load menu
                    Parsing.Wait = true;
                    SaveSystem.Slots = await Save.SaveController.GetSlots(Core.GameId);
                    Core.GameStatus = GameStateType.Load;
                    break;
                case "lookback":
                    //lookback - go to log mode
                    break;
                case "windowerase":
                    //windowerase - remove text window
                    break;
                case "automode":
                    //automode - automatically advance text at each clickwait (delay time set by automode_time)
                    break;
                case "rmenu":
                    //rmenu - opens the right-click menu
                    break;
            }
        }
    }
}
