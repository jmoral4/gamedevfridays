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
    enum AnimationTypes
    { 
        Attack, Idle, RunLeft, RunRight, Jump
    }

    internal class Player
    {
        public Rectangle Location { get; set; }
        private Vector2 Velocity;
        public float MovementSpeed { get; set; } = 100f;
        private ContentManager _content; 
        public AnimationTypes CurrentAnimationType { get; set; }
        public AnimatedSprite CurrentAnimation { get; set; }

        public Dictionary<AnimationTypes, AnimatedSprite> Animations { get; set; }

        public Player(ContentManager content, Rectangle startLocation) 
        {
            _content = content;
            Location = startLocation;
            Animations = new Dictionary<AnimationTypes, AnimatedSprite>();
            CurrentAnimationType = AnimationTypes.Idle;
            MovementSpeed = 200f;
        }

        public void Init(AnimationTypes initialAnimation)
        {
            CurrentAnimationType = initialAnimation;
            CurrentAnimation = Animations[CurrentAnimationType];
        }

        public void AddSprite(AnimationTypes animationType, string filename, int frameWidth, int frameHeight, int fps)
        {
            var anim = new AnimatedSprite(_content, filename, frameWidth, frameHeight, fps);
            anim.SetAnimation(6);
            anim.Start();
            Animations.Add(animationType, anim);
        }

        public void RunLeft()
        {
            if (Animations.ContainsKey(AnimationTypes.RunLeft))
            {
                CurrentAnimation = Animations[AnimationTypes.RunLeft];
                CurrentAnimationType = AnimationTypes.RunLeft;
            }

        }

        public void RunRight()
        {
            if (Animations.ContainsKey(AnimationTypes.RunRight))
            {
                CurrentAnimation = Animations[AnimationTypes.RunRight];
                CurrentAnimationType = AnimationTypes.RunRight;
            }
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ( Animations.ContainsKey(CurrentAnimationType) )
            {
                var currentAnimation = Animations[CurrentAnimationType];
                currentAnimation.Update(gameTime);
            }

            if( CurrentAnimationType == AnimationTypes.RunLeft )
            {
                Velocity.X = -MovementSpeed;
            }
            if (CurrentAnimationType == AnimationTypes.RunRight)
            {
                Velocity.X = MovementSpeed;
            }
            Location.Offset(Velocity * deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentAnimationType == AnimationTypes.RunLeft)
            {
                CurrentAnimation.Draw(spriteBatch, Location, Color.White, SpriteEffects.FlipHorizontally);

            }
            else
            {
                CurrentAnimation.Draw(spriteBatch, Location, Color.White);
            }
        }


    }
}
