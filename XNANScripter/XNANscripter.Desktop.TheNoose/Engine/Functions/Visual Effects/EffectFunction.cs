using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Effects;

namespace XNANScripter.Engine.Functions
{
    internal class EffectFunction : Function
    {
        public string name { get { return "effect"; } set { } }

        public List<string> parameters { get; set; }

        public EffectFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("$VAR");
            mask.Add("2");
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
                mask.Add("%VAR");
                mask.Add("1");
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
                effect.BuiltInNumber = ushort.Parse(ValuesParser.GetNumber(parameters[1]));

                if (parameters.Count > 3)
                {
                    effect.duration = ushort.Parse(ValuesParser.GetNumber(parameters[2]));
                }

                Drawing.UserEffectsList.Add(ushort.Parse(ValuesParser.GetNumber(parameters[0])), effect);
            }
            else
            {
                //Это эффект с заданной длительностью и маской
                UserEffect effect = new UserEffect();
                effect.BuiltInNumber = ushort.Parse(ValuesParser.GetNumber(parameters[1]));
                effect.duration = ushort.Parse(ValuesParser.GetNumber(parameters[2]));

                if (parameters.Count > 4)
                {
                    effect.mask = ValuesParser.GetString(parameters[3]);
                }

                Drawing.UserEffectsList.Add(ushort.Parse(ValuesParser.GetNumber(parameters[0])), effect);
            }

            parameters = null;
        }
    }
}