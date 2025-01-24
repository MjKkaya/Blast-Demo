using System.Collections.Generic;
using UnityEngine;


public class DraggableItemCreationManager : MonoBehaviour
{
    [SerializeField] private DraggableShape[] _draggableShapePrefabs;
    [SerializeField] private DraggableItem[] _draggableItemList;
    [SerializeField] private Transform _draggableCanvas;

    private ItemListFactory<DraggableShape> _itemFactory;
    private int _maxItemCreationCount;
    private int _shapePrefabCount;
    private int _remainDraggableItemCount;
    
    private List<DraggableShapeData> _draggableShapeDataList;
    
    
    private void Awake()
    {
        GameplayEvents.OnCreatedLevel += GameplayEvents_OnCreatedLevel; 
        GameplayEvents.OnSucceededItemMatch += GameplayEvents_OnSucceededItemMatch;
        
        _shapePrefabCount = _draggableShapePrefabs.Length;
        _maxItemCreationCount = _draggableItemList.Length;
        _itemFactory = new ItemListFactory<DraggableShape>(_draggableShapePrefabs);
        _draggableShapeDataList = new List<DraggableShapeData>(_maxItemCreationCount);
    }

    private void OnDestroy()
    {
        GameplayEvents.OnCreatedLevel -= GameplayEvents_OnCreatedLevel; 
        GameplayEvents.OnSucceededItemMatch -= GameplayEvents_OnSucceededItemMatch;
    }

    
    private void CreateDraggableShape()
    {
        ReleasePreviousShapeItems();
        CreateRandomShapeData();
        GameplayEvents.OnCreatedDraggableShapeDatas?.Invoke(_draggableShapeDataList);
    }
    
    private void ReleasePreviousShapeItems()
    {
        for (int i = 0; i < _maxItemCreationCount; i++)
        {
            if(_draggableItemList[i].transform.childCount == 0)
                continue;
            // Debug.Log($"ReleasePreviousShapeItems:{_draggableItemList[i].transform.GetChild(0).GetComponent<DraggableShape>().Data}");
            DraggableShape shapePrefab = GetDraggableShapePrefab(_draggableItemList[i].LastActiveShape.Data); 
            _itemFactory.ReleaseToPool(shapePrefab, _draggableItemList[i].LastActiveShape);
        }
    }
    
    private DraggableShape GetDraggableShapePrefab(DraggableShapeData shapeData)
    {
        for (int i = 0; i < _shapePrefabCount; i++)
        {
            if (_draggableShapePrefabs[i].Data == shapeData)
                return _draggableShapePrefabs[i];
        }
        return null;
    }
    
    private void CreateRandomShapeData()
    {
        _draggableShapeDataList.Clear();
        for (int i = 0; i < _maxItemCreationCount; i++)
        {
            int randomIndex = Random.Range(0, _shapePrefabCount);
            DraggableShape shapeItem = _itemFactory.GetItem(_draggableShapePrefabs[randomIndex]);
            shapeItem.Initialize(_draggableItemList[i].transform);
            _draggableShapeDataList.Add(shapeItem.Data);
            _draggableItemList[i].Initialize(shapeItem);
        }

        _remainDraggableItemCount = _maxItemCreationCount;
    }
    
    
    private void GameplayEvents_OnCreatedLevel(LevelData arg1, List<GridDotData> arg2)
    {
        CreateDraggableShape();
    }
    
    private void GameplayEvents_OnSucceededItemMatch(int filledCellCount, DraggableShapeData shapeData)
    {
        _remainDraggableItemCount--;
        if (_remainDraggableItemCount != 0) 
            return;

        //we must add delay because some classes listen this event and GameplayEvents.OnCreatedDraggableShapeDatas
        Invoke(nameof(CreateDraggableShape), 0.25f);
    }
}