using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public class BasicBrick : Brick
    {
        protected override void Start()
        {
            base.Start();
            _life = 1;
            _type = BrickType.Basic;
            _currentLife = _life;
        }
    }
}