using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public class LevelLoader : MonoBehaviour
    {
        private string[] _levelMap;
        private int rowCount = 15;
        private int columnCount = 10;

        private Transform _brickRoot;
        private float firstRowY = 4.2f;
        private float firstColumnX = -2.25f;
        private float rowOffset = -0.3f;
        private float columnOffset = 0.5f;

        public Transform BasicBrick;

        void Start()
        {
            _brickRoot = GameObject.Find("BrickRoot").transform;

            _levelMap = new string[]
            {
                "0", "1", "0", "1", "0", "0", "0", "0", "0", "0",
                "0", "0", "1", "0", "0", "0", "0", "0", "0", "1",
                "0", "0", "0", "1", "0", "1", "1", "0", "0", "0",
                "0", "0", "1", "1", "1", "0", "0", "1", "1", "1",
                "0", "0", "0", "0", "0", "1", "0", "1", "0", "0",
                "0", "0", "0", "1", "0", "0", "1", "0", "0", "0",
                "0", "0", "1", "0", "1", "0", "1", "1", "0", "1",
                "0", "1", "0", "0", "0", "1", "0", "0", "1", "0",
                "0", "0", "1", "0", "1", "0", "1", "0", "1", "1",
                "0", "0", "0", "1", "0", "0", "0", "1", "0", "0",
                "0", "0", "1", "0", "1", "0", "1", "0", "1", "0",
                "0", "0", "0", "0", "0", "1", "0", "0", "0", "0",
                "1", "0", "1", "0", "1", "0", "1", "1", "1", "0",
                "0", "0", "0", "0", "0", "0", "0", "1", "0", "0",
                "0", "0", "1", "0", "0", "0", "0", "0", "1", "0"
            };

            InitiliazeLevel();
        }

        void Update()
        {

        }

        public void InitiliazeLevel()
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (_levelMap[i * columnCount + j] == "1")
                    {
                        Instantiate(BasicBrick, new Vector3(firstColumnX + j * columnOffset, firstRowY + i * rowOffset, 0), Quaternion.identity, _brickRoot);
                    }
                }
            }
        }
    }
}
