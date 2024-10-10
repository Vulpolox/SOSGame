using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Myra;
using Myra.Graphics2D.UI;

namespace SOSGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GUIHandler _guiHandler;

        // Other Graphics
        Texture2D background;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // set the window size
            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {

            // Initialize UI
            _guiHandler = new GUIHandler(this);
            _guiHandler.InitializeGUIElements();

            // Initialize Other Graphics
            background = Content.Load<Texture2D>("GrayBackground");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            // Draw Other Stuff
            _spriteBatch.Begin();

            _spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            _spriteBatch.End();


            // Draw GUI Elements
            _guiHandler.Draw();

            base.Draw(gameTime);
        }
    }
}
