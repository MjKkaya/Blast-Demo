using UnityEngine;


public class GridDotItem : BaseGridItem, IAnchorable
{
    private GridDotData _dotData;

    // private readonly List<ISettleable> _intersectingCells = new (4);

    public void Init(GridDotData dotData, Transform newParent, Vector3 position)
    {
        transform.SetParent(newParent);
        _dotData = dotData;
        _rectTransform.anchoredPosition3D = position;
        _rectTransform.localScale = _originalScale;
        // _intersectingCells.Clear();
    }


    public void Overlapped(ShapeDirections[] shapeDirections)
    {
        //This is only checking first direction of the shape. It's not given an idea about match.
        if (!_dotData.IsConnectedLineAvailable(shapeDirections[0]))
            return;
        
        GameplayEvents.OnOverlappedDraggableItem?.Invoke(shapeDirections, _dotData.LocationIndexNo);
    }
}