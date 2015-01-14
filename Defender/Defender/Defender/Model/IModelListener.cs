using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Defender.Model
{
    public interface IModelListener
    {
        void PistolShot();
        void ShotgunShot(Vector2 playerPos);
        void EnemyDeath(FloatRectangle position);
        void EnemyHit();
        void PlayerHit();
        void PlayerDeath();
    }
}
