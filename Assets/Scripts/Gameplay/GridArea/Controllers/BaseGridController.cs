using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseGridController<D,I> : IDisposable where D : BaseGridData, new() where I : BaseGridItem
{
    protected LevelData _levelData;
    protected Transform _itemContainer;

    protected DataFactory<D> _dataFactory;
    protected ItemFactory<I> _itemFactory;
    
    protected List<D> _dataList = new ();
    public List<D> DataList => _dataList;
    
    protected List<I> _itemList = new ();
    public List<I> ItemList => _itemList;

    protected int _itemCount;
    
    
    public void Dispose()
    {
        _dataFactory.Dispose();
        _itemFactory.Dispose();
        _dataList = null;
        _itemList = null;
    }
}

public interface IResettable
{
    void Reset();
}