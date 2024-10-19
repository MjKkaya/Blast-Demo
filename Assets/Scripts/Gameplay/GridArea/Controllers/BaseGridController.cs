using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseGridController<Data,Item> : IDisposable where Data : BaseGridData, new() where Item : BaseGridItem
{
    protected LevelData _levelData;
    protected Transform _itemContainer;

    protected DataFactory<Data> _dataFactory;
    protected ItemFactory<Item> _itemFactory;
    
    protected List<Data> _dataList = new ();
    public List<Data> DataList => _dataList;
    
    protected List<Item> _itemList = new ();
    public List<Item> ItemList => _itemList;

    protected int _itemCount;
    
    
    public void Dispose()
    {
        _dataFactory.Dispose();
        _itemFactory.Dispose();
        _dataList = null;
        _itemList = null;
    }
}