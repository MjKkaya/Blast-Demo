using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class DataFactory <T> where T : class, IPoolable, new() 
{
    private ObjectPool<T> _gridDotDataPool;


    public DataFactory()
    {
        InitObjectPool();
    }

    public void Dispose()
    {
        _gridDotDataPool.Dispose();
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
        T dataObj = new T ();
        return dataObj;
    }

    // Called when an item is taken from the pool using Get
    private void OnTakeFromPool(T dataObj)
    {
        // Debug.Log($"{this}-OnTakeFromPool-dataObj:{dataObj}");
        dataObj.Clear();
    }

    // Called when an item is returned to the pool using Release
    private void OnReturnedToPool(T dataObj)
    {
        // Debug.Log($"{this}-OnReturnedToPool-obj:{dataObj}");
        dataObj.Clear();
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    private void OnDestroyPoolObject(T dataObj)
    {
        Debug.Log($"{this}-OnDestroyPoolObject-obj:{dataObj}");
        dataObj.Dispose();
    }

    public T GetDataInstance()
    {
        // Debug.Log($"{this}-GetDataInstance-CountAll:{_gridDotDataPool.CountAll}");
        T dataObj = _gridDotDataPool.Get();
        return dataObj;
    }
    

    public void ReleaseDataInstance(T dataObj)
    {
        Debug.Log($"{this}-ReleaseDataInstance-CountAll:{_gridDotDataPool.CountAll}, dataObj:{dataObj}");
        _gridDotDataPool.Release(dataObj);
    }

    #endregion


    public void CreateDataList(List<T> dataList, int dataCount)
    {
        Debug.Log($"{this}-CreateDataList-dataCount{dataCount}");
        for (int k = 0; k < dataCount; k++)
        {
            dataList.Add(GetDataInstance());
        }
    }
}