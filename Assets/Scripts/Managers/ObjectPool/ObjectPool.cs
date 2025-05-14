using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class ObjectPool : IObjectPool
    {
        //List and prototype 
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

        //Return a clean clone of the prefab if is possible
        public IPoolableObject Get()
        {
            foreach (var poolableObject in objectPoolList)
            {
                if (!poolableObject.IsDirty)
                {
                    poolableObject.IsDirty = true;
                    return poolableObject;
                }
            }

            return null;
        }

        //Retstar the state of a used clone
        public void Release(IPoolableObject obj)
        {
            obj.IsDirty = false;
        }
    }
}
