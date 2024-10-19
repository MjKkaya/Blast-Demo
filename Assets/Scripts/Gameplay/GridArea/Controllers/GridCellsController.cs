using System.Collections.Generic;
using UnityEngine;

public class GridCellsController: BaseGridController<GridCellData, GridCellItem>
{
    public void Initialize(LevelData levelData, Transform gridCellItemContainer, GridCellItem gridCellItemPrefab)
    {
        _levelData = levelData;
        _itemContainer = gridCellItemContainer;
        _itemCount = _levelData.CellCountInColumn * _levelData.CellCountInRow;
        _dataFactory = new DataFactory<GridCellData>();
        _itemFactory = new ItemFactory<GridCellItem>(gridCellItemPrefab);
        
        CreateCells();
    }
    /*
    // todo: We can assign the dots to a cell in instantiate time!
    public void AssignDotsToCells(List<GridDotItem> dotItemList)
    {
        GridDotItem leftBottomDot;
        GridDotItem rightBottomDot;
        GridDotItem leftTopDot;
        GridDotItem rightTopDot;
        
        for (int i = 0; i < _itemCount; i++)
        {
            GridCellData gridCellData = _dataList[i];
            
            //left bottom
            int dotIndexNo =  (gridCellData.GridPosition.Y * _levelData.CellCountInRow) + (gridCellData.GridPosition.Y % _levelData.CellCountInColumn) + gridCellData.GridPosition.X;
            leftBottomDot = dotItemList[dotIndexNo];
            leftBottomDot.AddIntersectingCell(_itemList[i]);
            
            //right bottom
            dotIndexNo++;
            rightBottomDot = dotItemList[dotIndexNo];
            rightBottomDot.AddIntersectingCell(_itemList[i]);
            
            //left top
            dotIndexNo += _levelData.CellCountInRow;
            leftTopDot = dotItemList[dotIndexNo];
            leftTopDot.AddIntersectingCell(_itemList[i]);
            
            //right top
            dotIndexNo++;
            rightTopDot = dotItemList[dotIndexNo];
            rightTopDot.AddIntersectingCell(_itemList[i]);
            
            _itemList[i].AssignRelatedDots(leftBottomDot, rightBottomDot, leftTopDot, rightTopDot);
        }
    }
    */
    /*
    public void AssignLinesToCells(List<GridLineItem> dotItemList)
    {
        GridLineItem bottom;
        GridLineItem left;
        GridLineItem right;
        GridLineItem top;
        
        for (int i = 0; i < _itemCount; i++)
        {
            GridCellData gridCellData = _dataList[i];
            
            //bottom
            int lineIndexNo =  gridCellData.GridPosition.X + (gridCellData.GridPosition.Y * ((_levelData.CellCountInRow * 2 ) +1));
            bottom = dotItemList[lineIndexNo];
            bottom.AddIntersectingCell(_itemList[i]);
            
            //left
            lineIndexNo += _levelData.CellCountInRow;
            left = dotItemList[lineIndexNo];
            left.AddIntersectingCell(_itemList[i]);
            
            //right
            lineIndexNo++;
            right = dotItemList[lineIndexNo];
            right.AddIntersectingCell(_itemList[i]);
            
            //top
            lineIndexNo += _levelData.CellCountInRow;
            top = dotItemList[lineIndexNo];
            top.AddIntersectingCell(_itemList[i]);
            
            _itemList[i].AssignRelatedLines(bottom, left, right, top);
        }
    }
    */

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
                gridCellData.Init(indexNo, new GridPosition(c,r), _levelData.CellDefaultColor, _levelData.CellFullColor);
                
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
