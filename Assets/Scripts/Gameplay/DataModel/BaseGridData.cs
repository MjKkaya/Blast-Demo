// public class DataFactory <T> where T  : class, IPoolable, new()
// public abstract class BaseGridData<T>  where T : IPoolable

// public abstract class BaseGridData : IPoolable
public class BaseGridData : IPoolable
{
    protected const int _unplacedIndexNo = -1;
    
    /// <summary>
    /// 2 dimensional array converted to single line array and LocationIndexNo represents single dimension array index number.
    ///  ex: in 4x4 grid area, 2x3 grid location => (2x4) + 3 = 11 
    /// </summary>
    protected int _locationIndexNo;
    public int LocationIndexNo
    {
        get { return _locationIndexNo; }
    }

    public bool IsOccupied { get; set; }


    public void Clear()
    {
        _locationIndexNo = _unplacedIndexNo;
    }

    public void Dispose()
    {
        Clear();
    }
}
