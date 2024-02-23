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
        private SpriteFont _gameFont;
        private Vector2 _player1ScoreLocation;
        private Vector2 _player2ScoreLocation;        
        AnimatedSprite _axeGuyAttack;
        AnimatedSprite _axeGuyWalk;
        private Vector2 _axeGuyWalkVelocity;
        private Rectangle _axeGuyLocation;
        private int _player1Score;
        private int _player2Score;
        private int _maxScore;
        Player _p1;
        

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
            _gameFont = Content.Load<SpriteFont>("gamefont");


            _p1 = new Player(Content, new Rectangle(200, 200, 144, 144));
            _p1.AddSprite(AnimationTypes.RunLeft, "Woodcutter_walk", 48, 48, 6);
            _p1.AddSprite(AnimationTypes.RunRight, "Woodcutter_walk", 48, 48, 6);
            _p1.AddSprite(AnimationTypes.Idle, "Woodcutter_attack1", 48, 48, 6);
            _p1.Init(AnimationTypes.Idle);

            _axeGuyAttack = new AnimatedSprite(Content, "Woodcutter_attack1", 48, 48, 6);
            _axeGuyWalk = new AnimatedSprite(Content, "Woodcutter_walk", 48, 48, 6);
            // Woodcutter_walk
            _axeGuyAttack.SetAnimation(6);
            _axeGuyAttack.Start();

            _axeGuyWalk.SetAnimation(6);
            _axeGuyWalk.Start();

            _player1Score = 0;
            _player2Score = 0;
            _maxScore = 3;

            InitializeGameBoard();

            // TODO: use this.Content to load your game content here
        }

        private void InitializeGameBoard()
        {
            _player1ScoreLocation = new Vector2(10, 10);
            _player2ScoreLocation = new Vector2(10, 600);
            _axeGuyLocation = new Rectangle(400, 200, 144, 144);
            _gameStates = GameStates.Setup;
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
        }


        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;            
            float walkSpeed = 200f;


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (_gameStates != GameStates.Playing)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                { 
                    _gameStates = GameStates.Playing; 
                }
            }
            else if (_gameStates == GameStates.Playing)
            {
                _p1.Update(gameTime);

                _axeGuyAttack.Update(gameTime);
                _axeGuyWalk.Update(gameTime);

                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    _p1.RunLeft();
                    _axeGuyWalkVelocity.X = -walkSpeed;

                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    _p1.RunRight();
                    _axeGuyWalkVelocity.X = walkSpeed;
                }
                else
                {
                    _axeGuyWalkVelocity.X = 0;
                }
                _axeGuyLocation.Offset(_axeGuyWalkVelocity * deltaTime);
            }


            if (_player1Score >= _maxScore || _player2Score >= _maxScore)
            {
                _gameStates = GameStates.Ended;
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _p1.Draw(_spriteBatch);
            _axeGuyWalk.Draw(_spriteBatch, _axeGuyLocation, Color.White);
            _spriteBatch.DrawString(_gameFont, _player1Score.ToString(),_player1ScoreLocation, Color.Black);
            _spriteBatch.DrawString(_gameFont, _player2Score.ToString(),_player2ScoreLocation, Color.Black); ;

             

            if(_gameStates == GameStates.Ended)
            {
                string winner = _player1Score > _player2Score ? "Player 1" : "Player 2";
                _spriteBatch.DrawString(_gameFont, $"{winner} Wins!", new Vector2( Window.ClientBounds.Width/2, Window.ClientBounds.Height / 2), Color.Black); ;

            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
