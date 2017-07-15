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
    }

    public class Brick : MonoBehaviour
    {
        private int _life;
        private BrickType _type;

        void Start()
        {
            _life = 1;
            _type = BrickType.Basic;
        }

        void Update()
        {

        }

        public void GetHit()
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

        void OnCollisionEnter2D(Collision2D col)
        {
            GetHit();
        }
    }
}
