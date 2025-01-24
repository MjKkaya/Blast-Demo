using System;
using System.Collections.Generic;


public static class GameplayEvents
{
    public static Action<DraggableShapeData> OnSelectedDraggableItem;
    public static Action OnDroppedDraggableItem;

    public static Action<ShapeDirections[], int> OnOverlappedDraggableItem;
    public static Action OnSeperatedDraggableItem;

    public static Action<DraggableShapeData, int> StartShapeMatchProcess;
    
    //first parameter: Filled cell count. 0 => n, second parameter: dragging item "dot/edge" count 
    public static Action<int, DraggableShapeData> OnSucceededItemMatch;
    public static Action OnFailedItemMatch;
    
    //int: filled cell count in row or column
    public static Action<int> OnFilledWholeLine;
    
    //int:GridCEllLocation index
    public static Action<int> CellMadeUnoccupied;

    public static Action PlayAgain;
    //LevelData, List<GridDotData> dotDataList
    public static Action<LevelData, List<GridDotData>> OnCreatedLevel;
    public static Action<List<DraggableShapeData>> OnCreatedDraggableShapeDatas;
    public static Action OnGameOver;
}