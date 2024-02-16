using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Security.Principal;

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
        private float _ballSpeed;
        private SpriteFont _gameScore;
        private Vector2 _paddleVelocity1;
        private Vector2 _paddleVelocity2;
        private Vector2 _player1ScoreLocation;
        private Vector2 _player2ScoreLocation;
        private SoundEffect _pingSound;
        private Texture2D _background;
        AnimatedSprite _axeGuyAttack;
        AnimatedSprite _axeGuyWalk;
        private Vector2 _axeGuyWalkVelocity;
        private Rectangle _axeGuyLocation;

        //   animations["walk"]    (animationnames, animatedsprites)

        private int _player1Score;
        private int _player2Score;
        private int _maxScore;
        

        private enum GameStates {
            Setup, Playing, Ended
        }

        private GameStates _gameStates;
        


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //16:9 ratio
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            
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
            _gameScore = Content.Load<SpriteFont>("gamefont");
            _pingSound = Content.Load<SoundEffect>("ping");
            _background = Content.Load<Texture2D>("bg");
            _axeGuyAttack = new AnimatedSprite(Content, "Woodcutter_attack1", 48, 48, 6);
            _axeGuyWalk = new AnimatedSprite(Content, "Woodcutter_walk", 48, 48, 6);
            // Woodcutter_walk
            _axeGuyAttack.SetAnimation(6);
            _axeGuyAttack.Start();

            _axeGuyWalk.SetAnimation(6);
            _axeGuyWalk.Start();
            //_axeGuyWalkVelocity = new Vector2(0, 0);
            _axeGuyLocation = new Rectangle(400, 200, 144,144);

            _player1Score = 0;
            _player2Score = 0;
            _maxScore = 3;

            ResetRound();

            // TODO: use this.Content to load your game content here
        }

        private void ResetRound()
        {
            _player1ScoreLocation = new Vector2(10, 10);
            _player2ScoreLocation = new Vector2(10, 600);
            _ballDirection = new Vector2(2, 1);
            _ballSpeed = 200;
            /*
                    ---------------------------------
                   |                  50
                   |               ----   25
                   |                -    75
                                    
                       x------------>y           
                    |               
                    |             ----
                    |               Window.Height-50  x
                    ------------------------------------
             */


            _paddleLocation1 = new Rectangle(Window.ClientBounds.Width / 2, 50, 200, 25);
            _paddleLocation2 = new Rectangle(Window.ClientBounds.Width / 2, Window.ClientBounds.Height - 50, 200, 25);
            _ballLocation = new Rectangle(Window.ClientBounds.Width / 2, 75, 25, 25);
        }


        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float paddleSpeed = 400f;
            float walkSpeed = 400f;

            _axeGuyAttack.Update(gameTime);
            _axeGuyWalk.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                
                _axeGuyWalkVelocity.X = -walkSpeed;

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                //_axeGuyWalk.Start();
                _axeGuyWalkVelocity.X = walkSpeed;
            }
            else
            {
                //_axeGuyWalk.Stop();
                _axeGuyWalkVelocity.X = 0;
            }
            _axeGuyLocation.Offset(_axeGuyWalkVelocity * deltaTime);

            if (_gameStates != GameStates.Playing)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                { 
                    _gameStates = GameStates.Playing; 
                }
            }

            if (_gameStates == GameStates.Playing)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    _paddleVelocity1.X = -paddleSpeed;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    _paddleVelocity1.X = paddleSpeed;
                }
                else
                {
                    _paddleVelocity1.X = 0;
                }


                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    _paddleVelocity2.X = -paddleSpeed;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    _paddleVelocity2.X = paddleSpeed;
                }
                else
                {
                    _paddleVelocity2.X = 0;
                }

                _paddleLocation1.Offset(_paddleVelocity1 * deltaTime);
                _paddleLocation2.Offset(_paddleVelocity2 * deltaTime);





                _ballLocation.Location = new Point(
                   _ballLocation.X + (int)(_ballDirection.X * _ballSpeed * deltaTime),
                   _ballLocation.Y + (int)(_ballDirection.Y * _ballSpeed * deltaTime)
                   );

                CheckCollisions();
            }
            else if (_gameStates == GameStates.Ended)
            { 
                
            }

            base.Update(gameTime);
        }

        private void CheckCollisions()
        {
            //(0,0) |----------------------------  |
            //      |  []                       [] |
            //      |
            if (_ballLocation.Left < 0 || _ballLocation.Right > Window.ClientBounds.Width)
            {
                // 8 = -8
                // -8 = 8
                _pingSound.Play();
                _ballDirection.X *= -1; //reverse the x direction
            }

            if (_ballLocation.Intersects(_paddleLocation1) || _ballLocation.Intersects(_paddleLocation2))
            {
                _pingSound.Play();
                _ballDirection.Y *= -1; //reverse the x direction
            }

            if (_ballLocation.Top <= 0)
            {
                _player2Score += 1;
                ResetRound();
            }

            if (_ballLocation.Bottom >= Window.ClientBounds.Height)
            {
                _player1Score += 1;
                ResetRound();
            }

            if (_player1Score >= _maxScore || _player2Score >= _maxScore)
            { 
                _gameStates = GameStates.Ended;
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();


            // _spriteBatch.Draw(_background, new Rectangle(0,0,1280,720), Color.White);

            //_axeGuyAttack.Draw(_spriteBatch, new Vector2(200, 200), Color.White);

            _axeGuyWalk.Draw(_spriteBatch, _axeGuyLocation, Color.White);

            _spriteBatch.Draw(_paddle, _paddleLocation1, Color.White);
            _spriteBatch.Draw(_paddle, _paddleLocation2, Color.White);
            _spriteBatch.Draw(_ball,   _ballLocation, Color.Red);
            _spriteBatch.DrawString(_gameScore, _player1Score.ToString(),_player1ScoreLocation, Color.Black);
            _spriteBatch.DrawString(_gameScore, _player2Score.ToString(),_player2ScoreLocation, Color.Black); ;

             

            if(_gameStates == GameStates.Ended)
            {
                string winner = _player1Score > _player2Score ? "Player 1" : "Player 2";
                _spriteBatch.DrawString(_gameScore, $"{winner} Wins!", new Vector2( Window.ClientBounds.Width/2, Window.ClientBounds.Height / 2), Color.Black); ;

            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
