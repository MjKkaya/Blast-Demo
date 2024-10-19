using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class DraggableItemMovementController : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster _canvasGaphicRaycaster;
    [SerializeField] private EventSystem _eventSystem;
    
    private PointerEventData _pointerEventData;
    private readonly List<RaycastResult> _resultList  = new ();

    private IAnchorable _lastAnchoredItem;
    private DraggableShapeData _draggableShapeData;
    private int _lastOverlappedDotIndex = -1;
    

    private void Awake()
    {
        //Set up the new Pointer Event
        GameplayEvents.OnSelectedDraggableItem += GameplayEvents_OnSelectedDraggableItem;
        GameplayEvents.OnOverlappedDraggableItem += GameplayEvents_OnOverlappedDraggableItem;
        GameplayEvents.OnDroppedDraggableItem += GameplayEvents_OnDroppedDraggableItem; 
        _pointerEventData = new PointerEventData(_eventSystem);
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
        //Set the Pointer Event Position to that of the mouse position
        _pointerEventData.position = Input.mousePosition;

        //Raycast using the Graphics Raycaster and mouse click position
        _canvasGaphicRaycaster.Raycast(_pointerEventData, _resultList);

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
            GameplayEvents.StartShapeMatchProcess?.Invoke(_draggableShapeData.ShapeDirection, _lastOverlappedDotIndex);
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