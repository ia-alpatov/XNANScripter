using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using XNANScripter.Engine.Buttons;
using XNANScripter.Engine.Effects;
using XNANScripter.Engine.Functions;
using XNANScripter.Engine.Procedures;
using XNANScripter.Engine.Sprites;
using XNANScripter.Engine.TextDisplaying;
using XNANScripter.Engine.Variables;
using XNANScripter.Engine.Variables.Alias;
using XNANScripter.Engine.Window_Menubar;
using Microsoft.Xna.Framework.Content;
using The_noose.Engine.TextDisplaying;
using The_noose.Engine.Utils.Camera;

namespace XNANScripter.Engine.Config
{
    internal struct Core
    {
        public static GameStateType GameStatus = GameStateType.Loading;

        public static List<Variable> EngineVarList = new List<Variable>();

        #region Functions List

        public static List<Function> FunctionsList = new List<Function>()
        {
            #region Text Window

            new WindoweffectFunction(),

            new BrFunction(),

            new SetwindowFunction(),

            #endregion Text Window

            #region Cursor

            new SetcursorFunction(),

            #endregion Cursor

            #region Image Display

            new BgaliaFunction(),

            new BgFunction(),

            new LspFunction(),

            new CspFunction(),

            #endregion Image Display

            #region Visual Effects

            new EffectFunction(),
            new EffectblankFunction(),
            new EffectcutFunction(),
            new EffectskipFunction(),
            //quke x y mono nego

            new QuakexFunction(),
            new QuakeyFunction(),
            new QuakeFunction(),

            new MonocroFunction(),

            #endregion Visual Effects

            #region Text Display

            new LocateFunction(),
            new TextspeedFunction(),

            #endregion Text Display

            #region Music/SFX Playback

            new CdfadeoutFunction(),
            new ChkcdfileFunction(),
            new Chkcdfile_exFunction(),
            new Mp3fadeoutFunction(),
            new PlayFunction(),
            new PlayonceFunction(),
            new PlaystopFunction(),
            new WaveFunction(),
            new WaveloopFunction(),
            new WavestopFunction(),
            new Mp3Function(),
            new Mp3loopFunction(),
            new Mp3saveFunction(),
            new DsoundFunction(),
            new DwaveFunction(),
            new DwaveloopFunction(),
            new DwavestopFunction(),
            new DwaveloadFunction(),
            new DwaveplayFunction(),
            new DwaveplayloopFunction(),
            new StopFunction(),
            new Mp3stopFunction(),
            new Mp3fadeinFunction(),
            new BgmFunction(),
            new BgmonceFunction(),
            new BgmstopFunction(),
            new BgmfadeoutFunction(),
            new BgmfadeinFunction(),
            new LoopbgmFunction(),
            new LoopbgmstopFunction(),
            new Mp3volFunction(),
            new ChvolFunction(),
            new VoicevolFunction(),
            new SevolFunction(),
            new BgmvolFunction(),
             //v,dv,mv
            new BgmdownmodeFunction(),

            #endregion Music/SFX Playback

            #region Movie Playback

            new AviFunction(),
            new MpegplayFunction(),
            new MovieFunction(),

            #endregion Movie Playback

            #region Choices

            new SelectcolorFunction(),

            #endregion Choices

            #region Jumps

            new GotoFunction(),
            new SkipFunction(),
            new GosubFunction(),
            new ReturnFunction(),
            new JumpfFunction(),
            new JumpbFunction(),
            new TablegotoFunction(),

            #endregion Jumps

            #region Image Buttons

            new BtndefFunction(),
            new BtnFunction(),

            new Btnwait2Function(),

            #endregion Image Buttons

            #region Wait/Timer

            new _dFunction(),
            new _wFunction(),
            new DelayFunction(),
            new WaitFunction(),

            //Таймеры и спрайты

            #endregion Wait/Timer

            #region Variable Manipulation/Calculations

            new StraliasFunction(),
            new NumaliasFunction(),
            new IntlimitFunction(),
            new DimFunction(),
            new MovFunction(),
            new Mov3Function(),
            new Mov4Function(),
            new Mov5Function(),
            new Mov6Function(),
            new Mov7Function(),
            new Mov8Function(),
            new Mov9Function(),
            new Mov9Function(),
            new MovlFunction(),
            new AddFunction(),
            new SubFunction(),
            new IncFunction(),
            new DecFunction(),
            new MulFunction(),
            new DivFunction(),
            new ModFunction(),
            new RndFunction(),
            new Rnd2Function(),
            new ItoaFunction(),
            new Itoa2Function(),
            new AtoiFunction(),
            new LenFunction(),
            new MidFunction(),
            new SplitFunction(),
            new SinFunction(),
            new CosFunction(),
            new TanFunction(),

            #endregion Variable Manipulation/Calculations

            #region Conditionals/Loops

            new IfFunction(),
            new NotifFunction(),

            #endregion Conditionals/Loops

            #region Right-Click Functionality

            new RmenuFunction(),
            new MenusetwindowFunction(),
            new SavenameFunction(),
            new MenuselectcolorFunction(),

            new RmodeFunction(),

            #endregion Right-Click Functionality

            #region Log Mode

            new LookbackbuttonFunction(),
            new LookbackcolorFunction(),
            new LookbackvoiceFunction(),

            #endregion Log Mode

            #region File Access Logs/Global Variables

            new GlobalonFunction(),

            #endregion File Access Logs/Global Variables

            #region Save/Load

            new SavenumberFunction(),

            new SaveonFunction(),
            new SaveoffFunction(),

            #endregion Save/Load

            #region Special Mode Settings

            new AutomodeFunction(),
            new Automode_timeFunction(),

            #endregion Special Mode Settings

            #region Console

            new VersionstrFunction(),
            new CaptionFunction(),

            #endregion Console

            #region Window Menubar

            new ResetmenuFunction(),

            new InsertmenuFunction(),

            #endregion Window Menubar

            #region PONScripter Commands

            new H_mapfontFunction(),

            #endregion PONScripter Commands

            #region Miscellaneous
             
            new SystemcallFunction(),

            #endregion

            new GameIDFunction(),

            new NsaFunction(),
        };

        #endregion Functions List

        #region Not Suported Functions

        public static List<Function> NotSupportedFunctionsList = new List<Function>()
        {
            new RubyonFunction(),
            new RubyoffFunction(),
        };

        #endregion

        public static bool IsFirstRun = true;

        public static string GameId;
    }

    internal struct UserVariables
    {
        public static Dictionary<NumVariable, int> NumVarList = new Dictionary<NumVariable, int>();
        public static Dictionary<string, string> StringVarList = new Dictionary<string, string>();
        public static Dictionary<string, int[,]> ArrayVarList = new Dictionary<string, int[,]>();
    }

    internal struct Parsing
    {
        public static bool Wait = false;

        public static List<string> ScriptText = new List<string>();
        public static ushort CurrentLine;

        public static Stack<SubroutineLevel> SubroutinesLevels = new Stack<SubroutineLevel>();
    }

    internal struct UserDefine
    {
        public static List<Procedure> ProceduresList = new List<Procedure>();

        public static List<UserFunctionsVariables.UserFunctionsVariables> UserFunctionsVariablesList = new List<UserFunctionsVariables.UserFunctionsVariables>();
        public static List<int> UserFunctionsVariablesIndexsList = new List<int>();
        public static int UserFunctionsLevel = 0;
    }

    internal struct Drawing
    {
        public static bool IsVideoPlaying = false;

        //Включен ли режим обработки изображений в режиме совместимости
        public static bool IsCompatibilityOn = true;

        //Список пользовательских эффектов
        public static Dictionary<ushort, UserEffect> UserEffectsList = new Dictionary<ushort, UserEffect>();

        //Список вшитых эффектов

        public static Dictionary<ushort,Effect> BuiltInEffects = new Dictionary<ushort, Effect>();
        public static double CurrentEffectTime = 0;

        //Применяется ли эффект к бэкграунду?

        //Применяется эффект ли к одному из спрайтов?

        //Список спрайтов
        public static Dictionary<int, Sprite> SpritesList = new Dictionary<int, Sprite>();

        //Бэкгруанд
        public static Texture2D background;
        public static Texture2D NextBackground;


        public static Rectangle BackgroundRectangle = new Rectangle(0, 0, 800, 480);
        public static float backgroundScale = 1f;

        public static GraphicsDevice GraphicsDevice;

        public static Color TintColor = Color.White;
        public static Color NextTintColor = Color.White;

        public static UserEffect BackgroundEffect;

        public static Camera2D Camera;

        //quake effect
        public static QuakeEffect QuakeEffectType = QuakeEffect.None;
        public static int QuakeEffectTime;
        public static int QuakeEffectMaxTime;
        public static int QuakeEffectPixelAmpl;


        public static object backgroundLock = new object();
    }

    internal struct Sound
    {
        public static SoundManager soundmanager = new SoundManager();
    }

    internal struct Video
    {
        public static string PathToVideo;

        public static bool SkipOnTouch = false;

        public static bool StopPlaying = false;

        public static bool IsLooped = false;

        public static Rectangle VideoRect = new Rectangle(0, 0, 800, 480);
    }

    internal struct Delay
    {
        public static bool Sleeping = false;

        public static bool StartOnTouch = false;
        public static bool GetTouch = false;

        public static ushort TimeToSleep = 0;
        public static int ElapsedTime = 0;
    }

    internal struct SaveSystem
    {
        public static string SaveMenuTitle = "Save";
        public static string LoadMenuTitle = "Load";
        public static string SlotTitle = "Slot ";
        public static string CloseButtonTitle = "Close";

        public static int SaveNumber = 9;

        public static bool IsSaveEnabled = false;

        public static List<KeyValuePair<int, DateTime>> Slots;
    }

    internal struct RightMenu
    {
        public static Dictionary<string, string> RightMenuList = new Dictionary<string, string>();

        public static int TextFontWidth;
        public static int TextFontHeight;
        public static int TextSpacingX;
        public static int TextSpacingY;
        public static bool BoldFace;
        public static bool DropShadow;
        public static Color WindowColor;

        public static Color EmptySavefileColor;
        public static Color MouseoffColor;
        public static Color MouseoverColor;

        public static bool IsOn = false;
        public static Vector2 StartPosition;
    }

    internal struct Choices
    {
        public static Color MouseoverColor;
        public static Color MouseoffColor;
    }

    internal struct TextWindow
    {
        public static UserEffect WindowEffect;

        public static Dictionary<int, string> FontsList = new Dictionary<int, string>();

        public static Rectangle TextRect = new Rectangle(0, 0, 800, 480);

        public static bool IsSmthChanged = false;

        public static bool IsShowing = false;

        public static int TextShowMode = 1;

        public static List<StyledChar> CharMode1 = new List<StyledChar>();

        public static StyledChar[,] CharMode2;
        public static Vector2 TextPointer = new Vector2(0, 0);

        public static Color TextColor = Color.White;

        public static bool WaitForClick = false;
        public static int AfteClick;

        public static bool Skip = false;

        public static Sprite ClickWaitCursor;
        public static Vector2 ClickWaitCursorVector;

        public static Vector2 PageWaitCursorVector;
        public static Sprite PageWaitCursor;

        public static Texture2D TextBackground;
        public static Rectangle TextBackgroundRectangle;

        public static List<TextPart> TextParts = new List<TextPart>();
        public static Vector2 TextCurrentPosition = new Vector2(0,0);
        public static bool WaitForClickPressed;
    }

    internal struct LogMode
    {
        public static Sprite PageUpAc;
        public static Sprite PageUpInAc;
        public static Sprite PageDownAc;
        public static Sprite PageDownInAc;

        public static Color TextColor { get; set; }
    }

    internal struct TopMenu
    {
        public static List<MenuItem> MenuList = new List<MenuItem>();
    }

    internal struct UserButtons
    {
        public static Texture2D ButtonsBuffer;

        public static Dictionary<int, UserButton> ButtonsList = new Dictionary<int, UserButton>();

        public static bool WaitForPress = false;

        public static string WaitVariable;

        public static int TimeSpend;
    }

    internal struct System
    {
        public static ContentManager Content; 
        
    }

    public enum GameStateType
    {
        Loading,
        Game,
        Save,
        Load,
        Select
    }
}