using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    [Flags]
    public enum BrickType
    {
        Empty = 0,
        Basic = 1,
        Unbreakable = 2,
    }

    public class Brick : MonoBehaviour
    {
        protected int _life;
        protected BrickType _type;

        protected virtual void Start()
        {

        }

        void Update()
        {

        }

        private void GetHit()
        {
            _life--;
            if (_life == 0)
            {
                DestroySelf();
            }
        }

        private void DestroySelf()
        {
            BrickPools.Pools[_type].Kill(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            GetHit();
        }
    }
}
