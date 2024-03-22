using GameDev.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection.Metadata;

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
        Player _player;

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


            var playerTexture = Content.Load<Texture2D>("knight");
            var playerAtlas = TextureAtlas.Create("Animations/Player", playerTexture, 84, 84);
            var playerFactory = new SpriteSheetAnimationFactory(playerAtlas);
            playerFactory.Add("idle_down", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }));
            playerFactory.Add("down", new SpriteSheetAnimationData(new[] { 4, 5, 6, 7, 8 }, isLooping: false));
            playerFactory.Add("up", new SpriteSheetAnimationData(new[] { 9, 10, 11, 12, 13 }, isLooping: false));
            playerFactory.Add("idle_up", new SpriteSheetAnimationData(new[] { 29 }));
            playerFactory.Add("idle_right", new SpriteSheetAnimationData(new[] { 14 }));
            playerFactory.Add("right", new SpriteSheetAnimationData(new[] { 15, 16, 17, 18, 19 }, isLooping: false));
            playerFactory.Add("idle_left", new SpriteSheetAnimationData(new[] { 20 }));
            playerFactory.Add("left", new SpriteSheetAnimationData(new[] { 21, 22, 23, 24, 25 }, isLooping: false));
            playerFactory.Add("atk_down", new SpriteSheetAnimationData(new[] { 27, 28 }, isLooping: false));
            playerFactory.Add("atk_up", new SpriteSheetAnimationData(new[] { 30, 31 }, isLooping: false));
            playerFactory.Add("atk_right", new SpriteSheetAnimationData(new[] { 34, 33 }, isLooping: false));
            playerFactory.Add("atk_left", new SpriteSheetAnimationData(new[] { 37, 36 }, isLooping: false));
            _player = new Player(playerFactory);
            _player.Position = new Vector2(100, 100);






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
               
                _player.Update(gameTime);


                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {

                   
                    _player.MoveLeft();

                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
 
                    _player.MoveRight();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.W))
                {

                    _player.MoveUp();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.S))
                {

                    _player.MoveDown();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    _player.Attack();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    _player.Fire();
                }
                else
                {
                    _player.ReturnToIdle();
                }      
                
            }





            if (_player1Score >= _maxScore || _player2Score >= _maxScore)
            {
                _gameStates = GameStates.Ended;
            }

            base.Update(gameTime);
        }

        private void Test()
        {

        }

        protected override void Draw(GameTime gameTime)
        {
           
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            
            _spriteBatch.DrawRectangleF(_player.BoundingBox, Color.Red);            
            _spriteBatch.DrawString(_gameFont, _player1Score.ToString(),_playerScoreLocation1, Color.Black);
            _spriteBatch.DrawString(_gameFont, _player2Score.ToString(),_playerScoreLocation2, Color.Black); ;



            _player.Draw(_spriteBatch);

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
