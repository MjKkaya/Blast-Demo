using UnityEngine;


[CreateAssetMenu(fileName = "ShapeData", menuName = "Blast-Demo/DraggableShapeData")]
public class DraggableShapeData : ScriptableObject
{
    public ShapeDirections[] ShapeDirection;
}

/*
[Flags]
public enum ShapeDirection
{ 
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8
}
*/

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