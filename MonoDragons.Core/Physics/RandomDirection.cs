﻿using Microsoft.Xna.Framework;

namespace MonoDragons.Core.Physics
{
    public class RandomDirection
    {
        private Vector2 _direction;

        public Vector2 Get()
        {
            if (_direction == Vector2.Zero)
                ResolveRandomDirection();
            return _direction;
        }

        private void ResolveRandomDirection()
        {
            _direction = new PhysicsEngine.Physics().GetDirectionTowards(Vector2.Zero, new Vector2(Rng.Int(-100, 100), Rng.Int(-100, 100)));
        }
    }
}
