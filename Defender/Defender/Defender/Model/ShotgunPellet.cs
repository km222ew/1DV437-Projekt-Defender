using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defender.Model
{
    class ShotgunPellet
    {
        private FloatRectangle position;
        private float velocity;
        private int totalDamage;

        public ShotgunPellet(FloatRectangle position)
        {
            this.position = position;
            velocity = -5f;
            totalDamage = 6;
        }

        public int TotalDamage
        {
            get { return totalDamage; }
        }

        public FloatRectangle Position
        {
            get { return position; }
        }

        public bool EnemyCollision(FloatRectangle enemy)
        {
            if (position.isIntersecting(enemy))
            {
                totalDamage -= 1;
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
