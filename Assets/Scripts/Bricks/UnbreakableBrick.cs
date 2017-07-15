using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public class UnbreakableBrick : Brick
    {
        protected override void Start()
        {
            base.Start();
            _life = -1;
            _type = BrickType.Unbreakable;
        }
    }
}
