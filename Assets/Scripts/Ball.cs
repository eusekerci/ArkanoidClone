using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;

	void Start ()
	{
	    _rigidbody = GetComponent<Rigidbody2D>();
	    _renderer = GetComponent<SpriteRenderer>();

        _rigidbody.velocity = new Vector2(3, 5);
	}
	
	void Update ()
    {
		
	}
}
