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
        Attack, Idle, RunLeft, RunRight, RunUp, RunDown, Death
    }

    internal class Actor
    {
        public Rectangle Location { get; set; }
        public Rectangle CollisionBox { get; set; }
        public int Scale { get; set; }
        private Vector2 Velocity;
        public float MovementSpeed { get; set; } = 100f;
        private ContentManager _content; 
        public AnimationTypes CurrentAnimationType { get; set; }
        public AnimatedSprite_Legacy CurrentAnimation { get; set; }

        public Dictionary<AnimationTypes, AnimatedSprite_Legacy> Animations { get; set; }

        public void SetCollisionBox(Rectangle collisionBox, int yoffset)
        {
            CollisionBox = new Rectangle(collisionBox.X, collisionBox.Y + (yoffset*Scale), collisionBox.Width * Scale, collisionBox.Height * Scale);
        }

        public Actor(ContentManager content, Rectangle startLocation, int scale) 
        {
            _content = content;
            Location = new Rectangle(startLocation.X, startLocation.Y , (startLocation.Width * scale) , (startLocation.Height * scale) );
            Scale = scale;

            Animations = new Dictionary<AnimationTypes, AnimatedSprite_Legacy>();
            CurrentAnimationType = AnimationTypes.Idle;
            MovementSpeed = 200f;
        }

        public void Init(AnimationTypes initialAnimation)
        {
            CurrentAnimationType = initialAnimation;
            CurrentAnimation = Animations[CurrentAnimationType];
        }

        public void AddSprite(AnimationTypes animationType, string filename, int frameWidth, int frameHeight, int fps, int frameCount)
        {
            var anim = new AnimatedSprite_Legacy(_content, filename, frameWidth, frameHeight, fps);
            anim.SetAnimation(frameCount);
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

        public void Attack()
        {
            if (Animations.ContainsKey(AnimationTypes.Attack))
            {
                CurrentAnimation = Animations[AnimationTypes.Attack];
                CurrentAnimationType = AnimationTypes.Attack;
            }
        }

        public void Die()
        {
            if (Animations.ContainsKey(AnimationTypes.Death))
            {
                CurrentAnimation = Animations[AnimationTypes.Death];
                CurrentAnimationType = AnimationTypes.Death;
            }
        }

        public void Idle()
        {
            if (Animations.ContainsKey(AnimationTypes.Idle))
            {
                CurrentAnimation = Animations[AnimationTypes.Idle];
                CurrentAnimationType = AnimationTypes.Idle;
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

        public void RunUp()
        {
            if (Animations.ContainsKey(AnimationTypes.RunUp))
            {
                CurrentAnimation = Animations[AnimationTypes.RunUp];
                CurrentAnimationType = AnimationTypes.RunUp;
            }
        }
        public void RunDown()
        {
            if (Animations.ContainsKey(AnimationTypes.RunDown))
            {
                CurrentAnimation = Animations[AnimationTypes.RunDown];
                CurrentAnimationType = AnimationTypes.RunDown;
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
            if (CurrentAnimationType == AnimationTypes.RunUp)
            {
                Velocity.Y = -MovementSpeed;
            }
            if (CurrentAnimationType == AnimationTypes.RunDown)
            {
                Velocity.Y = MovementSpeed;
            }
            if (CurrentAnimationType != AnimationTypes.RunLeft && CurrentAnimationType != AnimationTypes.RunRight)
            {
                Velocity = Vector2.Zero;
            }
            Location = new Rectangle((int)(Location.X + Velocity.X * deltaTime), (int)(Location.Y + Velocity.Y * deltaTime), Location.Width, Location.Height);
            CollisionBox = new Rectangle((int)(CollisionBox.X + Velocity.X * deltaTime), (int)(CollisionBox.Y + Velocity.Y * deltaTime), CollisionBox.Width, CollisionBox.Height);
            //Location.Offset(Velocity * deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch, Color blendedColor)
        {
            if (CurrentAnimationType == AnimationTypes.RunLeft)
            {
                // Calculate the adjusted position for when the sprite is flipped horizontally.
                var flippedRect = new Rectangle((Location.X - Location.Width) + CurrentAnimation.FrameWidth * Scale, Location.Y, CurrentAnimation.FrameWidth * Scale, CurrentAnimation.FrameHeight * Scale);

                CurrentAnimation.Draw(spriteBatch, flippedRect, blendedColor, SpriteEffects.FlipHorizontally);
            }
            else
            {
                CurrentAnimation.Draw(spriteBatch, Location, blendedColor);
            }
        }



    }
}
