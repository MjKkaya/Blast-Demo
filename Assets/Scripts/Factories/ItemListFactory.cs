using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ItemListFactory <T> where T  : Object, IPoolable
{
    private readonly Dictionary<T, ObjectPool<T>> _objectPoolDictionary = new ();

    public ItemListFactory(T[] prefabArray)
    {
        InitObjectPool(prefabArray);
    }

    public void Dispose()
    {
        foreach (var item in _objectPoolDictionary)
        {
            item.Value.Dispose();
        }
        // _objectPoolDictionary.Dispose();
        BaseGridData test = new BaseGridData();
    }


    #region Object Pool

    private void InitObjectPool(T[] prefabArray)
    {
        int itemCount = prefabArray.Length;
        // Debug.Log($"{this}-InitObjectPool-itemCount:{itemCount}");
        for (int i = 0; i < itemCount; i++)
        {
            RegisterPrefabPool(prefabArray[i]);
        }
    }

    private void RegisterPrefabPool(T objPrefab)
    {
        _objectPoolDictionary.Add(objPrefab, new ObjectPool<T>(CreatePooledData, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 30));
        
        T CreatePooledData()
        {
            // Debug.Log($"{this}-CreatePooledData");
            T itemObj = Object.Instantiate(objPrefab); 
            return itemObj;
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(T itemObj)
        {
            // Debug.Log($"{this}-OnTakeFromPool-itemObj:{itemObj}");
            itemObj.Prepare();
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(T itemObj)
        {
            // Debug.Log($"{this}-OnReturnedToPool-itemObj:{itemObj}");
            itemObj.Clear();
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(T itemObj)
        {
            // Debug.Log($"{this}-OnDestroyPoolObject-itemObj:{itemObj}");
            itemObj.Dispose();
        }
    }
    

    public T GetItem(T objType)
    {
        // Debug.Log($"{this}-GetItemInstance-CountAll:{_objectPoolDictionary.CountAll}");
        if (!_objectPoolDictionary.ContainsKey(objType))
        {
            // Debug.LogError($"{this}-GetItemInstance-objType isn't in the _objectPoolDictionary!!!");
            return null;
        }

        T itemObj = _objectPoolDictionary[objType].Get();
        return itemObj;
    }
    
    //Return an object to the pool (reset objects before returning).
    public void ReleaseToPool(T prefabType, T itemObj)
    {
        // Debug.Log($"{this}-ReleaseToPool-prefabType:{prefabType}");
        if (!_objectPoolDictionary.ContainsKey(prefabType))
        {
            // Debug.LogError($"{this}-ReleaseToPool-objType isn't in the _objectPoolDictionary!!!");
            return;
        }
        _objectPoolDictionary[prefabType].Release(itemObj);
    }

    #endregion
}