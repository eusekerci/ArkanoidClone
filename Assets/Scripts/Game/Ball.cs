using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Arkanoid
{
    public class Ball : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _renderer;

        private float _optimalSpeed = 5f;
        private float _maxSpeed = 10f;
        private float _speedBuffTimer;
        private float _speedBuffAfterSecs = 7.5f;

        private float _currentSpeed = 5f;
        private float _paddleBounceCoff = 250f;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();

            _rigidbody.velocity = new Vector2(0, 5).normalized * _currentSpeed;
            _speedBuffTimer = 0f;
        }

        void FixedUpdate()
        {
            if (_rigidbody.velocity.sqrMagnitude > _currentSpeed)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * _currentSpeed;
            }

            //TODO one axis of the velocity is near zero
            if (Mathf.Abs(_rigidbody.velocity.y) <= 0.05f)
            {
                _rigidbody.AddForce(new Vector2(0f, 0.5f * Mathf.Sign(_rigidbody.velocity.y)));
            }
            else if (Mathf.Abs(_rigidbody.velocity.x) <= 0.05f)
            {
                _rigidbody.AddForce(new Vector2(0.5f * Mathf.Sign(_rigidbody.velocity.x), 0f));
            }

            //If ball didn't hit paddle or any brick for a while, get speed buff
            _speedBuffTimer += Time.fixedDeltaTime;
            if (_speedBuffTimer > _speedBuffAfterSecs)
            {
                _currentSpeed = _maxSpeed;
                _rigidbody.velocity = _rigidbody.velocity.normalized * _currentSpeed;
            }
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.tag == "Paddle")
            {
                _rigidbody.AddForce((transform.position- col.transform.position).normalized * _paddleBounceCoff);
                _currentSpeed = _optimalSpeed;
                ResetSpeed();
            }
            else if (col.gameObject.tag == "Brick")
            {
                ResetSpeed();
            }
        }

        private void ResetSpeed()
        {
            _currentSpeed = _optimalSpeed;
            _speedBuffTimer = 0f;
        }
    }
}