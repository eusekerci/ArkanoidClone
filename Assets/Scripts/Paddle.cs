using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public class Paddle : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            float positionX = 0;
#if UNITY_ANDROID || UNITY_IPHONE
	    if (Input.touchCount > 0)
	    {
	        positionX = Input.touches[0].position.x;
	    }
	    else
	    {
	        positionX = transform.position.x;
	    }
#else
            positionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
#endif

            transform.position = new Vector3(positionX, transform.position.y, transform.position.z);
        }
    }
}