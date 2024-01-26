namespace GameDev.Shared.Animation;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class SpriteSheetAnimatedSprite
{
    private Texture2D _spriteSheet;
    private float _timePerFrame;
    private float _totalElapsed;
    private bool _isAnimating;

    // Stores the rectangles for each frame of each animation state
    private Dictionary<AnimationState, List<Rectangle>> _animations;

    public bool IsLooping { get; set; }
    private AnimationState _currentState;

    public SpriteSheetAnimatedSprite(ContentManager content, string textureName, Dictionary<AnimationState, List<Rectangle>> animations, float framesPerSecond)
    {
        _spriteSheet = content.Load<Texture2D>(textureName);
        _animations = animations;
        _timePerFrame = 1.0f / framesPerSecond;
        _isAnimating = true;
        IsLooping = true;
    }

    public void SetAnimation(AnimationState state, bool isLooping = true)
    {
        _currentState = state;
        IsLooping = isLooping;
        _currentFrame = 0;
        _totalElapsed = 0;
    }

    public void Update(GameTime gameTime)
    {
        if (!_isAnimating) return;

        _totalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_totalElapsed > _timePerFrame)
        {
            _currentFrame++;
            if (_currentFrame == _animations[_currentState].Count)
            {
                _currentFrame = 0;
                if (!IsLooping)
                {
                    _isAnimating = false;
                }
            }
            _totalElapsed -= _timePerFrame;
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
    {
        List<Rectangle> frames = _animations[_currentState];
        Rectangle sourceRect = frames[_currentFrame];
        spriteBatch.Draw(_spriteSheet, position, sourceRect, color);
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

    private int _currentFrame;
}
