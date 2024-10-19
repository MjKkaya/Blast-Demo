using UnityEngine;

public class GridDotData : BaseGridData
{
    private GridPosition _gridPosition;
    public GridPosition GridPosition
    {
        get { return _gridPosition; }
    }
    
    private Sprite _dotIcon;

    private GridLineData[] _connectedLineList;
    
    
    public void Init(int locationIndexNo, GridPosition gridPosition, Sprite dotIcon)
    {
        _locationIndexNo = locationIndexNo;
        _gridPosition = gridPosition;
        _dotIcon = dotIcon;
        // _connectedLineList = new GridLineData[4];
    }

    public new void Clear()
    {
        _locationIndexNo = _unplacedIndexNo;
        _dotIcon = null;
    }

    public void AssignRelatedLines(GridLineData[] lineDataArray)
    {
        _connectedLineList = lineDataArray;
    }

    public bool IsConnectedLineAvailable(ShapeDirections firstDirection)
    {
        GridLineData lineData = _connectedLineList[(int)firstDirection];
        return lineData != null && !lineData.IsOccupied;
    }

    public GridLineData GetConnectedGridLineData(ShapeDirections direction)
    {
        return _connectedLineList[(int)direction];
    }
}