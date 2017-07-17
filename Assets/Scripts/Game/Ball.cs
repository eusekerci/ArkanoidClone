using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = System.Random;

namespace Arkanoid
{
    public class Ball : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _renderer;
        private TrailRenderer _trailRenderer;

        private float _optimalSpeed = 5f;
        private float _maxSpeed = 10f;
        private float _speedBuffTimer;
        private float _speedBuffAfterSecs = 7.5f;

        private float _currentSpeed = 5f;
        private float _paddleBounceCoff = 250f;

        private bool _followPaddle;
        private Color _myColor;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _trailRenderer = transform.GetComponentInChildren<TrailRenderer>();

            _followPaddle = true;
            _trailRenderer.enabled = false;
            _myColor = _renderer.color;

            MessageBus.OnEvent<GameIsStarted>().Subscribe(evnt =>
            {
                _followPaddle = false;
                _trailRenderer.enabled = true;
                _rigidbody.velocity = new Vector2(0, 1).normalized * _currentSpeed;
                _speedBuffTimer = 0f;
            });

            MessageBus.OnEvent<LevelIsCompleted>().Subscribe(evnt =>
            {
                _rigidbody.velocity = new Vector2(0,0);
                _followPaddle = true;
                _trailRenderer.enabled = false;
                _speedBuffTimer = 0f;
            });

            MessageBus.OnEvent<BrickIsHit>().Subscribe(evnt =>
            {
                iTween.ColorFrom(gameObject, Color.red, 0.3f);
                iTween.ColorTo(gameObject, iTween.Hash("r", _myColor.r, "g", _myColor.g, "b", _myColor.b, "delay", 0.33f));

                iTween.ScaleFrom(gameObject, new Vector3(1.5f, 1.5f, 1.5f), 0.3f);
                iTween.ScaleTo(gameObject, iTween.Hash("x", 1f, "y", 1f, "z", 1f, "delay", 0.33f));
            });
        }

        void FixedUpdate()
        {
            if (_followPaddle)
            {
                transform.position = new Vector3(Paddle.Instance.Position.x, -2.5f, 0f);
            }

            if (_rigidbody.velocity.sqrMagnitude > _currentSpeed)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * _currentSpeed;
            }

            //TODO one axis of the velocity is near zero
            if (Mathf.Abs(_rigidbody.velocity.y) <= 0.1f)
            {
                _rigidbody.AddForce(new Vector2(0f, 0.75f * Mathf.Sign(_rigidbody.velocity.y)));
            }
            else if (Mathf.Abs(_rigidbody.velocity.x) <= 0.1f)
            {
                _rigidbody.AddForce(new Vector2(0.75f * Mathf.Sign(_rigidbody.velocity.x), 0f));
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
            else if (col.gameObject.tag == "DeadZone")
            {
                _rigidbody.velocity = new Vector2(0, 0);
                _followPaddle = true;
                _trailRenderer.enabled = false;
                _speedBuffTimer = 0f;
                GameManager.Instance.BallIsDead();
            }
        }

        private void ResetSpeed()
        {
            _currentSpeed = _optimalSpeed;
            _speedBuffTimer = 0f;
        }
    }
}