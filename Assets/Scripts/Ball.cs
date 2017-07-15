using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public class Ball : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _renderer;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();

            _rigidbody.velocity = new Vector2(3, 5);
        }

        void FixedUpdate()
        {
            //TODO one axis of the velocity is near zero
            if (Mathf.Abs(_rigidbody.velocity.y) <= 0.1f)
            {
                _rigidbody.AddForce(new Vector2(0f, 0.1f));
            }
            else if (Mathf.Abs(_rigidbody.velocity.x) <= 0.1f)
            {
                _rigidbody.AddForce(new Vector2(0.1f, 0f));
            }
        }
    }
}