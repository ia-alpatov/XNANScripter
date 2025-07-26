using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Threading.Tasks;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Functions;
using XNANScripter.Engine.Procedures;
using Microsoft.Xna.Framework.Input.Touch;
using XNANScripter.Engine.Variables;
using Microsoft.Xna.Framework.Input;
using The_noose.Engine.TextDisplaying;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace XNANScripter.Engine
{
    internal class GameManager
    {

        private SpriteFont testFont;

        private double time = 0;

        private System.Text.RegularExpressions.Match matcher;

        private RenderTarget2D MainRenderTarget;

        internal async Task LoadContent(Microsoft.Xna.Framework.Content.ContentManager contentManager)
        {
            testFont = contentManager.Load<SpriteFont>("TestFont");

            Texture2D tempTex = new Texture2D(Drawing.GraphicsDevice, 1, 1);
            tempTex.SetData<Color>(new Color[] { Color.Black });

            TextWindow.TextBackground = tempTex;

            /*
             1. Instantaneous display. No runtime variable needed.
2. Left-sided shutter
3. Right-sided shutter
4. Upwards shutter
5. Downwards shutter
6. Left-moving curtain
7. Right-moving curtain
8. Upwards curtain
9. Downwards curtain
10. Pixelwise crossfade
11. Left-moving scroll
12. Right-moving scroll
13. Upwards scroll
14. Downwards scroll
15. Fade via mask pattern. You must provide a filename pointing to a mask image (of either 256 colors or full color). Within a masked image, the white areas fade slowly, and the black areas fade quickly.
16. Mosaic out. After this effect is called, the state of the screen will be uncertain (like with Effect 0), so please call a display command, like print, immediately afterwards.
17. Mosaic in
18. Crossfade via mask. This works similarly to Effect 15, except it is far more processor intensive, so use it with care.


             Drawing.BuiltInEffects.Add(2, contentManager.Load<Effect>("Effects/2"));
            Drawing.BuiltInEffects.Add(3, contentManager.Load<Effect>("Effects/3"));
            Drawing.BuiltInEffects.Add(4, contentManager.Load<Effect>("Effects/4"));
            Drawing.BuiltInEffects.Add(5, contentManager.Load<Effect>("Effects/5"));
            Drawing.BuiltInEffects.Add(6, contentManager.Load<Effect>("Effects/6"));
            Drawing.BuiltInEffects.Add(7, contentManager.Load<Effect>("Effects/7"));
            Drawing.BuiltInEffects.Add(8, contentManager.Load<Effect>("Effects/8"));
            Drawing.BuiltInEffects.Add(9, contentManager.Load<Effect>("Effects/9"));
            Drawing.BuiltInEffects.Add(10, contentManager.Load<Effect>("Effects/10"));
            Drawing.BuiltInEffects.Add(11, contentManager.Load<Effect>("Effects/11"));
            Drawing.BuiltInEffects.Add(12, contentManager.Load<Effect>("Effects/12"));
            Drawing.BuiltInEffects.Add(13, contentManager.Load<Effect>("Effects/13"));
            Drawing.BuiltInEffects.Add(14, contentManager.Load<Effect>("Effects/14"));
            Drawing.BuiltInEffects.Add(15, contentManager.Load<Effect>("Effects/15"));
            Drawing.BuiltInEffects.Add(16, contentManager.Load<Effect>("Effects/16"));
            Drawing.BuiltInEffects.Add(17, contentManager.Load<Effect>("Effects/17"));
            Drawing.BuiltInEffects.Add(18, contentManager.Load<Effect>("Effects/18"));


             */

            MainRenderTarget = new RenderTarget2D(
                Config.Drawing.GraphicsDevice,
                Config.Drawing.GraphicsDevice.PresentationParameters.BackBufferWidth,
                Config.Drawing.GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                Config.Drawing.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);


            Task.Run(async () => { await ParseConfig(); });
        }

        private async Task ParseConfig()
        {


            //cache images
            //await LoadFolderFiles(await StorageFolder.GetFolderFromPathAsync(Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "\\" + Config.System.Content.RootDirectory));

            ushort count = 0;

            //Загружаем файлы со скриптами Можно ли искать файлы с конкретным расширением?

            #region Script Loader

            bool HasFiles = true;

            while (HasFiles)
            {
                //Загружаем текст из файлов, пока не возникнет ошибка открытия (переделать)
                try
                {
                    using (System.IO.Stream stream =
                           TitleContainer.OpenStream(@"Content\" + String.Format("{0:00}", count) + ".utf"))
                    {
                        using (System.IO.StreamReader sreader = new System.IO.StreamReader(stream))
                        {
                            while (!sreader.EndOfStream)
                            {
                                Parsing.ScriptText.Add(sreader.ReadLine());
                            }

                            Parsing.ScriptText.TrimExcess();

                            count++;
                        }
                    }
                }
                catch
                {
                    HasFiles = false;
                }
            }

            #endregion Script Loader

            //Проверяем загрузились ли файлы



            //Парсим настройки от *define до game

            #region Config Parser

            bool HasSmth = true;
            count = 0;
            bool IsConfigBlockStarted = false;

            //Первые две строчки могут быть ;mode ;gameid

            while (HasSmth)
            {
                //Если эта строчка не комментарий и она не пуста то проверяем её.
                if (!String.IsNullOrEmpty(Parsing.ScriptText[count]) && Parsing.ScriptText[count][0] != ';')
                {
                    //Если начался блок игры(функций) то прекращаем идти по файлу
                    if (Parsing.ScriptText[count] == "game")
                    {
                        break;
                    }

                    //Проверяем не начался ли блок настройки
                    if (!IsConfigBlockStarted)
                    {
                        //Ищем блок настройки и только потом ищем функции
                        if (Parsing.ScriptText[count] == "*define")
                        {
                            IsConfigBlockStarted = true;
                        }
                    }
                    else
                    {
                        //Получаем название функции
                        const string regExpr = @"^[\s]*[A-Za-z0-9_]+";
                        matcher = System.Text.RegularExpressions.Regex.Match(Parsing.ScriptText[count], regExpr);

                        string funtionName = null;
                        string param;

                        if (matcher.Success)
                        {
                            //Записываем имя функции и её параметры
                            funtionName = matcher.Value.Trim();
                            param = Parsing.ScriptText[count].Remove(0, matcher.Length);

                            //Проверяем существование такой функции
                            //Сверх странное поведение поиск по списку выдаёт ссылку а не копию
                            // UserVariables.NumVarList["k"].
                            Function resultFunction = Core.FunctionsList.SingleOrDefault(fn => fn.name == funtionName);
                            if (resultFunction != null)
                            {
                                //Если такая существует то передаём ей параметры
                                resultFunction.Parse(param);

                                //Если проверка параметров на валидность прошла успешно, то запускаем функцию
                                await resultFunction.Run();

                                resultFunction = null;
                            }
                            else
                            {
                                //Если нет, то ?????
                                //throw new Exception("Нет такой функции");
                            }
                        }
                        else
                        {
                            //Получена неверная строка
                        }
                    }
                }

                //Если мы дошли до конца файла то прекращаем парсить и !!!наверно завершаем работу программы
                if (Parsing.ScriptText.Count == count - 1)
                {
                    break;
                }

                count++;
            }

            //Записываем строчку на которой остановились

            #endregion Config Parser

            //Парсим процедуры от game до *start и потом от end до конца файла

            #region Procedures Parser

            //Лишиние регулярки = Proc.count - 1

            #region Loop version

            bool HasProcedures = true;

            int LinesCount = Parsing.ScriptText.Count - 1;

            while (HasProcedures)
            {
                count++;

                if (Parsing.ScriptText[count].Length != 0)
                {
                    //Регуляркой получаем название (можно будет заменить простым циклом)
                    matcher = System.Text.RegularExpressions.Regex.Match(Parsing.ScriptText[count],
                        @"^[\s]*\*[A-Za-z0-9_]+");

                    if (matcher.Success)
                    {
                        ushort start = count;
                        string value = matcher.Value.Trim();

                        while (true)
                        {
                            count++;

                            if (count == LinesCount)
                            {
                                HasProcedures = false;
                                break;
                            }

                            if (Parsing.ScriptText[count].Length != 0)
                            {
                                matcher = System.Text.RegularExpressions.Regex.Match(Parsing.ScriptText[count],
                                    @"^[\s]*\*[A-Za-z0-9_]+");

                                if (matcher.Success)
                                {
                                    count--;
                                    break;
                                }
                            }
                        }

                        UserDefine.ProceduresList.Add(new Procedure(value, start, count));
                    }
                }
            }

            #endregion Loop version

            //Или может использовать рекурсию?   Без оптимизации 0x8007015

            #region Recursive version

            //    FindProcedure(j);
            /*
        private Procedure FindProcedure(int j)
        {
            if (j == Parsing.ScriptText.Count - 2)
            {
                return new Procedure("",j,0);
            }

            // Так как всё и даже старт является процедурой, то проверяем не началоли ли это процедуры
            if (Parsing.ScriptText[j].Length != 0 && Parsing.ScriptText[j][0] != ';')
            {
                //Регуляркой получаем название (можно будет заменить простым циклом)
                const string regExpr = @"^[\s]*\*[A-Za-z0-9_]+";
                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(Parsing.ScriptText[j], regExpr);

                if (m.Success)
                {
                    string name = m.Value.Remove(0, 1);
                    m = null;

                    Procedure proc = FindProcedure(j + 1);

                    int start = j;

                    int end = proc.startPoint - 1;

                    proc = new Procedure(name, start, end);

                    UserDefine.ProceduresList.Add(proc);

                    return proc;
                }
                else
                {
                    m = null;

                    return FindProcedure(j + 1);
                }
            }
            else
            {
                return FindProcedure(j + 1);
            }
        }
         *
        */

            #endregion Recursive version

            #endregion Procedures Parser

            //Записываем строчку с которой начнём выполение скрипта
            Parsing.CurrentLine = UserDefine.ProceduresList.First(var => var.name == "*start").startPoint;

            Core.GameStatus = GameStateType.Game;


            Task.Run(async () => { GameParser(); });
        }

        /*
        private async Task LoadFolderFiles(StorageFolder folder)
        {
            var files = await folder.GetFilesAsync();
            foreach (StorageFile file in files)
            {
                try
                {
                    Config.System.Content.Load<Texture2D>(file.Path.Replace(Windows.ApplicationModel.Package.Current.InstalledLocation.Path + "\\" + Config.System.Content.RootDirectory + "\\", string.Empty));
                }
                catch(Exception)
                {

                }
            }

            var folders = await folder.GetFoldersAsync();
            foreach (var f in folders)
            {
                await LoadFolderFiles(f);
            }
        }
*/
        private const string regExpr = @"^[\s]*[A-Za-z0-9_]+";

        internal void GameParser()
        {
            while (true)
            {
                if (Parsing.Wait)
                {
                    Task.WaitAll(Task.Delay(100));
                    if (Delay.Sleeping && !Delay.StartOnTouch)
                    {
                        if (Delay.ElapsedTime < Delay.TimeToSleep)
                        {
                            Delay.ElapsedTime += 100;
                        }
                        else
                        {
                            Delay.Sleeping = false;
                            Parsing.Wait = false;
                        }
                    }

                    continue;
                }

                Parsing.CurrentLine++;

                //Получаем название функции

                matcher = System.Text.RegularExpressions.Regex.Match(Parsing.ScriptText[Parsing.CurrentLine], regExpr);

                if (matcher.Success)
                {
                    RunFunction(matcher, Parsing.ScriptText[Parsing.CurrentLine]);
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(Parsing.ScriptText[Parsing.CurrentLine]))
                    {
                        if (Parsing.ScriptText[Parsing.CurrentLine].Trim().StartsWith("*"))
                        {


                        }

                        else
                        {
                            matcher = System.Text.RegularExpressions.Regex.Match(
                                Parsing.ScriptText[Parsing.CurrentLine].Trim(), "^#[A-Za-z0-9]{6,6}$");
                            if (matcher.Success)
                            {
                                TextWindow.TextColor = ValuesParser.GetColor(matcher.Value);
                            }
                            else
                            {
                                if (ValuesParser.GetText(Parsing.ScriptText[Parsing.CurrentLine]))
                                {
                                    Parsing.Wait = true;
                                }
                                else
                                {
                                    //Получена неверная строка
                                    throw new Exception("Неверная строка");
                                }
                            }

                        }
                    }
                }
            }
        }

        private void RunFunction(System.Text.RegularExpressions.Match matcher, string line)
        {
            //Записываем имя функции и её параметры
            string funtionName = matcher.Value.Trim();
            string param = line.Remove(0, matcher.Length);

            //Проверяем существование такой функции
            //Поиск по списку выдаёт ссылку а не копию
            // UserVariables.NumVarList["k"].
            Function resultFunction = Core.FunctionsList.SingleOrDefault(fn => fn.name == funtionName);
            if (resultFunction != null)
            {
                //Если такая существует то передаём ей параметры
                var res = resultFunction.Parse(param);

                //Если проверка параметров на валидность прошла успешно, то запускаем функцию
                Task.WaitAll(resultFunction.Run());

                if (funtionName.ToLower() == "if" && res != null)
                {
                    RunFunction(System.Text.RegularExpressions.Regex.Match(res, regExpr), res);
                }

                resultFunction = null;
            }
            else
            {
                Function notSupportedFunction =
                    Core.NotSupportedFunctionsList.SingleOrDefault(fn => fn.name == funtionName);
                if (notSupportedFunction != null)
                {
                    //??? 
                }
                else
                {
                    throw new Exception("Нет такой функции");
                }
            }
        }

        internal void Update(TimeSpan elapsedTime, TimeSpan totalTime)
        {

            Sound.soundmanager.Update(totalTime - elapsedTime);
            switch (Core.GameStatus)
            {
                case GameStateType.Loading:
                    break;

                case GameStateType.Game:
                    time = totalTime.TotalMilliseconds;

                    if (UserButtons.WaitForPress)
                    {
                        var touchCol = TouchPanel.GetState();


                        var ms = Mouse.GetState();
                        if (ms != null)
                        {
                            foreach (var btn in UserButtons.ButtonsList)
                            {
                                if (ms.LeftButton == ButtonState.Pressed)
                                {
                                    var rect = btn.Value.Rectangle;
                                    rect = new Rectangle((int)(Math.Round(rect.X * Drawing.backgroundScale)),
                                        (int)(Math.Round(rect.Y * Drawing.backgroundScale)),
                                        (int)(Math.Round(rect.Width * Drawing.backgroundScale)),
                                        (int)(Math.Round(rect.Height * Drawing.backgroundScale)));
                                    if (rect.Contains(new Vector2(ms.X, ms.Y)))
                                    {
                                        UserButtons.ButtonsList[btn.Key].Draw = true;
                                    }
                                    else
                                    {
                                        UserButtons.ButtonsList[btn.Key].Draw = false;
                                    }

                                }

                                if (ms.LeftButton == ButtonState.Released)
                                {
                                    var rect = btn.Value.Rectangle;
                                    rect = new Rectangle((int)(Math.Round(rect.X * Drawing.backgroundScale)),
                                        (int)(Math.Round(rect.Y * Drawing.backgroundScale)),
                                        (int)(Math.Round(rect.Width * Drawing.backgroundScale)),
                                        (int)(Math.Round(rect.Height * Drawing.backgroundScale)));

                                    if (btn.Value.Draw && rect.Contains(new Vector2(ms.X, ms.Y)))
                                    {
                                        ValuesParser.SetNumber(UserButtons.WaitVariable, btn.Key.ToString());
                                        UserButtons.WaitForPress = false;
                                        Parsing.Wait = false;
                                        break;
                                    }
                                }
                            }
                        }

                        foreach (var touch in touchCol)
                        {
                            foreach (var btn in UserButtons.ButtonsList)
                            {
                                var rect = btn.Value.Rectangle;
                                rect = new Rectangle((int)(Math.Round(rect.X * Drawing.backgroundScale)),
                                    (int)(Math.Round(rect.Y * Drawing.backgroundScale)),
                                    (int)(Math.Round(rect.Width * Drawing.backgroundScale)),
                                    (int)(Math.Round(rect.Height * Drawing.backgroundScale)));

                                if (touch.State == TouchLocationState.Pressed)
                                {
                                    if (rect.Contains(touch.Position))
                                    {
                                        UserButtons.ButtonsList[btn.Key].Draw = true;
                                    }
                                }

                                if (touch.State == TouchLocationState.Released)
                                {
                                    if (btn.Value.Draw && rect.Contains(touch.Position))
                                    {
                                        ValuesParser.SetNumber(UserButtons.WaitVariable, btn.Key.ToString());
                                        UserButtons.WaitForPress = false;
                                        Parsing.Wait = false;
                                        break;
                                    }
                                }

                                if (touch.State == TouchLocationState.Moved)
                                {
                                    if (rect.Contains(touch.Position))
                                    {
                                        UserButtons.ButtonsList[btn.Key].Draw = true;
                                    }
                                    else
                                    {
                                        UserButtons.ButtonsList[btn.Key].Draw = false;
                                    }
                                }
                            }
                        }

                        if (!UserButtons.WaitForPress)
                        {
                            UserButtons.ButtonsList.Clear();
                        }
                    }

                    if (TextWindow.TextParts.Count > 0 && Parsing.Wait)
                    {
                        if (!TextWindow.WaitForClick)
                        {
                            if (TextWindow.TextParts.Count > (int)TextWindow.TextCurrentPosition.Y)
                            {
                                var current = TextWindow.TextParts[(int)TextWindow.TextCurrentPosition.Y];
                                var plusX = int.Parse(Core.EngineVarList.First(var => var.name == "textspeed").Get()) *
                                            0.05f * (float)elapsedTime.TotalMilliseconds;
                                if (current.Text.Length < plusX + TextWindow.TextCurrentPosition.X)
                                {
                                    if (!current.IsEndingWithNewLine && !current.IsEndingWithWait)
                                    {
                                        TextWindow.TextCurrentPosition.Y++;
                                        TextWindow.TextCurrentPosition.X = 0;
                                    }
                                    else
                                    {
                                        TextWindow.TextCurrentPosition.X = current.Text.Length;
                                        TextWindow.WaitForClick = true;
                                    }
                                }
                                else
                                {
                                    TextWindow.TextCurrentPosition.X += plusX;
                                }
                            }
                            else
                            {
                                TextWindow.TextCurrentPosition.Y--;
                                TextWindow.WaitForClick = true;
                            }
                        }
                        else
                        {
                            var ms = Mouse.GetState();
                            if (ms != null)
                            {

                                if (ms.LeftButton == ButtonState.Released && TextWindow.WaitForClickPressed)
                                {
                                    if (TextWindow.TextParts.Count <= (int)TextWindow.TextCurrentPosition.Y + 1)
                                    {
                                        var current = TextWindow.TextParts[(int)TextWindow.TextCurrentPosition.Y];

                                        if (current.IsEndingWithNewLine)
                                        {
                                            TextWindow.TextParts.Clear();
                                            TextWindow.TextPointer = Vector2.Zero;
                                            TextWindow.TextCurrentPosition.Y = 0;
                                        }
                                        else if (current.IsEndingWithWait)
                                        {
                                            TextWindow.TextCurrentPosition.Y++;
                                        }

                                        Parsing.Wait = false;
                                    }
                                    else
                                    {
                                        TextWindow.TextCurrentPosition.Y++;
                                        TextWindow.TextCurrentPosition.X = 0;
                                    }

                                    TextWindow.WaitForClick = false;
                                    TextWindow.WaitForClickPressed = false;
                                    break;
                                }
                                else if (ms.LeftButton == ButtonState.Pressed)
                                {
                                    TextWindow.WaitForClickPressed = true;
                                }
                            }

                            var touchCol = TouchPanel.GetState();




                            foreach (var touch in touchCol)
                            {
                                if (touch.State == TouchLocationState.Released)
                                {
                                    if (TextWindow.TextParts.Count <= (int)TextWindow.TextCurrentPosition.Y + 1)
                                    {
                                        var current = TextWindow.TextParts[(int)TextWindow.TextCurrentPosition.Y];

                                        if (current.IsEndingWithNewLine)
                                        {
                                            TextWindow.TextParts.Clear();
                                            TextWindow.TextPointer = Vector2.Zero;
                                            TextWindow.TextCurrentPosition.Y = 0;
                                        }
                                        else if (current.IsEndingWithWait)
                                        {
                                            TextWindow.TextCurrentPosition.Y++;
                                        }

                                        Parsing.Wait = false;
                                    }
                                    else
                                    {
                                        TextWindow.TextCurrentPosition.Y++;
                                        TextWindow.TextCurrentPosition.X = 0;
                                    }

                                    TextWindow.WaitForClick = false;
                                    break;
                                }
                            }


                          
                        }
                    }

                    break;

                case GameStateType.Load:
                    //Load menu state

                    break;
            }
        }

        internal void Draw(TimeSpan elapsedTime, TimeSpan totalTime,
            Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Config.Drawing.GraphicsDevice.SetRenderTarget(MainRenderTarget);
            Config.Drawing.GraphicsDevice.Clear(Color.Black);

            switch (Core.GameStatus)
            {
                case GameStateType.Loading:
                    spriteBatch.Begin();
                    spriteBatch.DrawString(testFont, "Loading",
                        new Vector2(
                            spriteBatch.GraphicsDevice.Viewport.Width / 2 - testFont.MeasureString("Loading").X / 2,
                            spriteBatch.GraphicsDevice.Viewport.Height / 2 - testFont.MeasureString("Loading").Y / 2),
                        Color.White);
                    spriteBatch.End();
                    break;

                case GameStateType.Game:
                    lock (Drawing.backgroundLock)
                    {


                        if (Drawing.NextBackground != null)
                        {
                            if (Drawing.background != null)
                            {
                                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                                {
                                    //   Drawing.BuiltInEffects[Drawing.BackgroundEffect.]

                                    spriteBatch.Draw(Drawing.background,
                                        new Rectangle(0, 0,
                                            (int)Math.Round(Drawing.background.Width * Drawing.backgroundScale),
                                            (int)Math.Round(Drawing.background.Height * Drawing.backgroundScale)),
                                        Drawing.TintColor);
                                    spriteBatch.Draw(Drawing.NextBackground,
                                        new Rectangle(0, 0,
                                            (int)Math.Round(Drawing.background.Width * Drawing.backgroundScale),
                                            (int)Math.Round(Drawing.background.Height * Drawing.backgroundScale)),
                                        Drawing.NextTintColor);
                                }
                                spriteBatch.End();
                            }
                            else
                            {
                                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                                {
                                    spriteBatch.Draw(Drawing.NextBackground,
                                        new Rectangle(0, 0,
                                            (int)Math.Round(Drawing.background.Width * Drawing.backgroundScale),
                                            (int)Math.Round(Drawing.background.Height * Drawing.backgroundScale)),
                                        Drawing.NextTintColor);
                                }
                                spriteBatch.End();
                            }
                        }
                        else if (Drawing.background != null)
                        {
                            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                            {
                                spriteBatch.Draw(Drawing.background,
                                    new Rectangle(0, 0,
                                        (int)Math.Round(Drawing.background.Width * Drawing.backgroundScale),
                                        (int)Math.Round(Drawing.background.Height * Drawing.backgroundScale)),
                                    Drawing.TintColor);
                            }
                            spriteBatch.End();
                        }
                    }

                    spriteBatch.Begin();
                    if (UserButtons.ButtonsList.Count > 0)
                    {
                        foreach (var btn in UserButtons.ButtonsList)
                        {
                            if (btn.Value.Draw)
                                spriteBatch.Draw(UserButtons.ButtonsBuffer,
                                    new Rectangle((int)(btn.Value.XShift * Drawing.backgroundScale),
                                        (int)(btn.Value.YShift * Drawing.backgroundScale),
                                        (int)(btn.Value.Rectangle.Width * Drawing.backgroundScale),
                                        (int)(btn.Value.Rectangle.Height * Drawing.backgroundScale)),
                                    btn.Value.Rectangle,
                                    Color.White);
                        }
                    }

                    if (TextWindow.IsShowing)
                    {
                        if (TextWindow.TextBackground != null)
                        {
                            spriteBatch.Draw(TextWindow.TextBackground, TextWindow.TextBackgroundRectangle,
                                Color.White * 0.2f);
                        }

                        if (TextWindow.TextParts.Count > 0)
                        {
                            Vector2 TextPosition = Vector2.Zero;
                            for (int i = 0; i < TextWindow.TextParts.Count; i++)
                            {
                                try
                                {
                                    if (i <= TextWindow.TextCurrentPosition.Y &&
                                        !String.IsNullOrWhiteSpace(TextWindow.TextParts[i].Text))
                                    {
                                        Vector2 size = Vector2.Zero;

                                        if (TextWindow.TextParts[i].Position != Vector2.Zero)
                                        {
                                            TextPosition = Vector2.Zero;
                                        }

                                        TextPart tempTp;
                                        if (i < TextWindow.TextCurrentPosition.Y ||
                                            (TextWindow.TextCurrentPosition.Y == i &&
                                             TextWindow.TextCurrentPosition.X >= TextWindow.TextParts[i].Text.Length))
                                        {
                                            tempTp = new TextPart()
                                            {
                                                IsColoredPart = TextWindow.TextParts[i].IsColoredPart,
                                                IsEndingWithNewLine = TextWindow.TextParts[i].IsEndingWithNewLine,
                                                IsEndingWithWait = TextWindow.TextParts[i].IsEndingWithWait,
                                                Position =
                                                    new Vector2(TextWindow.TextParts[i].Position.X,
                                                        TextWindow.TextParts[i].Position.Y),
                                                TextColor = TextWindow.TextParts[i].TextColor,
                                                Text = TextWindow.TextParts[i].Text
                                            };
                                        }
                                        else
                                        {
                                            tempTp = new TextPart()
                                            {
                                                IsColoredPart = TextWindow.TextParts[i].IsColoredPart,
                                                IsEndingWithNewLine = TextWindow.TextParts[i].IsEndingWithNewLine,
                                                IsEndingWithWait = TextWindow.TextParts[i].IsEndingWithWait,
                                                Position =
                                                    new Vector2(TextWindow.TextParts[i].Position.X,
                                                        TextWindow.TextParts[i].Position.Y),
                                                TextColor = TextWindow.TextParts[i].TextColor,
                                                Text =
                                                    TextWindow.TextParts[i].Text.Remove(
                                                        (int)TextWindow.TextCurrentPosition.X)
                                            };
                                        }

                                        List<TextPart> textParts = GetTextParts(tempTp, TextPosition);
                                        textParts.Reverse();
                                        for (int r = 0; r < textParts.Count; r++)
                                        {
                                            if (r == 0)
                                                spriteBatch.DrawString(testFont, textParts[r].Text,
                                                    new Vector2(textParts[r].Position.X * 10 + TextPosition.X,
                                                        textParts[r].Position.Y * 30 + TextPosition.Y),
                                                    textParts[r].TextColor);
                                            else
                                                spriteBatch.DrawString(testFont, textParts[r].Text,
                                                    new Vector2(textParts[r].Position.X * 10,
                                                        textParts[r].Position.Y * 30),
                                                    textParts[r].TextColor);
                                        }

                                        var LastText = textParts.Last();
                                        size = testFont.MeasureString(LastText.Text);

                                        if (LastText.IsEndingWithNewLine)
                                        {
                                            TextPosition.X = 0;
                                        }
                                        else
                                        {
                                            TextPosition.X += LastText.Position.X * 10 + size.X;
                                        }

                                        if (LastText.Position.Y != 0)
                                        {
                                            TextPosition.Y = LastText.Position.Y * 30;
                                        }

                                    }
                                    else if (String.IsNullOrWhiteSpace(TextWindow.TextParts[i].Text) &&
                                             TextWindow.TextParts[i].IsBr)
                                    {
                                        if (!TextWindow.TextParts[i].IsEndingWithNewLine)
                                        {
                                            TextPosition.X = 0;
                                            TextPosition.Y += 30;
                                        }
                                    }
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }

                    }

                    spriteBatch.End();
                    break;
                case GameStateType.Load:
                    //Load menu state
                    spriteBatch.Begin();
                    //Title of window
                    var titleSize = testFont.MeasureString(Config.SaveSystem.LoadMenuTitle);
                    spriteBatch.DrawString(testFont, Config.SaveSystem.LoadMenuTitle,
                        new Vector2(Drawing.BackgroundRectangle.Width / 2 - titleSize.X / 2, 10),
                        Color.White);
                    //Back button
                    var BackButtonSize = testFont.MeasureString(Config.SaveSystem.CloseButtonTitle);
                    spriteBatch.DrawString(testFont, Config.SaveSystem.CloseButtonTitle,
                        new Vector2(Drawing.BackgroundRectangle.Width - BackButtonSize.X - 10,
                            Drawing.BackgroundRectangle.Height - BackButtonSize.Y - 10),
                        Color.White);
                    //Slots grid
                    var max = DateTime.MaxValue.ToString();
                    var maxCellSize = testFont.MeasureString(max);
                    int padding = 10;
                    int cols = (int)((Drawing.BackgroundRectangle.Width - padding * 2) / maxCellSize.X);
                    cols -= 1;

                    int currentCol = 0;
                    int row = 0;
                    for (int i = 0; i < Config.SaveSystem.Slots.Count; i++)
                    {
                        string TextToWrite;
                        if (Config.SaveSystem.Slots[i].Value == DateTime.MinValue)
                        {
                            TextToWrite = SaveSystem.SlotTitle + (i + 1);
                        }
                        else
                        {
                            TextToWrite = Config.SaveSystem.Slots[i].Value.ToString();
                        }

                        var size = testFont.MeasureString(TextToWrite);

                        spriteBatch.DrawString(testFont, TextToWrite,
                            new Vector2(maxCellSize.X * currentCol + padding, size.Y * row + padding + titleSize.Y * 2),
                            Color.White);


                        if (currentCol > cols)
                        {
                            currentCol = 0;
                            row++;
                        }
                        else
                        {
                            currentCol++;
                        }
                    }

                    spriteBatch.End();
                    break;
                case GameStateType.Select:
                    break;
            }


            //For quake[xy], for background appear, for effects on textwindow, save prev state for diasapear 


            if (Drawing.QuakeEffectType != QuakeEffect.None)
            {
                Drawing.QuakeEffectTime += elapsedTime.Milliseconds;

                if (Drawing.QuakeEffectTime <= Drawing.QuakeEffectMaxTime)
                {
                    Vector2 pos = Drawing.Camera.Location;

                    switch (Drawing.QuakeEffectType)
                    {
                        case QuakeEffect.QuakeX:
                            pos.X = (float)(new Random().NextDouble() * Drawing.QuakeEffectPixelAmpl * 2 -
                                            Drawing.QuakeEffectPixelAmpl);
                            break;
                        case QuakeEffect.QuakeY:
                            pos.Y = (float)(new Random().NextDouble() * Drawing.QuakeEffectPixelAmpl * 2 -
                                            Drawing.QuakeEffectPixelAmpl);
                            break;
                        case QuakeEffect.QuakeXY:
                            pos.X = (float)(new Random().NextDouble() * Drawing.QuakeEffectPixelAmpl * 2 -
                                            Drawing.QuakeEffectPixelAmpl);
                            pos.Y = (float)(new Random().NextDouble() * Drawing.QuakeEffectPixelAmpl * 2 -
                                            Drawing.QuakeEffectPixelAmpl);
                            break;
                    }

                    Drawing.Camera.Location = pos;
                }
                else
                {
                    Drawing.Camera.Location = Vector2.Zero;
                    Drawing.QuakeEffectType = QuakeEffect.None;
                    Drawing.QuakeEffectTime = 0;
                    Parsing.Wait = false;
                }

            }

            Config.Drawing.GraphicsDevice.SetRenderTarget(null);
            Config.Drawing.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null,
                Drawing.Camera.TransformMatrix);
            {
                spriteBatch.Draw(MainRenderTarget, Drawing.BackgroundRectangle, Color.White);
            }
            spriteBatch.End();


        }

        private List<TextPart> GetTextParts(TextPart tp, Vector2 TextPosition)
        {
            if (testFont.MeasureString(tp.Text).X + tp.Position.X * 10 + TextPosition.X >
                (TextWindow.TextBackgroundRectangle.Width + TextWindow.TextBackgroundRectangle.X))
            {
                var tempText = string.Empty;
                foreach (char c in tp.Text)
                {
                    tempText += c;
                    if (testFont.MeasureString(tempText).X + tp.Position.X * 10 + TextPosition.X >
                        (TextWindow.TextBackgroundRectangle.Width + TextWindow.TextBackgroundRectangle.X))
                    {
                        tempText = tempText.Remove(tempText.Length - 1);
                        break;
                    }
                }

                TextPart nextTp = new TextPart()
                {
                    IsColoredPart = tp.IsColoredPart,
                    Position = new Vector2(0, tp.Position.Y + 1 + TextPosition.Y / 30),
                    Text = tp.Text.Remove(0, tempText.Length), TextColor = tp.TextColor
                };
                tp.Text = tempText;
                var result = GetTextParts(nextTp, Vector2.Zero);
                result.Add(tp);
                return result;
            }
            else
            {
                return new List<TextPart>() { tp };
            }
        }

        internal void Initialize()
        {
            //Инциализиреум конфиги и пр.

            //   UserDefine.ProceduresList.Add(new Procedure());

            for (int i = 0; i <= 999; i++)
            {
                UserVariables.NumVarList.Add(new Variables.Alias.NumVariable(String.Empty, i), 0);
            }

            Core.EngineVarList.Add(new TextspeedVariable("1"));
        }
    }
}