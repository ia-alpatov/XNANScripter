using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Sprites;

namespace XNANScripter.Engine.Functions
{
    internal class LspFunction : Function
    {
        public string name { get { return "lsp"; } set { } }

        public List<string> parameters { get; set; }

        public LspFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("%VAR");
            mask.Add("$VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("%VAR");
            mask.Add("3");
            mask.Add("2");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                return parameters[parameters.Count - 1];
            }
            else
            {
                throw new Exception("Неверные параметры функции.");
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            int key = int.Parse(ValuesParser.GetNumber(parameters[0]));

            Sprite sprite = await ValuesParser.GetSprite(parameters[1]);

            sprite.IsHidden = false;

            sprite.Rectangle = new Rectangle(int.Parse(ValuesParser.GetNumber(parameters[2])), int.Parse(ValuesParser.GetNumber(parameters[3])), sprite.texturesList[0].Width, sprite.texturesList[0].Height);

            //sprite.Rectangle = new Rectangle(0, 0, sprite.texturesList[0].Width, sprite.texturesList[0].Height);

            if (parameters.Count > 5)
            {
                throw new NotImplementedException();
            }

            Drawing.SpritesList.Add(key, sprite);

            parameters = null;
        }
    }
}