using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public class Paddle : MonoBehaviour
    {
        #region Singleton

        private static Paddle instance;

        public static Paddle Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Paddle();
                }
                return instance;
            }
        }
        #endregion

        public Transform Transform { get { return transform; } }

        public Vector3 Position { get { return transform.position; } }

        void Awake()
        {
            instance = this;
        }

        void Update()
        {
            //TODO GameManager.GameIsPaused?
            if (Time.timeScale < 0.01)
                return;

            float positionX = 0;
#if UNITY_ANDROID || UNITY_IPHONE
	    if (Input.touchCount > 0)
	    {
	        positionX = Camera.main.ScreenToWorldPoint(Input.touches[0].position).x;
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