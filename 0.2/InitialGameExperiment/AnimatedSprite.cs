using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InitialGameExperiment
{
    internal class AnimatedSprite
    {
        private Texture2D _texture;
        private float _timePerFrame;

        public int FrameWidth { get;  private set; }
        public int FrameHeight { get; private set; }

        public bool IsLooping { get; set; }
        private bool _isAnimating = false;

        private int _currentFrame;
        private int _animationFrameCount;
        private float _totalElapsed;

        public AnimatedSprite(ContentManager contentManager, string textureName, int frameWidth, int frameHeight, float framesPerSeconds)
        { 
            _texture = contentManager.Load<Texture2D>(textureName);
            FrameHeight = frameHeight;
            FrameWidth = frameWidth;
            _timePerFrame = 1.0f / framesPerSeconds;
        }

        public void SetAnimation(int frameCount, bool isLooping = true) 
        {
            _animationFrameCount = frameCount;
            IsLooping = isLooping;
            _currentFrame = 0;
            _totalElapsed = 0;
        }


        public void Update(GameTime gameTime)
        {
            if(!_isAnimating) return;

            _totalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if(_totalElapsed > _timePerFrame)
            {
                _currentFrame++;
                if (_currentFrame == _animationFrameCount)
                {
                    _currentFrame = 0;
                    // handle when not allowed to loop
                    if(!IsLooping)
                    {
                        _isAnimating = false;
                    }
                }
                _totalElapsed -= _timePerFrame;
            }        
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle position, Color color, SpriteEffects effect = SpriteEffects.None) 
        {
            int frameIndex = _currentFrame % _animationFrameCount;

            Rectangle sourceRect = new Rectangle(FrameWidth * frameIndex, 0, FrameWidth, FrameHeight);

            //Rectangle destRect = new Rectangle((int) position.X,(int) position.Y, 144, 144);

            spriteBatch.Draw(_texture, position, sourceRect, color, 0 , Vector2.Zero, effect, 0);
               /*
                     -------------------------
                    |      x





                            Source and Dest
                    [x.......x]    =>    |----------------------------
                      [x]                |   [ ]
                                         |
                */


        }


        public void Start()
        {
            _isAnimating = true;
            _currentFrame = 0;
            _totalElapsed = 0;
        }

        public void Stop()
        {
            _isAnimating = false;
        }


    }
}
