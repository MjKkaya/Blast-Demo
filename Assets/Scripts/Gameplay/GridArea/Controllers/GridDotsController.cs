using System.Collections.Generic;
using UnityEngine;

public class GridDotsController: BaseGridController<GridDotData, GridDotItem>
{
    public void Initialize(LevelData levelData, Transform itemContainer, GridDotItem itemPrefab)
    {
        _levelData = levelData;
        _itemContainer = itemContainer;
        _itemCount = (_levelData.CellCountInColumn + 1) * (_levelData.CellCountInRow + 1);
        _dataFactory = new DataFactory<GridDotData>();
        _itemFactory = new ItemFactory<GridDotItem>(itemPrefab);
        
        CreateDots();
    }

    public void AssignConnectedLines(List<GridLineData> lineDataList)
    {
        for (int i = 0; i < _itemCount; i++)
        {
            GridLineData[] lineDataArray = new GridLineData[(int)ShapeDirections.Max];
            GridDotData gridDotData = _dataList[i];
            int lineIndexNo;
            
            //right
            if (gridDotData.GridPosition.X < _levelData.CellCountInRow)
            {
                lineIndexNo = gridDotData.LocationIndexNo + (gridDotData.GridPosition.Y * _levelData.CellCountInRow);
                lineDataArray[(int) ShapeDirections.Right] = lineDataList[lineIndexNo];
            }
            
            //left
            if (gridDotData.GridPosition.X > 0)
            {
                lineIndexNo = gridDotData.LocationIndexNo + (gridDotData.GridPosition.Y * _levelData.CellCountInRow) - 1;
                lineDataArray[(int) ShapeDirections.Left] = lineDataList[lineIndexNo];
            }
            
            //up
            if (gridDotData.GridPosition.Y < _levelData.CellCountInColumn)
            {
                lineIndexNo = gridDotData.LocationIndexNo + ((gridDotData.GridPosition.Y + 1) * _levelData.CellCountInRow);
                lineDataArray[(int) ShapeDirections.Up] = lineDataList[lineIndexNo];
            }
            
            //down
            if (gridDotData.GridPosition.Y > 0)
            {
                lineIndexNo = gridDotData.LocationIndexNo + ((gridDotData.GridPosition.Y - 1) * _levelData.CellCountInRow) - 1;
                lineDataArray[(int) ShapeDirections.Down] = lineDataList[lineIndexNo];
            }

            gridDotData.AssignRelatedLines(lineDataArray);
        }
    }

    private void CreateDots()
    {
        _dataList.Clear();
        _dataFactory.CreateDataList(_dataList, _itemCount);
        
        _itemList.Clear();
        _itemFactory.CreateItemList(_itemList, _itemCount);

        int rowCount = _levelData.CellCountInColumn + 1;
        int columnCount = _levelData.CellCountInRow + 1;
        int indexNo = 0;
        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < columnCount; c++)
            {
                GridDotData gridDotData = _dataList[indexNo];
                gridDotData.Init(indexNo, new GridPosition(c,r), _levelData.DotIcon);
                
                GridDotItem gridDotItem =_itemList[indexNo];
                Vector3 position = CalculateItemPosition(r, c, _levelData.GapBetweenCell);
                gridDotItem.Init(gridDotData, _itemContainer, position);
                indexNo++;
            }
        }
    }

    private Vector3 CalculateItemPosition(int rowIndexNo, int columnIndexNo, float gap)
    {
        float posX = _levelData.DotStartPos.x + (columnIndexNo * gap);
        float posY = _levelData.DotStartPos.y + (rowIndexNo * gap);
        return new Vector3(posX, posY);
    }
}
