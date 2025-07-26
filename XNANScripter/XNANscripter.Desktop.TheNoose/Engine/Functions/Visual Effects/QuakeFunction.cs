using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class QuakeFunction : Function
    {
        public string name { get { return "quake"; } set { } }

        public List<string> parameters { get; set; }

        public QuakeFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[2];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            //throw new NotImplementedException();

            Drawing.QuakeEffectType = QuakeEffect.QuakeXY;
            Drawing.QuakeEffectTime = 0;
            Drawing.QuakeEffectMaxTime = int.Parse(ValuesParser.GetNumber(parameters[1]));
            Drawing.QuakeEffectPixelAmpl = int.Parse(ValuesParser.GetNumber(parameters[0]));

            Parsing.Wait = true;
           

            parameters = null;
        }
    }
}