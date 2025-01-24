using UnityEngine;


public class GridCellData  : BaseGridData
{
    private GridPosition _gridPosition;
    public GridPosition GridPosition
    {
        get { return _gridPosition; }
    }
    
    //There are 4 sides: Left-right-Up-Down.
    private GridLineData[] _connectedLines = new GridLineData[(int)ShapeDirections.Max];
    public GridLineData[] ConnectedLines => _connectedLines;
    
    
    public void Init(int locationIndexNo, GridPosition gridPosition)
    {
        _locationIndexNo = locationIndexNo;
        _gridPosition = gridPosition;
    }

    public void AddConnectedLine(int indexNo, GridLineData lineData)
    {
        _connectedLines[indexNo] = lineData;
    }

    public bool CheckSetCellState()
    {
        int count = _connectedLines.Length;
        for (int i = 0; i < count; i++)
        {
            if(!_connectedLines[i].IsOccupied)
                return false;
        }
        return true;
    }
}