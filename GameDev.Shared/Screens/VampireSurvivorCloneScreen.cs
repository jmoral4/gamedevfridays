using GameDev.Shared.BaseServices;
using GameDev.Shared.Screens.General;
using GameDev.Shared.Screens.System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MonoGame.Extended;

namespace GameDev.Shared.Screens
{
    internal class VampireSurvivorCloneScreen : GameScreen
    {
        SpriteFont _gameFont;
        InputAction escapeAction;
        InputAction pauseAction;
        ContentManager content;
        float pauseAlpha;
        bool paused = false;
        private FrameCounter _frameCounter = new FrameCounter();

        public VampireSurvivorCloneScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            escapeAction = new InputAction(
                new Buttons[] { Buttons.Back },
                new Keys[] { Keys.Escape },
                true);

            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start },
                new Keys[] { Keys.Space }, true);

        }       

        public override void Activate(bool instancePreserved)
        {
           if (!instancePreserved)
            {

                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");
                
                OSInfo.Instance.DebugListAllStats();

                //load the textures
                _gameFont = content.Load<SpriteFont>("Fonts/gamefont");

                //UncapFramerate();
            }

        }

        private void UncapFramerate()
        {
            //toggle fixed (60fps locked) vs unlocked timestep
            this.ScreenManager.Game.IsFixedTimeStep = false;

            // Get the current graphics device manager and disable V-Sync
            var graphicsDeviceManager = this.ScreenManager.Game.Services.GetService(typeof(IGraphicsDeviceManager)) as GraphicsDeviceManager;
            if (graphicsDeviceManager != null)
            {
                graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
                graphicsDeviceManager.ApplyChanges();
            }
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            base.Unload();
        }

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            // Gradually fade in or out depending on whether we are covered by the pause screen.
            base.Update(gameTime, otherScreenHasFocus, false);
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (!paused)
            {

            }

        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;
            GamePadState gamepadState = input.CurrentGamePadStates[playerIndex];
            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamepadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];


            PlayerIndex player;
            if (escapeAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                // escape screen
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else if (pauseAction.Evaluate(input, ControllingPlayer, out player))
            {
                // simple game pause
                paused = !paused;
            }           
            else
            {
                // handle input in generic case
            }

            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                var mousePosition = new Point(mouseState.X, mouseState.Y);                
                Debug.WriteLine($"MOUSE:{mousePosition.X},{mousePosition.Y}");
            }

        }

        public override void Draw(GameTime gameTime)
        {            
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _frameCounter.Update(deltaTime);
            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                  Color.CornflowerBlue, 0, 0);

            SpriteBatch _spriteBatch = ScreenManager.SpriteBatch;

            _spriteBatch.Begin();
            // Draw everything here...
            _spriteBatch.End();            


            
            // DRAW DEBUG INFO ON TOPMOST LAYER
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_gameFont, fps, new Microsoft.Xna.Framework.Vector2(1, 1), Color.Black);            
            _spriteBatch.End();

        }
    }



}

