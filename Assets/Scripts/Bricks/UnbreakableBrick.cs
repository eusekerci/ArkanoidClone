using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public class UnbreakableBrick : Brick
    {
        private Animator _anim;

        protected override void Start()
        {
            base.Start();
            _life = -1;
            _type = BrickType.Unbreakable;
            _currentLife = _life;
            _anim = GetComponent<Animator>();
        }

        protected override void GetHit()
        {
            base.GetHit();
            _anim.SetTrigger("shine");
        }
    }
}
