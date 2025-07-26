using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XNANScripter.Engine.Config;

namespace XNANScripter.Engine.Functions
{
    internal class MovieFunction : Function
    {
        public string name { get { return "movie"; } set { } }

        public List<string> parameters { get; set; }

        public MovieFunction()
        {
        }

        public string Parse(string _parameters)
        {
            List<string> mask = new List<string>();

            mask.Add("(stop)");
            mask.Add("1");

            parameters = ValuesParser.GetParams(_parameters, mask);

            if (parameters != null)
            {
                string result = parameters[1];
                parameters[1] = "0";
                return result;
            }
            else
            {
                mask.Clear();

                mask.Add("$VAR");
                mask.Add("(loop|click|async)");
                mask.Add("0");
                mask.Add("4");

                parameters = ValuesParser.GetParams(_parameters, mask);

                if (parameters != null)
                {
                    string result = parameters[parameters.Count - 1];
                    parameters[parameters.Count - 1] = "1";
                    return result;
                }
                else
                {
                    mask.Clear();

                    mask.Add("$VAR");
                    mask.Add("(pos)");
                    mask.Add("%VAR");
                    mask.Add("%VAR");
                    mask.Add("%VAR");
                    mask.Add("%VAR");
                    mask.Add("(loop|click|async)");
                    mask.Add("5");
                    mask.Add("4");

                    parameters = ValuesParser.GetParams(_parameters, mask);

                    if (parameters != null)
                    {
                        string result = parameters[parameters.Count - 1];
                        parameters[parameters.Count - 1] = "2";
                        return result;
                    }
                    else
                    {
                        throw new Exception("Неверные параметры функции.");
                    }
                }
            }
        }

        public async System.Threading.Tasks.Task Run()
        {
            ushort type = ushort.Parse(parameters[parameters.Count - 1]);

            switch (type)
            {
                case 0:
                    //Отстанавливаем видео
                    Video.StopPlaying = true;
                    break;

                case 1:
                    //Воспроизводим видео с параметрами или без
                    Video.PathToVideo = @"Content\" + ValuesParser.GetString(parameters[0]);
                    Video.SkipOnTouch = false;

                    Parsing.Wait = true;

                    //Устанавливаем положение
                    Video.VideoRect = new Microsoft.Xna.Framework.Rectangle(0, 0, 800, 480);

                    //Проверяем параметры
                    for (int i = 1; i < parameters.Count - 1; i++)
                    {
                        switch (parameters[i])
                        {
                            case "loop":
                                Video.IsLooped = true;
                                break;

                            case "click":
                                Video.SkipOnTouch = true;
                                break;

                            case "async":
                                Video.SkipOnTouch = false;
                                Parsing.Wait = false;
                                break;
                        }
                    }

                    Drawing.IsVideoPlaying = true;
                    break;

                case 2:
                    //Воспроизводим видео с параметрами или без в нужной части экрана
                    Video.PathToVideo = @"Content\" + ValuesParser.GetString(parameters[0]);
                    Video.SkipOnTouch = false;

                    Parsing.Wait = true;

                    //Устанавливаем место, где будет проигрываться видео
                    Video.VideoRect = new Microsoft.Xna.Framework.Rectangle(int.Parse(ValuesParser.GetNumber(parameters[2])), int.Parse(ValuesParser.GetNumber(parameters[3])), int.Parse(ValuesParser.GetNumber(parameters[4])), int.Parse(ValuesParser.GetNumber(parameters[5])));

                    //Проверяем параметры
                    for (int i = 6; i < parameters.Count - 1; i++)
                    {
                        switch (parameters[i])
                        {
                            case "loop":
                                Video.IsLooped = true;
                                break;

                            case "click":
                                Video.SkipOnTouch = true;
                                break;

                            case "async":
                                Video.SkipOnTouch = false;
                                Parsing.Wait = false;
                                break;
                        }
                    }

                    Drawing.IsVideoPlaying = true;
                    break;
            }

            parameters = null;
        }
    }
}