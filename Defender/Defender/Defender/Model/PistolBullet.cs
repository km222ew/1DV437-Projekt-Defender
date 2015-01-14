using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Defender.Model
{
    class PistolBullet
    {
        private FloatRectangle position;
        private float velocity;
        private bool hit;

        public PistolBullet(FloatRectangle position)
        {
            this.position = position;
            velocity = -8f;
            hit = false;
        }

        public FloatRectangle Position
        {
            get { return position; }
        }

        public bool Hit
        {
            get { return hit; }
        }

        public bool EnemyCollision(FloatRectangle enemy)
        {
            if (position.isIntersecting(enemy))
            {
                hit = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Update(float gameTime)
        {
            position.updateY(velocity, gameTime);
        }
    }
}
