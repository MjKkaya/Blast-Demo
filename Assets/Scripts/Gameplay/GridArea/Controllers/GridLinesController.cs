using System.Collections.Generic;
using UnityEngine;

public class GridLinesController : BaseGridController<GridLineData, GridLineItem>, IResettable
{
    private readonly List<GridLineData> _matchedDataList = new((int)ShapeDirections.Max);
    public List<GridLineData> MatchedDataList => _matchedDataList;
    
    
    public void Initialize(LevelData levelData, Transform itemContainer, GridLineItem gridLineItemPrefab)
    {
        _levelData = levelData;
        _itemContainer = itemContainer;
        _itemCount = (_levelData.CellCountInRow * (_levelData.CellCountInColumn+1)) + (_levelData.CellCountInColumn * (_levelData.CellCountInRow +1));
        _dataFactory = new DataFactory<GridLineData>();
        _itemFactory = new ItemFactory<GridLineItem>(gridLineItemPrefab);
        
        CreateLines();
    }
    
    public void Reset()
    {
        int listCount = _itemList.Count;
        for (int i = 0; i < listCount; i++)
        {
            GridLineItem lineItem = _itemList[i];
            lineItem.SetAsUnoccupied(_levelData.LineDefaultColor);
        }
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

    /// <summary>
    ///  In this method, cells and rows are connected to each other via the data class.
    /// </summary>
    /// <param name="cellDataList"></param>
    public void AssignConnectedCellsAndLines(List<GridCellData> cellDataList)
    {
        int cellCount = cellDataList.Count;
        for (int i = 0; i < cellCount; i++)
        {
            //Actually there are only max 2 connected cell for each line.
            //  "Left" or  "Right" or "Left and Right"  
            //  "Up" or  "Down" or "Up and Down"
            GridCellData cellData = cellDataList[i];
            
            //The line down-side the cell.
            int lineIndexNo = cellData.LocationIndexNo + (cellData.GridPosition.Y * _levelData.CellCountInRow) + cellData.GridPosition.Y;
            _dataList[lineIndexNo].AddConnectedCell(cellData);
            cellData.AddConnectedLine((int)ShapeDirections.Down, _dataList[lineIndexNo]);
            
            //The line left-side of the cell.
            lineIndexNo += _levelData.CellCountInRow;
            _dataList[lineIndexNo].AddConnectedCell(cellData);
            cellData.AddConnectedLine((int)ShapeDirections.Left, _dataList[lineIndexNo]);
            
            //The line right-side of the cell.
            lineIndexNo++;
            _dataList[lineIndexNo].AddConnectedCell(cellData);
            cellData.AddConnectedLine((int)ShapeDirections.Right, _dataList[lineIndexNo]);
            
            //The line up-side of the cell.
            lineIndexNo += _levelData.CellCountInRow;
            _dataList[lineIndexNo].AddConnectedCell(cellData);
            cellData.AddConnectedLine((int)ShapeDirections.Up, _dataList[lineIndexNo]);
        }
    }

    public bool CheckAndSetMatchedLines(ShapeDirections[] shapeDirections, int startingDotLocationIndex, List<GridDotData> dotDataList)
    {
        int directionLength = shapeDirections.Length;
        ResetHighlightedLines();
        UpdateMatchedLineList(shapeDirections, startingDotLocationIndex, dotDataList);

        if (directionLength == _matchedDataList.Count)
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
        int dotCount = dotDataList.Count;
        for (int i = 0; i < directionLength; i++)
        {
            GridDotData dotData = dotDataList[dotLocationIndex];
            GridLineData gridLineData = dotData.GetConnectedGridLineData(shapeDirections[i]);
            if (gridLineData == null || gridLineData.IsOccupied)
                break;
             
            _matchedDataList.Add(gridLineData);
            dotLocationIndex = GridDotsController.GetConnectedDotLocationIndex(_levelData.CellCountInRow, dotLocationIndex, shapeDirections[i]);
            if (dotLocationIndex < 0 || dotLocationIndex > dotCount)
                break;
        }
    }
    
    
    
    public void CheckAndMarkHighlightedLines(ShapeDirections[] shapeDirections, int startingDotLocationIndex, List<GridDotData> dotDataList)
    {
        ResetHighlightedLines();
        UpdateMatchedLineList(shapeDirections, startingDotLocationIndex, dotDataList);
        
        int directionLength = shapeDirections.Length;
        // Debug.Log($"CheckAndMarkHighlightedLines-shapeDirections/_matchedDataList:{directionLength}/{_matchedDataList.Count}");
        if (directionLength != _matchedDataList.Count)
            return;

        SetLinesAsHighlighted();
    }
    
    public void ResetHighlightedLines()
    {
        int count = _matchedDataList.Count;
        for (int i = 0; i < count; i++)
        {
            GridLineData data = _matchedDataList[i];
            if(!data.IsOccupied)
                _itemList[data.LocationIndexNo].BackToDefaultView(_levelData.LineDefaultColor);
        }
        _matchedDataList.Clear();
    }

    private void SetLinesAsHighlighted()
    {
        int count = _matchedDataList.Count;
        for (int i = 0; i < count; i++)
        {
            GridLineData data = _matchedDataList[i];
            _itemList[data.LocationIndexNo].SetHighlightedView(_levelData.LineHighlightedColor);
        }
    }
    private void SetLinesAsOccupied()
    {
        int count = _matchedDataList.Count;
        for (int i = 0; i < count; i++)
        {
            GridLineData data = _matchedDataList[i];
            _itemList[data.LocationIndexNo].SetAsOccupied(_levelData.LineFullColor);
            // Debug.Log($"SetLinesAsOccupied:{data.LocationIndexNo}=>{data.PrintConnectedCell()}");
        }
    }
    public void SetLinesAsUnOccupied(GridLineData[] dataList)
    {
        int count = dataList.Length;
        for (int i = 0; i < count; i++)
        {
            GridLineData data = dataList[i];
            _itemList[data.LocationIndexNo].SetAsUnoccupied(_levelData.LineDefaultColor);
        }
    }

    private Vector3 CalculateItemPosition(int rowIndexNo, int columnIndexNo, float gap)
    {
        float posX = _levelData.DotStartPos.x + (columnIndexNo * gap);
        float posY = _levelData.DotStartPos.y + (rowIndexNo * gap);
        return new Vector3(posX, posY);
    }
}
