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
        private int _player1Score;
        private int _player2Score;
        private int _maxScore;
        private Vector2 _playerScoreLocation1;
        private Vector2 _playerScoreLocation2;
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

            _playerScoreLocation1 = new Vector2(10, 10);
            _playerScoreLocation2 = new Vector2(10, _graphics.GraphicsDevice.Viewport.Height - 100);

            _player1Score = 0;
            _player2Score = 0;
            _maxScore = 3;

            InitializeGameBoard();

            // TODO: use this.Content to load your game content here
        }

        private void InitializeGameBoard()
        {            
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

                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    _p1.RunLeft();                    

                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    _p1.RunRight();                    
                }
                else
                {
                    _p1.Idle();
                }               
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
            _spriteBatch.DrawString(_gameFont, _player1Score.ToString(),_playerScoreLocation1, Color.Black);
            _spriteBatch.DrawString(_gameFont, _player2Score.ToString(),_playerScoreLocation2, Color.Black); ;

             

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
