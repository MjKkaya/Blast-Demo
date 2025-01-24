using System.Collections.Generic;
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
        GameplayEvents.CellMadeUnoccupied += GameplayEvents_CellMadeUnoccupied;
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
        GameplayEvents.CellMadeUnoccupied -= GameplayEvents_CellMadeUnoccupied;
        _gridCellsController.Dispose();
        _gridDotsController.Dispose();
        _gridLinesController.Dispose();
    }

    public void PlayAgain()
    {
        _gridCellsController.Reset();
        _gridLinesController.Reset();
        GameplayEvents.OnCreatedLevel?.Invoke(_levelData, _gridDotsController.DataList);
    }
    
    
    private void Initialize()
    {
        _gridCellsController.Initialize(_levelData, _cellContainer, gridCellItemPrefab);
        _gridDotsController.Initialize(_levelData, _dotContainer, gridDotItemPrefab);
        _gridLinesController.Initialize(_levelData, _lineContainer, gridLineItemPrefab);

        _gridDotsController.AssignConnectedLines(_gridLinesController.DataList); 
        _gridLinesController.AssignConnectedCellsAndLines(_gridCellsController.DataList); 
        GameplayEvents.OnCreatedLevel?.Invoke(_levelData, _gridDotsController.DataList);
    }

    
    #region Gameplay Events

    private void GameplayEvents_OnOverlappedDraggableItem(ShapeDirections[] shapeDirections, int startingDotLocationIndex)
    {
        _gridLinesController.CheckAndMarkHighlightedLines(shapeDirections, startingDotLocationIndex, _gridDotsController.DataList);
    }

    private void GameplayEvents_OnSeperatedDraggableItem()
    {
        _gridLinesController.ResetHighlightedLines();
    }
    
    private void GameplayEvents_StartShapeMatchProcess(DraggableShapeData shapeData, int dotIndex)
    {
        bool isMatch = _gridLinesController.CheckAndSetMatchedLines(shapeData.ShapeDirection, dotIndex, _gridDotsController.DataList);
        if (isMatch)
        {
            List<GridCellData> filledCellList = _gridCellsController.CheckAndSetFilledCells(_gridLinesController.MatchedDataList);
            int filledCellCount = filledCellList.Count;
            // Debug.Log($"Mj-isFilledCell:{filledCellCount}");
            GameplayEvents.OnSucceededItemMatch?.Invoke(filledCellCount, shapeData);
            
            if (filledCellCount > 0)
                _gridCellsController.CheckAndDestroyFilledRowAndColumn(filledCellList);

            // if(isFilledCell)
            //     GameplayEvents.OnFilledCell?.Invoke();//todo: add point and add sound!
            //todo : Add a delay for:
            // to call OnSucceededItemMatch
        }
        else
            GameplayEvents.OnFailedItemMatch?.Invoke();
    }

    private void GameplayEvents_CellMadeUnoccupied(int cellIndexNo)
    {
        _gridLinesController.SetLinesAsUnOccupied(_gridCellsController.DataList[cellIndexNo].ConnectedLines);
    }
    
    #endregion
}