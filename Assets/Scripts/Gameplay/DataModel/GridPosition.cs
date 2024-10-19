public struct GridPosition
{
    public int X { get; private set; }
    public int Y { get; private set; }


    public GridPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Set(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString() => $"({X}, {Y})";
}