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

        void Start()
        {
            _life = 1;
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
            Destroy(gameObject);
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            GetHit();
        }
    }
}
