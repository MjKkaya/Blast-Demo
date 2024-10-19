using System;


public static class GameplayEvents
{
    public static Action<DraggableShapeData> OnSelectedDraggableItem;
    public static Action OnDroppedDraggableItem;

    public static Action<ShapeDirections[], int> OnOverlappedDraggableItem;
    public static Action OnSeperatedDraggableItem;

    public static Action<ShapeDirections[], int> StartShapeMatchProcess;
    public static Action OnSucceededItemMatch;
    public static Action OnFailedItemMatch;

}