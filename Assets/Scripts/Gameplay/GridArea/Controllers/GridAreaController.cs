using UnityEngine;


public class GridAreaController : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;
    [Space]
    [SerializeField] private Transform _cellContainer;
    [SerializeField] private GridCellItem gridCellItemPrefab;
    [Space]
    [SerializeField] private Transform _dotContainer;
    [SerializeField] private GridDotItem gridDotItemPrefab;
    [Space]
    [SerializeField] private Transform _lineContainer;
    [SerializeField] private GridLineItem gridLineItemPrefab;
    
    private GridCellsController _gridCellsController;
    private GridDotsController _gridDotsController;
    private GridLinesController _gridLinesController;


    private void Awake()
    {
        GameplayEvents.OnOverlappedDraggableItem += GameplayEvents_OnOverlappedDraggableItem;
        GameplayEvents.OnSeperatedDraggableItem += GameplayEvents_OnSeperatedDraggableItem;
        GameplayEvents.StartShapeMatchProcess += GameplayEvents_StartShapeMatchProcess; 
        _gridCellsController = new GridCellsController();
        _gridDotsController = new GridDotsController();
        _gridLinesController = new GridLinesController();
    }

    private void Start()
    {
        Initialize();
    }

    private void OnDestroy()
    {
        GameplayEvents.OnOverlappedDraggableItem -= GameplayEvents_OnOverlappedDraggableItem;
        GameplayEvents.OnSeperatedDraggableItem -= GameplayEvents_OnSeperatedDraggableItem;
        GameplayEvents.StartShapeMatchProcess -= GameplayEvents_StartShapeMatchProcess; 
        _gridCellsController.Dispose();
        _gridDotsController.Dispose();
        _gridLinesController.Dispose();
    }

    
    private void Initialize()
    {
        _gridCellsController.Initialize(_levelData, _cellContainer, gridCellItemPrefab);
        _gridDotsController.Initialize(_levelData, _dotContainer, gridDotItemPrefab);
        _gridLinesController.Initialize(_levelData, _lineContainer, gridLineItemPrefab);

        // _gridCellsController.AssignDotsToCells(_gridDotsController.ItemList);//todo: no need!!!
        // _gridCellsController.AssignLinesToCells(_gridLinesController.ItemList); //we need for explode the cell
        _gridDotsController.AssignConnectedLines(_gridLinesController.DataList); 
    }
    
    
    private void GameplayEvents_OnOverlappedDraggableItem(ShapeDirections[] shapeDirections, int startingDotLocationIndex)
    {
        _gridLinesController.CheckAndMarkHighlightedLines(shapeDirections, startingDotLocationIndex, _gridDotsController.DataList);
    }

    private void GameplayEvents_OnSeperatedDraggableItem()
    {
        _gridLinesController.ResetHighlightedLines();
    }
    
    private void GameplayEvents_StartShapeMatchProcess(ShapeDirections[] directions, int dotIndex)
    {
        bool isMatch = _gridLinesController.CheckAndSetMatchedLines(directions, dotIndex, _gridDotsController.DataList);
        if(isMatch)
            GameplayEvents.OnSucceededItemMatch?.Invoke();
        else
            GameplayEvents.OnFailedItemMatch?.Invoke();
    }
}