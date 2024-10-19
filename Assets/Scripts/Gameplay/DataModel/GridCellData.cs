using UnityEngine;


public class GridCellData  : BaseGridData
{
    private GridPosition _gridPosition;
    public GridPosition GridPosition
    {
        get { return _gridPosition; }
    }
    
    
    private Color _defaultColor;
    private Color _fullColor;
    
    
    public void Init(int locationIndexNo, GridPosition gridPosition, Color defaultColor, Color fullColor)
    {
        _locationIndexNo = locationIndexNo;
        _gridPosition = gridPosition;
        _defaultColor= defaultColor;
        _fullColor = fullColor;
    }
}