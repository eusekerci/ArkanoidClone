using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace Arkanoid
{
    public class BrickPool
    {
        private readonly Queue<GameObject> _brickPool;
        private readonly Transform _poolParent;
        public BrickType PoolType;

        public BrickPool(GameObject prefab, BrickType brickType, int poolSize)
        {
            _brickPool = new Queue<GameObject>();
            _poolParent = new GameObject(prefab.name + "Pool").transform;
            PoolType = brickType;

            Object.DontDestroyOnLoad(_poolParent.gameObject);

            for (int i = 0; i < poolSize; i++)
            {
                GameObject brickGo = Object.Instantiate(prefab, _poolParent);
                brickGo.SetActive(false);
                _brickPool.Enqueue(brickGo);
            }
        }

        public GameObject Get()
        {
            if (_brickPool.Count <= 0)
            {
                //TODO we must handle this situation
                Debug.LogError("Pool size is exceeded");
                return null;
            }

            GameObject returnGo = _brickPool.Dequeue();
            returnGo.SetActive(true);
            return returnGo;
        }

        public GameObject[] GetMany(int count)
        {
            if (count < _brickPool.Count)
            {
                //TODO we must handle this situation
                Debug.LogError("Pool size is exceeded");
                return null;
            }

            GameObject[] returnGos = new GameObject[count];
            for (int i = 0; i < count; i++)
            {
                GameObject returnGo = _brickPool.Dequeue();
                returnGo.SetActive(true);
                returnGos[i] = returnGo;
            }

            return returnGos;
        }

        public void Kill(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(_poolParent);
            _brickPool.Enqueue(obj);
        }

        public void KillMany(GameObject[] objs)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].SetActive(false);
                objs[i].transform.SetParent(_poolParent);
                _brickPool.Enqueue(objs[i]);
            }
        }
    }

    public static class BrickPools
    {
        public static Dictionary<BrickType, BrickPool> Pools { get; private set; }

        public static void InitiliazeBrickPools()
        {
            Pools = new Dictionary<BrickType, BrickPool>();
            Pools.Add(BrickType.Basic, new BrickPool(Resources.Load<GameObject>("Prefabs/BasicBrick"), BrickType.Basic, 150));
        }
    }
}
