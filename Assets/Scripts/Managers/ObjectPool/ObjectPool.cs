using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class ObjectPool : IObjectPool
    {
        List<IPoolableObject> objectPoolList;
        GameObject prefab;

        public ObjectPool(int numberOfObjects, GameObject prefab)
        {
            this.prefab = prefab;
            objectPoolList = new List<IPoolableObject>(numberOfObjects);
            for (int i = 0; i < numberOfObjects; i++)
            {
                objectPoolList.Add(GameObject.Instantiate(prefab).GetComponent<IPoolableObject>());
                objectPoolList[i].IsDirty = false;
            }
        }

        public IPoolableObject Get()
        {
            foreach (var bullet in objectPoolList)
            {
                if (!bullet.IsDirty)
                {
                    bullet.IsDirty = true;
                    return bullet;
                }
            }

            return null;
        }

        public void Release(IPoolableObject obj)
        {
            obj.IsDirty = false;
        }
    }
}
