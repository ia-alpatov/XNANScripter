using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Effects;

namespace XNANScripter.Engine.Functions
{
    internal class WindoweffectFunction : Function
    {
        public string name { get { return "windoweffect"; } set { } }

        public List<string> parameters { get; set; }

        public WindoweffectFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("$VAR");
            mask.Add("1");
            mask.Add("2");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                string value = parameters[parameters.Count - 1];
                parameters[parameters.Count - 1] = "2";
                return value;
            }
            else
            {
                mask.Clear();

                mask.Add("%VAR");
                mask.Add("%VAR");
                mask.Add("0");
                mask.Add("2");

                parameters = ValuesParser.GetParams(_parameters, mask);

                if (parameters != null)
                {
                    string value = parameters[parameters.Count - 1];
                    parameters[parameters.Count - 1] = "1";
                    return value;
                }
                else
                {
                    throw new Exception("Неверные параметры функции.");
                }
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            if (parameters[parameters.Count - 1] == "1")
            {
                //Это эффект с возможно заданной длительностью
                UserEffect effect = new UserEffect();
                effect.BuiltInNumber = ushort.Parse(ValuesParser.GetNumber(parameters[0]));

                if (parameters.Count > 2)
                {
                    effect.duration = ushort.Parse(ValuesParser.GetNumber(parameters[1]));
                }

                TextWindow.WindowEffect = effect;
            }
            else
            {
                //Это эффект с заданной длительностью и маской
                UserEffect effect = new UserEffect();
                effect.BuiltInNumber = ushort.Parse(ValuesParser.GetNumber(parameters[0]));
                effect.duration = ushort.Parse(ValuesParser.GetNumber(parameters[1]));

                if (parameters.Count > 3)
                {
                    effect.mask = ValuesParser.GetString(parameters[2]);
                }

                TextWindow.WindowEffect = effect;
            }

            parameters = null;
        }
    }
}