using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class GameOverController : MonoBehaviour
{
    //Listen Item selected and item creatinon 
    //But run: 1: only all 3 item created.
    //2: When Item dropped 

    private LevelData _levelData;
    private List<GridDotData> _dotDataList;
    private List<DraggableShapeData> _shapeDataList;
    
    
    private void Awake()
    {
        GameplayEvents.OnCreatedLevel += GameplayEvents_OnCreatedLevel;
        GameplayEvents.OnSucceededItemMatch += GameplayEvents_OnSucceededItemMatch;
        GameplayEvents.OnCreatedDraggableShapeDatas += GameplayEvents_OnCreatedDraggableShapeDatas;
        _shapeDataList = new(3);
    }

    private void OnDestroy()
    {
        GameplayEvents.OnCreatedLevel -= GameplayEvents_OnCreatedLevel;
        GameplayEvents.OnSucceededItemMatch -= GameplayEvents_OnSucceededItemMatch;
        GameplayEvents.OnCreatedDraggableShapeDatas -= GameplayEvents_OnCreatedDraggableShapeDatas;
    }


    //todo:convert NativeArrayList and job
    
    /// <summary>
    /// We trigger this method for all item creation and after dropped item!
    /// We need to check all remaining items because dragging item should be unplaced but other items can be, and it will be make an empty place after dropped! 
    /// </summary>
    private void StartToCheckGameOver()
    {
        Debug.Log($"GameOvercontroller-StartToCheckGameOver-start, shapeCount:{_shapeDataList.Count}");
        if (_shapeDataList.Count == 0)
            return;
        
        int cellCountInRow = _levelData.CellCountInRow; 
        
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        int shapeCount = _shapeDataList.Count;
        int dotCount = _dotDataList.Count;
        bool isThereSpace = false;
        
        //Remaining draggable shape count
        for (int i = 0; i < shapeCount; i++)
        {
            ShapeDirections[] shapeDirections = _shapeDataList[i].ShapeDirection;
            int directionLength = shapeDirections.Length;
            
            //All dot count
            for (int d = 0; d < dotCount; d++)
            {
                int matchedCount = 0;
                int dotLocationIndex = d;
                
                //We took first dot and start to check if  the shape is fit the line
                for (int sd = 0; sd < directionLength; sd++)
                {
                    GridDotData dotData = _dotDataList[dotLocationIndex];
                    GridLineData gridLineData = dotData.GetConnectedGridLineData(shapeDirections[sd]);
                    if (gridLineData == null || gridLineData.IsOccupied)
                        break;

                    matchedCount++;
                    dotLocationIndex = GridDotsController.GetConnectedDotLocationIndex(cellCountInRow, dotLocationIndex, shapeDirections[sd]);
                    if (dotLocationIndex < 0 || dotLocationIndex > dotCount)
                        break;
                }

                if (directionLength == matchedCount)
                {
                    // Debug.Log($"isThereSpace is true: shapeIndex:{i}, shape:{_shapeDataList[i].name},  dotIndexes:{d}...{dotLocationIndex}, ");
                    isThereSpace = true;
                    break;
                }
            }

            if (isThereSpace)
                break;
        }
        
        Debug.Log($"GameOvercontroller-StartToCheckGameOver-isThereSpace:{isThereSpace}, shapeCount:{shapeCount}, dotcount:{dotCount}");
        if (!isThereSpace)
            GameplayEvents.OnGameOver?.Invoke();
        
        stopwatch.Stop();
        // Debug.Log($"stopwatch:{stopwatch.Elapsed.TotalMilliseconds}");
        
    }

    private void GameplayEvents_OnCreatedLevel(LevelData levelData, List<GridDotData> dotDataList)
    {
        _levelData = levelData;
        _dotDataList = dotDataList;
    }
    
    private void GameplayEvents_OnSucceededItemMatch(int cellIndex, DraggableShapeData shapeData)
    {
        Debug.Log($"GameOvercontroller-OnSucceededItemMatch-shapeData:{shapeData}");
        _shapeDataList.Remove(shapeData);
        if (_shapeDataList.Count > 0)
        {
            CancelInvoke();
            //We must add delay to wait score update
            Invoke(nameof(StartToCheckGameOver), 1.5f);
        }
    }
    
    private void GameplayEvents_OnCreatedDraggableShapeDatas(List<DraggableShapeData> shapeDataList)
    {
        Debug.Log($"GameOvercontroller-OnCreatedDraggableShapeDatas-shapeDataList:{shapeDataList.Count}");
        _shapeDataList.Clear();
        int count = shapeDataList.Count;
        for (int i = 0; i < count; i++)
        {
            _shapeDataList.Add(shapeDataList[i]);
        }

        CancelInvoke();
        Invoke(nameof(StartToCheckGameOver), 1.5f);
    }
}