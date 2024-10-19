using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ItemFactory <T> : Object where T  : Object, IPoolable
{
    private ObjectPool<T> _gridDotDataPool;
    private T _prefabInstance;

    public ItemFactory(T prefabInstance)
    {
        _prefabInstance = prefabInstance;
        InitObjectPool();
    }

    public void Dispose()
    {
        _gridDotDataPool.Dispose();
        BaseGridData test = new BaseGridData();
    }


    #region Object Pool

    private void InitObjectPool()
    {
        Debug.Log($"{this}-InitObjectPool");
        _gridDotDataPool = new ObjectPool<T>(CreatePooledData, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 30);
    }

    private T CreatePooledData()
    {
        // Debug.Log($"{this}-CreatePooledData");
        // T itemObj = UnityEngine.Instan Instantiate(_prefabInstance);
        T itemObj = Instantiate(_prefabInstance); 
        return itemObj;
    }

    // Called when an item is taken from the pool using Get
    private void OnTakeFromPool(T itemObj)
    {
        // Debug.Log($"{this}-OnTakeFromPool-itemObj:{itemObj}");
        itemObj.Clear();
    }

    // Called when an item is returned to the pool using Release
    private void OnReturnedToPool(T itemObj)
    {
        // Debug.Log($"{this}-OnReturnedToPool-itemObj:{itemObj}");
        itemObj.Clear();
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    private void OnDestroyPoolObject(T itemObj)
    {
        // Debug.Log($"{this}-OnDestroyPoolObject-itemObj:{itemObj}");
        itemObj.Dispose();
    }

    public T GetItemInstance()
    {
        // Debug.Log($"{this}-GetItemInstance-CountAll:{_gridDotDataPool.CountAll}");
        T itemObj = _gridDotDataPool.Get();
        return itemObj;
    }
    

    public void ReleaseItemInstance(T itemObj)
    {
        Debug.Log($"{this}-ReleaseItemInstance-CountAll:{_gridDotDataPool.CountAll}, itemObj:{itemObj}");
        _gridDotDataPool.Release(itemObj);
    }

    #endregion


    public void CreateItemList(List<T> dataList, int dataCount)
    {
        Debug.Log($"{this}-CreateDataList-dataCount{dataCount}");
        for (int k = 0; k < dataCount; k++)
        {
            dataList.Add(GetItemInstance());
        }
    }
}
