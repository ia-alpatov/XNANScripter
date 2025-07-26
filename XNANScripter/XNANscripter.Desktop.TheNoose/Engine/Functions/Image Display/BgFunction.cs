using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class BgFunction : Function
    {
        public string name { get { return "bg"; } set { } }

        public List<string> parameters { get; set; }

        public BgFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("$VAR|COLOR|black|white");
            mask.Add("%VAR");
            mask.Add("0");
            mask.Add("4");

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
           
            //Создаём текстуру

            //Задаём размер текстуры
            int width, height;

            width = Drawing.BackgroundRectangle.Width;
            height = Drawing.BackgroundRectangle.Height;

            Texture2D tempTex;
            Color[] texData;

            switch (parameters[0][0])
            {
                case 'b':
                    tempTex = new Texture2D(Drawing.GraphicsDevice, width, height);
                    texData = new Color[width * height];

                    //Заполняем текстуру цветом
                    for (int i = 0; i < texData.Length; i++)
                    {
                        texData[i] = Color.Black;
                    }

                    tempTex.SetData<Color>(texData);

                    await SetBackground(tempTex, width, height);
                    break;

                case 'w':
                    tempTex = new Texture2D(Drawing.GraphicsDevice, width, height);
                    texData = new Color[width * height];

                    //Заполняем текстуру цветом
                    for (int i = 0; i < texData.Length; i++)
                    {
                        texData[i] = Color.White;
                    }

                    tempTex.SetData<Color>(texData);

                    await SetBackground(tempTex, width, height);
                    break;

                case '#':
                    tempTex = new Texture2D(Drawing.GraphicsDevice, width, height);
                    texData = new Color[width * height];

                    //Получаем цвет из строки
                    Color color = ValuesParser.GetColor(parameters[0]);

                    //Заполняем текстуру цветом
                    for (int i = 0; i < texData.Length; i++)
                    {
                        texData[i] = color;
                    }

                    tempTex.SetData<Color>(texData);
                    await SetBackground(tempTex, width, height);
                    break;

                default:
                    using (System.IO.Stream stream = TitleContainer.OpenStream(@"Content\" + ValuesParser.GetString(parameters[0])))
                    {
                        await SetBackground(Texture2D.FromStream(Drawing.GraphicsDevice, stream), width, height);
                    }
                    
                    break;
            }

          
            parameters = null;
        }

        private async Task SetBackground(Texture2D tempTex,int width, int height)
        {
           
                
            
            if (tempTex.Width != width || tempTex.Height != height)
            {
                if (tempTex.Width < width || tempTex.Height < height)
                {
                    double widthPercent = ((height - tempTex.Height) / (tempTex.Height / 100.0)) / 100.0 + 1;

                    Drawing.backgroundScale = (float)widthPercent;


                }
            }

            //Применяем эффект

            switch (parameters.Count)
            {
                case 3:
                    if (Drawing.UserEffectsList.ContainsKey(ushort.Parse(parameters[1])))
                    {
                        await Task.Delay(Drawing.UserEffectsList[ushort.Parse(parameters[1])].duration);
                    }
                    break;

                case 4:
                    break;
            }

            lock (Drawing.backgroundLock)
            {
                //            Drawing.BackgroundEffect = Drawing.UserEffectsList[ushort.Parse(parameters[1])];

                //          Drawing.CurrentEffectTime = 0;
                if (Drawing.background != null)
            {
                Drawing.background.Dispose();
                Drawing.background = null;

                // Config.System.Content.Unload();
                
            }
            Drawing.background = tempTex;
            //Parsing.Wait = true;
            
            }
        }
    }
}