using System.Collections.Generic;
using UnityEngine;

public class GridLinesController : BaseGridController<GridLineData, GridLineItem>
{
    private readonly List<GridLineData> _matchedLineList = new((int)ShapeDirections.Max);
    
    
    public void Initialize(LevelData levelData, Transform itemContainer, GridLineItem gridLineItemPrefab)
    {
        _levelData = levelData;
        _itemContainer = itemContainer;
        _itemCount = (_levelData.CellCountInRow * (_levelData.CellCountInColumn+1)) + (_levelData.CellCountInColumn * (_levelData.CellCountInRow +1));
        _dataFactory = new DataFactory<GridLineData>();
        _itemFactory = new ItemFactory<GridLineItem>(gridLineItemPrefab);
        
        CreateLines();
    }
    
    private void CreateLines()
    {
        _dataList.Clear();
        _dataFactory.CreateDataList(_dataList, _itemCount);
        
        _itemList.Clear();
        _itemFactory.CreateItemList(_itemList, _itemCount);

        /*Line items are placed according to this order.
         * r = 2 =>  _ _ _ _ _ _ 
         * r = 1 => | | | | | | |
         * r = 0 =>  _ _ _ _ _ _
         ... until r = 15
         */
        int cellCountInColumn = _levelData.CellCountInColumn + (_levelData.CellCountInColumn + 1);
        int indexNo = 0;
        int realRowPositionIndex = 0; 
        for (int r = 0; r < cellCountInColumn; r++)
        {
            bool isEvenRow = r % 2 == 0;
            if (isEvenRow && r != 0)
                realRowPositionIndex++;
            int cellCountInRow = isEvenRow ? _levelData.CellCountInRow : _levelData.CellCountInRow + 1;
            for (int c = 0; c < cellCountInRow; c++)
            {
                GridLineData lineData = _dataList[indexNo];
                lineData.Init(indexNo);
                
                GridLineItem lineItem =_itemList[indexNo];
                
                
                Vector3 position = CalculateItemPosition(realRowPositionIndex, c, _levelData.GapBetweenCell);
                Quaternion rotation = Quaternion.Euler(0, 0, isEvenRow ? -90 : 0);
                lineItem.Init(lineData, _itemContainer, position, rotation);
                indexNo++;
            }
        }
    }


    public bool CheckAndSetMatchedLines(ShapeDirections[] shapeDirections, int startingDotLocationIndex, List<GridDotData> dotDataList)
    {
        int directionLength = shapeDirections.Length;
        ResetHighlightedLines();
        UpdateMatchedLineList(shapeDirections, startingDotLocationIndex, dotDataList);

        if (directionLength == _matchedLineList.Count)
        {
            SetLinesAsOccupied();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateMatchedLineList(ShapeDirections[] shapeDirections, int startingDotLocationIndex, List<GridDotData> dotDataList)
    {
        int directionLength = shapeDirections.Length;
        int dotLocationIndex = startingDotLocationIndex;
        for (int i = 0; i < directionLength; i++)
        {
            GridDotData dotData = dotDataList[dotLocationIndex];
            GridLineData gridLineData = dotData.GetConnectedGridLineData(shapeDirections[i]);
            if (gridLineData == null || gridLineData.IsOccupied)
                break;
             
            _matchedLineList.Add(gridLineData);
            dotLocationIndex = GetConnectedDotLocationIndex(dotLocationIndex, shapeDirections[i]);
        }
    }
    
    private int GetConnectedDotLocationIndex(int dotIndexNo, ShapeDirections shapeDirection)
    {
        int nextDotIndexNo = dotIndexNo;

        switch (shapeDirection)
        {
            case ShapeDirections.Left:
                nextDotIndexNo--;
                break;
            case ShapeDirections.Right:
                nextDotIndexNo++;
                break;
            case ShapeDirections.Up:
                nextDotIndexNo += _levelData.CellCountInRow + 1;
                break;
            case ShapeDirections.Down:
                nextDotIndexNo -= _levelData.CellCountInRow + 1;
                break;
        }
        
        return nextDotIndexNo;
    }
    
    public void CheckAndMarkHighlightedLines(ShapeDirections[] shapeDirections, int startingDotLocationIndex, List<GridDotData> dotDataList)
    {
        // ResetHighlightedLines();
        UpdateMatchedLineList(shapeDirections, startingDotLocationIndex, dotDataList);
        
        int directionLength = shapeDirections.Length;
        if (directionLength != _matchedLineList.Count)
            return;

        SetLinesAsHighlighted();
    }
    
    public void ResetHighlightedLines()
    {
        int count = _matchedLineList.Count;
        // Debug.Log($"ResetHighlightedLines:{count}");
        for (int i = 0; i < count; i++)
        {
            GridLineData data = _matchedLineList[i];
            _itemList[data.LocationIndexNo].BackToDefaultView();
        }
        _matchedLineList.Clear();
    }

    private void SetLinesAsHighlighted()
    {
        int count = _matchedLineList.Count;
        for (int i = 0; i < count; i++)
        {
            GridLineData data = _matchedLineList[i];
            _itemList[data.LocationIndexNo].SetHighlightedView();
        }
    }
    private void SetLinesAsOccupied()
    {
        int count = _matchedLineList.Count;
        for (int i = 0; i < count; i++)
        {
            GridLineData data = _matchedLineList[i];
            _itemList[data.LocationIndexNo].SetOccupied();
        }
        _matchedLineList.Clear();
    }

    private Vector3 CalculateItemPosition(int rowIndexNo, int columnIndexNo, float gap)
    {
        float posX = _levelData.DotStartPos.x + (columnIndexNo * gap);
        float posY = _levelData.DotStartPos.y + (rowIndexNo * gap);
        return new Vector3(posX, posY);
    }
}
