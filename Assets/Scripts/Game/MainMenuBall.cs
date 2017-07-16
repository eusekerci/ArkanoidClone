using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Arkanoid
{
    public class MainMenuBall : MonoBehaviour
    {

        private Rigidbody2D _rigidbody;

        private float _currentSpeed = 7.5f;
        private Random _rand;

        void Start()
        {
            _rand = new Random();
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.velocity = new Vector2((float) _rand.NextDouble() - 1f, (float) _rand.NextDouble() - 1f)
                                      .normalized * _currentSpeed;
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _currentSpeed;
        }
    }
}