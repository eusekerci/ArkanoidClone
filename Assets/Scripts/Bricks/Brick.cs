using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
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
        protected int _currentLife;
        private BoxCollider2D _collider;
        private AudioSource _audio;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _audio = GetComponent<AudioSource>();
        }

        protected virtual void Start()
        {

        }

        void Update()
        {

        }

        protected virtual void GetHit()
        {
            MessageBus.Publish(new BrickIsHit()
            {
                Pos = Position,
                BType = _type
            });

            _currentLife--;
            _audio.Play();
            if (_currentLife == 0)
            {
                DestroySelf();
            }
        }

        private void DestroySelf()
        {
            _collider.enabled = false;

            iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("y", 0.1f, "time", 0.3f));
            iTween.ScaleTo(gameObject, new Vector3(0,0,0), 1);
            iTween.RotateTo(gameObject, new Vector3(0,0,180), 1);

            IDisposable d = null;
            d = Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(x =>
            {
                MessageBus.Publish(new BrickIsDestroyed()
                {
                    Pos = Position,
                    BType = _type
                });
                BrickPools.Pools[_type].Kill(gameObject);
            });
        }

        public void Clear()
        {
            BrickPools.Pools[_type].Kill(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            GetHit();
        }

        private void OnEnable()
        {
            transform.localScale = new Vector3(1,1,1);
            _currentLife = _life;
            _collider.enabled = true;
        }
    }
}
