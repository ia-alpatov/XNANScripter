﻿using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class Mp3loopFunction : Function
    {
        public string name { get { return "mp3loop"; } set { } }

        public List<string> parameters { get; set; }

        public Mp3loopFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("$VAR");
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
            Sound.soundmanager.Play(3, 0, ValuesParser.GetString(parameters[0]), true);

            parameters = null;
        }
    }
}