using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class DraggableItemMovementController : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Canvas _followCanvas;
    
    /*
    [Header("For Test")]
    [SerializeField] private Camera testFollowCamera;
    [SerializeField] private RectTransform testMouseItem;
    [SerializeField] private RectTransform testConvertedItem;
    [SerializeField] private RectTransform testFollowItem;
    private RectTransform _testGridAreaRectTransform;
    */
    
    private GraphicRaycaster _canvasGraphicRaycaster;
    private PointerEventData _pointerEventData;
    private readonly List<RaycastResult> _resultList  = new (4);

    private IAnchorable _lastAnchoredItem;
    private DraggableShapeData _draggableShapeData;
    private int _lastOverlappedDotIndex = -1;
    private float _canvasScaleFactor;
    
    
    private void Awake()
    {
        GameplayEvents.OnSelectedDraggableItem += GameplayEvents_OnSelectedDraggableItem;
        GameplayEvents.OnOverlappedDraggableItem += GameplayEvents_OnOverlappedDraggableItem;
        GameplayEvents.OnDroppedDraggableItem += GameplayEvents_OnDroppedDraggableItem; 
        _pointerEventData = new PointerEventData(_eventSystem);
        _canvasGraphicRaycaster = _followCanvas.GetComponent<GraphicRaycaster>();
        // _testGridAreaRectTransform = _followCanvas.GetComponent<RectTransform>();
    }

    private void Start()
    {
        _canvasScaleFactor = _followCanvas.scaleFactor;
        // Debug.Log($"Mj-_canvasScaleFactor{_canvasScaleFactor}");
    }

    private void OnDestroy()
    {
        GameplayEvents.OnSelectedDraggableItem -= GameplayEvents_OnSelectedDraggableItem; 
        GameplayEvents.OnOverlappedDraggableItem -= GameplayEvents_OnOverlappedDraggableItem;
        GameplayEvents.OnDroppedDraggableItem -= GameplayEvents_OnDroppedDraggableItem; 
    }

    private void Update()
    {
        if (_draggableShapeData)
            CheckOverlapItem();
    }

    private void CheckOverlapItem()
    {
        //For Test item-Start
        /*
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_testGridAreaRectTransform, Input.mousePosition, testFollowCamera, out var testPosition);
        testConvertedItem.anchoredPosition = testPosition;
        testPosition += _draggableShapeData.PivotPositionGap;
        testFollowItem.anchoredPosition = testPosition;
        */
        //--------

        Vector2 newMousePoint = new Vector2(Input.mousePosition.x + (_draggableShapeData.PivotPositionGap.x * _canvasScaleFactor), Input.mousePosition.y + (_draggableShapeData.PivotPositionGap.y * _canvasScaleFactor));;
        // Debug.Log($"Mj-mousePosition:{Input.mousePosition}, newMousePoint:{newMousePoint}");
        
        //For Test item-End
        /*
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_testGridAreaRectTransform, newMousePoint, testFollowCamera, out var startPoint2);
        testMouseItem.anchoredPosition = startPoint2;
        //--------
        */
            
        _pointerEventData.position = newMousePoint;

        //Raycast using the Graphics Raycaster and mouse click position
        _canvasGraphicRaycaster.Raycast(_pointerEventData, _resultList);
            
        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        if (_resultList.Count == 0)
            return;

        int count = _resultList.Count;
        IAnchorable anchorableItem = null;
        for (int i = 0; i < count; i++)
        {
            anchorableItem = _resultList[i].gameObject.GetComponent<IAnchorable>();
            //We are break the loop because mouse is on an IAnchorable item. We don't need to look at other objects!
            if (anchorableItem != null)
                break;
        }
        
        if (anchorableItem == null )
        {
            if (_lastAnchoredItem != null)
            {
                GameplayEvents.OnSeperatedDraggableItem?.Invoke();
                _lastAnchoredItem = null;
                // Debug.Log($"CheckOverlapItem-OnSeperatedDraggableItem-list:{count}");
            }
        }
        else if (anchorableItem != _lastAnchoredItem)
        {
            //Mouse is overlap to new IAnchorable item.
            // Debug.Log($"CheckOverlapItem-Overlapped-list:{count}");
            anchorableItem.Overlapped(_draggableShapeData.ShapeDirection);
            _lastAnchoredItem = anchorableItem;
        }
        
        _resultList.Clear();
    }

    private void GameplayEvents_OnSelectedDraggableItem(DraggableShapeData data)
    {
        _draggableShapeData = data;
    }

    private void GameplayEvents_OnOverlappedDraggableItem(ShapeDirections[] shapeDirections, int dotIndexNo)
    {
        // we have _draggableShapeData.
        _lastOverlappedDotIndex = dotIndexNo;
    }

    private void GameplayEvents_OnDroppedDraggableItem()
    {
        if (_lastAnchoredItem != null && _lastOverlappedDotIndex != -1)
        {
            GameplayEvents.StartShapeMatchProcess?.Invoke(_draggableShapeData, _lastOverlappedDotIndex);
        }
        else
        {
            GameplayEvents.OnFailedItemMatch?.Invoke();
        }
        
        _draggableShapeData = null;
        _lastAnchoredItem = null;
        _lastOverlappedDotIndex = -1;
    }
}