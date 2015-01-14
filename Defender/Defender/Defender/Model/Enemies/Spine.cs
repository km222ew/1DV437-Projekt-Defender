using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defender.Model.Enemies
{
    class Spine : Enemy
    {
        private FloatRectangle position;
        private float speed;
        private int health;
        private int damage;
        private float attackSpeed;
        private float attackSpeedTimeElapsed;

        public Spine(FloatRectangle position, float speed) 
            :base(position, speed)
        {
            this.position = position;
            this.speed = speed;

            health = 4;
            damage = 3;
            attackSpeed = 1f;
        }

        public override float AttackSpeedTimeElapsed
        {
            get { return attackSpeedTimeElapsed; }
            set { attackSpeedTimeElapsed = value; }
        }

        public override int Health
        {
            get { return health; }
        }

        public override void Hit()
        {
            health -= 1;
        }

        public override bool HasStopped()
        {
            if (speed == 0)
            {
                return true;
            }

            return false;
        }

        public override void Stop()
        {
            speed = 0;
        }

        public override float AttackSpeed
        {
            get { return attackSpeed; }
        }

        public override int Attack()
        {
            return damage;
        }

        public override FloatRectangle Position
        {
            get { return position; }
        }

        public override void Update(float gameTime)
        {
            position.updateY(speed, gameTime);
        }
    }
}
