using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class VoicevolFunction : Function
    {
        public string name { get { return "voicevol"; } set { } }

        public List<string> parameters { get; set; }

        public VoicevolFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
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
            Sound.soundmanager.SetVoiceVolume(int.Parse(ValuesParser.GetNumber(parameters[0])));

            parameters = null;
        }
    }
}