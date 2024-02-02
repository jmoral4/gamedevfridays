using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace InitialGameExperiment
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _ball;
        private Texture2D _paddle;
        private Rectangle _ballLocation;
        private Rectangle _paddleLocation1;
        private Rectangle _paddleLocation2;
        private Vector2 _ballDirection;
        private int _ballSpeed;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _ball = Content.Load<Texture2D>("ball");
            _paddle = Content.Load<Texture2D>("paddle");
            _ballDirection = new Vector2(0, 1);
            _ballSpeed = 2;
            /*
                    ---------------------------------
                   |                  50
                   |              ----   25
                   |                -    75
                                    
                                  
                    |               
                    |             ----
                    |               Window.Height-50
                    ------------------------------------
             */


            _paddleLocation1 = new Rectangle(Window.ClientBounds.Width/2 , 50, 200, 25);
            _paddleLocation2 = new Rectangle(Window.ClientBounds.Width / 2, Window.ClientBounds.Height-50, 200, 25);
            _ballLocation = new Rectangle(Window.ClientBounds.Width / 2, 75, 25, 25);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            int movementSpeed = 5;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _paddleLocation1.Location = new Point(_paddleLocation1.X - movementSpeed, _paddleLocation1.Y);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _paddleLocation1.Location = new Point(_paddleLocation1.X + movementSpeed, _paddleLocation1.Y);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _paddleLocation2.Location = new Point(_paddleLocation2.X - movementSpeed, _paddleLocation2.Y);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _paddleLocation2.Location = new Point(_paddleLocation2.X + movementSpeed, _paddleLocation2.Y);
            }

            _ballLocation.Location = new Point(_ballLocation.X + (int)(_ballDirection.X * _ballSpeed), _ballLocation.Y + (int)(_ballDirection.Y * _ballSpeed));



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_paddle, _paddleLocation1, Color.White);
            _spriteBatch.Draw(_paddle, _paddleLocation2, Color.White);
            _spriteBatch.Draw(_ball,   _ballLocation, Color.Red);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
