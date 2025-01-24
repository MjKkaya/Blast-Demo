using System.Collections.Generic;
using UnityEngine;

public class GridCellsController: BaseGridController<GridCellData, GridCellItem>, IResettable
{
    private List<int> _filledRowIndexList;
    private List<int> _filledColumnIndexList;

    private List<GridCellData> _checkedCellList;
    private List<GridCellData> _filledCellList;
    
    
    public void Initialize(LevelData levelData, Transform gridCellItemContainer, GridCellItem gridCellItemPrefab)
    {
        _levelData = levelData;
        _itemContainer = gridCellItemContainer;
        _itemCount = _levelData.CellCountInColumn * _levelData.CellCountInRow;
        _dataFactory = new DataFactory<GridCellData>();
        _itemFactory = new ItemFactory<GridCellItem>(gridCellItemPrefab);
        
        _filledRowIndexList = new(_levelData.CellCountInRow); 
        _filledColumnIndexList = new(_levelData.CellCountInRow); 
        
        int listCount = _levelData.CellCountInColumn * _levelData.CellCountInRow;
        _checkedCellList = new (listCount);
        _filledCellList = new (listCount);
        
        CreateCells();
    }
    
    public void Reset()
    {
        int listCount = _itemList.Count;
        for (int i = 0; i < listCount; i++)
        {
            GridCellItem cellItem = _itemList[i];
            cellItem.SetAsUnoccupied(_levelData.CellDefaultColor);
        }
    }

    public List<GridCellData> CheckAndSetFilledCells(List<GridLineData> lineDatas)
    {
        int lineCount = lineDatas.Count;
        _checkedCellList.Clear();
        _filledCellList.Clear();
        
        // Debug.Log($"Mj-CheckAndSetFilledCells-lineCount:{lineCount}");
        for (int l = 0; l < lineCount; l++)
        {
            List<GridCellData> cellDataList = lineDatas[l].ConnectedCellList;
            for (int c = 0; c < cellDataList.Count; c++)
            {
                GridCellData gridCellData = cellDataList[c];
                // Debug.Log($"Mj-CheckAndSetFilledCells:{gridCellData.LocationIndexNo}");
                if(_checkedCellList.Contains(gridCellData))
                    continue;
                
                _checkedCellList.Add(gridCellData);//to prevent to check again same cell!
                bool isCellFull = gridCellData.CheckSetCellState();
                if (!isCellFull)
                    continue;
                
                _itemList[gridCellData.LocationIndexNo].SetAsOccupied(_levelData.CellFullColor);
                _filledCellList.Add(gridCellData);
            }
        }

        return _filledCellList;
    }

    
    public void CheckAndDestroyFilledRowAndColumn(List<GridCellData> filledCellList)
    {
        _filledRowIndexList.Clear();
        _filledColumnIndexList.Clear();
        
        int cellCount = filledCellList.Count;
        for (int i = 0; i < cellCount; i++)
        {
            GridCellData gridCellData = filledCellList[i];
            if(IsRowOccupied(gridCellData.GridPosition.Y))
            {
                // Debug.Log($"Mj-RowOccupied:{gridCellData.GridPosition.Y}");
                _filledRowIndexList.Add(gridCellData.GridPosition.Y);
            }
            if (IsColumnOccupied(gridCellData.GridPosition.X))
            {
                // Debug.Log($"Mj-ColumnOccupied:{gridCellData.GridPosition.Y}");
                _filledColumnIndexList.Add(gridCellData.GridPosition.X);
            }
        }

        if (_filledRowIndexList.Count > 0)
        {
            int rowCount = _filledRowIndexList.Count; 
            for (int i = 0; i < rowCount; i++)
            {
                // Debug.Log($"Mj-SetUnoccupiedCellsInRow:{filledRowIndexList[i]}");
                SetUnoccupiedCellsInRow(_filledRowIndexList[i]);
                GameplayEvents.OnFilledWholeLine?.Invoke(_levelData.CellCountInRow);
            }
        }
        
        if (_filledColumnIndexList.Count > 0)
        {
            int columnCount = _filledColumnIndexList.Count; 
            for (int i = 0; i < columnCount; i++)
            {
                // Debug.Log($"Mj-SetUnoccupiedCellsInColumn{filledColumnIndexList[i]}");
                SetUnoccupiedCellsInColumn(_filledColumnIndexList[i]);
                GameplayEvents.OnFilledWholeLine?.Invoke(_levelData.CellCountInColumn);
            }
        }
    }
    private bool IsRowOccupied(int rowIndexNo)
    {
        int indexNo = rowIndexNo * _levelData.CellCountInRow;
        // Debug.Log($"Mj-IsRowOccupied-rowIndexNo:{rowIndexNo}, indexNo:{indexNo}");
        for (int i = 0; i <  _levelData.CellCountInRow; i++)
        {
            // Debug.Log($"Mj-IsRowOccupied-i:{i}, indexNo:{indexNo}, IsOccupied:{_dataList[indexNo].IsOccupied}");
            if(!_dataList[indexNo].IsOccupied)
                return false;
            indexNo++;
        }
        return true;
    }
    private bool IsColumnOccupied(int columnIndexNo)
    {
        int indexNo = columnIndexNo;
        // Debug.Log($"Mj-IsColumnOccupied-firstIndexInRow:{indexNo}, _levelData.CellCountInRow:{_levelData.CellCountInColumn}");
        for (int i = 0; i < _levelData.CellCountInColumn; i++)
        {
            // Debug.Log($"Mj-IsColumnOccupied-i:{i}, indexNo:{indexNo}, IsOccupied:{_dataList[indexNo].IsOccupied}");
            if(!_dataList[indexNo].IsOccupied)
                return false;
            indexNo += _levelData.CellCountInRow;
        }
        return true;
    }

    private void SetUnoccupiedCellsInRow(int rowIndexNo)
    {
        int indexNo = rowIndexNo * _levelData.CellCountInRow;
        for (int i = 0; i < _levelData.CellCountInRow; i++)
        {
            // Debug.Log($"Mj-SetUnoccupiedCellsInRow-indexNo:{indexNo}");
            _itemList[indexNo].SetAsUnoccupied(_levelData.CellDefaultColor);
            SetUnoccupiedRelatedCell(indexNo);
            GameplayEvents.CellMadeUnoccupied?.Invoke(indexNo);
            indexNo++;
        }
    }
    private void SetUnoccupiedCellsInColumn(int columnIndexNo)
    {
        int indexNo = columnIndexNo;
        // Debug.Log($"Mj-SetUnoccupiedCellsInColumn-indexNo:{indexNo}");
        for (int i = 0; i < _levelData.CellCountInColumn; i++)
        {
            _itemList[indexNo].SetAsUnoccupied(_levelData.CellDefaultColor);
            SetUnoccupiedRelatedCell(indexNo);
            GameplayEvents.CellMadeUnoccupied?.Invoke(indexNo);
            indexNo += _levelData.CellCountInRow;
        }
    }
    private void SetUnoccupiedRelatedCell(int cellIndexNo)
    {
        GridLineData[] lineDataList = _dataList[cellIndexNo].ConnectedLines;
        for (int i = 0; i < lineDataList.Length; i++)
        {
            GridLineData gridLineData = lineDataList[i];
            foreach (GridCellData cellData in gridLineData.ConnectedCellList)
            {
                _itemList[cellData.LocationIndexNo].SetAsUnoccupied(_levelData.CellDefaultColor);
            }
        }
    }
    

    private void CreateCells()
    {
        _dataList.Clear();
        _dataFactory.CreateDataList(_dataList, _itemCount);
        
        _itemList.Clear();
        _itemFactory.CreateItemList(_itemList, _itemCount);
        
        int rowCount = _levelData.CellCountInColumn;
        int columnCount = _levelData.CellCountInRow;
        int indexNo = 0;
        
        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < columnCount; c++)
            {
                GridCellData gridCellData = _dataList[indexNo];
                gridCellData.Init(indexNo, new GridPosition(c,r));
                
                GridCellItem gridCellItem = _itemList[indexNo];
                Vector3 position = CalculateItemPosition(r, c, _levelData.GapBetweenCell);
                gridCellItem.Init(_dataList[indexNo], _itemContainer, position);
                
                indexNo++;
            }
        }
    }
    
    private Vector3 CalculateItemPosition(int rowIndexNo, int columnIndexNo, float gap)
    {
        float posX = _levelData.CellStartPos.x + (columnIndexNo * gap);
        float posY = _levelData.CellStartPos.y + (rowIndexNo * gap);
        return new Vector3(posX, posY);
    }
    
}
