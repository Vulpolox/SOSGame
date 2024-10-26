using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Myra;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;

// note: for record game feature, use Monogame.Extend for coroutine support
// for delays between moves

namespace SOSGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        protected GUIHandler _guiHandler;

        // line stuff
        protected List<Line> _lines;
        Texture2D _lineTexture;

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

            // Initialize Line Stuff
            _lines = new List<Line>();
            _lineTexture = new Texture2D(GraphicsDevice, 1, 1);
            _lineTexture.SetData(new[] { Color.White });

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


            //draw lines to mark SOSs
            _spriteBatch.Begin();

            foreach (var line in this._lines)
            {
                DrawLine(_spriteBatch, this._lineTexture, line);
            }

            // draw test pixel
            DrawTestPixel(_spriteBatch, _lineTexture, 390, 200);

            _spriteBatch.End();
                      

            base.Draw(gameTime);
        }

        // method for drawing lines
        private void DrawLine(SpriteBatch spriteBatch, Texture2D lineTexture, Line line)
        {
            // calculate the direction and the length of the line
            Vector2 direction = line.EndCoords - line.StartCoords;
            float length = direction.Length();
            float rotation = (float)Math.Atan2(direction.Y, direction.X);

            // draw the line
            spriteBatch.Draw(
                     lineTexture,
                     line.StartCoords,              // Position at startPoint
                     null,                          // No source rectangle
                     line.LineColor,                // Use the line's color
                     rotation,                      // Rotate to match the line's angle
                     Vector2.Zero,                  // Origin at top-left of the texture
                     new Vector2(length, 1f),       // Scale width to length, height to 1 (thin line)
                     SpriteEffects.None,
                     0f);                           // Layer depth
        }


        private void DrawTestPixel(SpriteBatch spriteBatch, Texture2D texture, int x, int y)
        {
            spriteBatch.Draw(texture, new Vector2(x, y), Color.Yellow);
        }


        // method for adding lines to the List from which they are drawn
        public void AddLine(Line line) 
        { 
            _lines.Add(line);
            Console.WriteLine(line);
        }


        // method for cleaning up lines from previous game
        public void ClearLines()
        {
            _lines.Clear();
        }
    }
}
