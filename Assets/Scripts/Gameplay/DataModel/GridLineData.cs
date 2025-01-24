using System.Collections.Generic;


//todo: we can use struct instead of class!
public class GridLineData : BaseGridData
{
    private readonly List<GridCellData> _connectedCellList = new(2);
    public List<GridCellData> ConnectedCellList => _connectedCellList;
    
    public void Init(int locationIndexNo)
    {
        _locationIndexNo = locationIndexNo;
    }

    public void AddConnectedCell(GridCellData connectedCell)
    {
        _connectedCellList.Add(connectedCell);
    }
    
    /*
    //todo:delete it - for test
    public string PrintConnectedCell()
    {
        string str = string.Empty;
        foreach (var cell in _connectedCellList)
        {
            str += $"{cell.LocationIndexNo},";
        }

        return str;
    }
    */
}

