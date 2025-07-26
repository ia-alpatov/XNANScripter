using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class MpegplayFunction : Function
    {
        public string name { get { return "mpegplay"; } set { } }

        public List<string> parameters { get; set; }

        public MpegplayFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("$VAR");
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
            Video.PathToVideo = @"Content\" + ValuesParser.GetString(parameters[0]);
            Video.SkipOnTouch = (ValuesParser.GetNumber(parameters[1]) == "1");

            Drawing.IsVideoPlaying = true;
            Parsing.Wait = true;

            //Устанавливаем положение
            Video.VideoRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 800, 480);

            parameters = null;
        }
    }
}