using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public class BrickIsDestroyed : ArkEvent
    {
        public BrickType BType { get; set; }
        public Vector2 Pos { get; set; }
    }

    public class BrickIsHit : ArkEvent
    {
        public BrickType BType { get; set; }
        public Vector2 Pos { get; set; }
    }

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
        public Vector2 Position;

        protected virtual void Start()
        {

        }

        void Update()
        {

        }

        private void GetHit()
        {
            MessageBus.Publish(new BrickIsHit()
            {
                Pos = Position,
                BType = _type
            });

            _life--;
            if (_life == 0)
            {
                DestroySelf();
            }
        }

        private void DestroySelf()
        {
            MessageBus.Publish(new BrickIsDestroyed()
            {
                Pos = Position,
                BType = _type
            });
            BrickPools.Pools[_type].Kill(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            GetHit();
        }
    }
}
