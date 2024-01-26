namespace GameDev.Shared.Animation;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


public class IndividualTextureAnimatedSprite
{
    private Dictionary<AnimationState, Texture2D> _textures;
    private float _timePerFrame;
    private float _totalElapsed;
    private bool _isAnimating;

    public int FrameWidth { get; private set; }
    public int FrameHeight { get; private set; }
    public bool IsLooping { get; set; }

    private int _currentFrame;
    private int _animationFrameCount;
    private int _currentRow;
    private AnimationState _currentState;

    public IndividualTextureAnimatedSprite(ContentManager content, Dictionary<AnimationState, string> textureNames, int frameWidth, int frameHeight, float framesPerSecond)
    {
        _textures = new Dictionary<AnimationState, Texture2D>();
        foreach (var pair in textureNames)
        {
            _textures.Add(pair.Key, content.Load<Texture2D>(pair.Value));
        }

        FrameWidth = frameWidth;
        FrameHeight = frameHeight;
        _timePerFrame = 1.0f / framesPerSecond;
        _isAnimating = true;
    }

    public void SetAnimation(AnimationState state, int row, int frameCount, bool isLooping = true)
    {
        _currentState = state;
        _currentRow = row;
        _animationFrameCount = frameCount;
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
            // Loop the animation
            if (_currentFrame == _animationFrameCount)
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
        if (!_textures.TryGetValue(_currentState, out Texture2D texture))
        {
            // Handle the case where the texture is not found.
            return;
        }

        int frameIndex = _currentFrame % _animationFrameCount;
        Rectangle sourceRect = new Rectangle(FrameWidth * frameIndex, FrameHeight * _currentRow, FrameWidth, FrameHeight);
        spriteBatch.Draw(texture, position, sourceRect, color);
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
