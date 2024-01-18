using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDev.Shared.Utility
{
    internal class RotationHelper
    {
        private const float FULL_ROTATION = 360f;

        // Rotates every 60ms (not mapped to framerate)
        public void FixedRotate360(IRotatable obj, GameTime gameTime, float rotationAmount = 0.25f)
        {
            obj.elapsedMs += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (obj.elapsedMs > 60)
            {
                obj.elapsedMs = 0;
                obj.rotation += rotationAmount;
                if (obj.rotation > 360)
                    obj.rotation = 0;
            }
        }
       
        // rotate independent of framerate, calculate rotation speed based on desired degrees and time to rotate
        public void RotateByDegreesInTime(IRotatable obj, GameTime gameTime, float degrees, float time)
        {
            float rotationSpeed = degrees / time;
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            obj.rotation += rotationSpeed * elapsedSeconds;
            if (obj.rotation > FULL_ROTATION)
                obj.rotation -= FULL_ROTATION;
        }
    }
}
