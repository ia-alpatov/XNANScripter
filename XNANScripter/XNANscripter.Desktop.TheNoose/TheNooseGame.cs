using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using The_noose.Engine.Utils.Camera;
using XNANScripter.Engine;

namespace TheNoose
{
    public class TheNooseGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameManager gm;

        public TheNooseGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            _graphics.PreferredBackBufferWidth = 1400;
            _graphics.PreferredBackBufferHeight = 1050;
            
            XNANScripter.Engine.Config.System.Content = Content;
            gm = new GameManager();

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

            //Инциализиреум конфиги и пр.
            gm.Initialize();
            
            base.Initialize();
        }

        protected override async void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            XNANScripter.Engine.Config.Drawing.GraphicsDevice = GraphicsDevice;
            XNANScripter.Engine.Config.Drawing.BackgroundRectangle = GraphicsDevice.PresentationParameters.Bounds;
            XNANScripter.Engine.Config.TextWindow.TextBackgroundRectangle =
                GraphicsDevice.PresentationParameters.Bounds;

            XNANScripter.Engine.Config.Drawing.Camera = new Camera2D(GraphicsDevice.Viewport);

            await gm.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gm.Update(gameTime.ElapsedGameTime, gameTime.TotalGameTime);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            gm.Draw(gameTime.ElapsedGameTime, gameTime.ElapsedGameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}
