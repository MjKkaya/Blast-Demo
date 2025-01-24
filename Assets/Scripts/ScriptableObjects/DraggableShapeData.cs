using UnityEngine;


[CreateAssetMenu(fileName = "ShapeData", menuName = "Blast-Demo/DraggableShapeData")]
public class DraggableShapeData : ScriptableObject
{
    public ShapeDirections[] ShapeDirection;
    [Range(0, 1)]
    public float HeightFactor;
    [Range(-0.5f, 0.5f)]
    public float WidthFactor;

    //This is calculating in DraggableShape.cs 
    private Vector2 _pivotPositionGap;
    public Vector2 PivotPositionGap => _pivotPositionGap;
    
    public void SetPivotPositionGap(Vector2 itemSize, Vector2 itemScale)
    {
        // _pivotPositionGap = new Vector2(itemSize.x * WidthFactor * itemScale.x, itemSize.y * HeightFactor * itemScale.y);
        // Debug.Log($"itemSize:{itemSize}, _pivotPositionGap:{_pivotPositionGap}");
        _pivotPositionGap = new Vector2(itemSize.x * WidthFactor * itemScale.x, itemSize.y * HeightFactor * itemScale.y);
        // Vector2 newSize = new Vector2(itemSize.x * WidthFactor * itemScale.x * canvasScaleFactor.x, itemSize.y * HeightFactor * itemScale.y * canvasScaleFactor.y);
        // _pivotPositionGap = oldSize;
    }
}


public enum ShapeDirections
{
    Left = 0,
    Right = 1,
    Up = 2,
    Down = 3,
    Max = 4
    //If we need
    /*
    Cross_left_down,
    Cross_left_up,
    Cross_right_up,
    Cross_right_down,
    */
}