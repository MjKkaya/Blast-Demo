using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private DraggableShape _lastActiveShape;
    public DraggableShape LastActiveShape => _lastActiveShape;
    
    [SerializeField]private Camera _followCamera;
    
    private Transform _defaultParent;
    private Vector2 _defaultAnchoredPosition;
    
    private Transform _transform;
    private RectTransform _rectTransform;
    private RectTransform _rootRectTransform;

    private bool _isDragging;

    private void Awake()
    {
        _transform = transform;
        _rectTransform = GetComponent<RectTransform>();
        _rootRectTransform = transform.root.GetComponent<RectTransform>();
        _defaultParent = _transform.parent;
        _defaultAnchoredPosition = _rectTransform.anchoredPosition;
        // Debug.Log($"_transform-localPosition:{_transform.localPosition}, position:{_transform.position}");
        // Debug.Log($"_rectTransform-localPosition{_rectTransform.localPosition}, position:{_rectTransform.position}, anchoredPosition:{_rectTransform.anchoredPosition}");
        GameplayEvents.OnFailedItemMatch += GameplayEvents_OnFailedItemMatch;
        GameplayEvents.OnSucceededItemMatch += GameplayEvents_OnSucceededItemMatch;
    }

    private void OnDestroy()
    {
        GameplayEvents.OnFailedItemMatch -= GameplayEvents_OnFailedItemMatch;
        GameplayEvents.OnSucceededItemMatch -= GameplayEvents_OnSucceededItemMatch;
    }

    public void Initialize(DraggableShape shape)
    {
        _lastActiveShape = shape;
        GoBackDefaultPlace();
        gameObject.SetActive(true);
    }
    
    private void GoBackDefaultPlace()
    {
        _transform.SetParent(_defaultParent);
        _rectTransform.anchoredPosition = _defaultAnchoredPosition;
        _isDragging = false;
    }
    

#region Drag-Drop

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
        _transform.SetParent(_transform.root);
        GameplayEvents.OnSelectedDraggableItem?.Invoke(_lastActiveShape.Data);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // _rectTransform.anchoredPosition = Input.mousePosition;
        // Debug.Log($"mousePosition:{Input.mousePosition}, ScreenToViewportPoint:{Camera.main.ScreenToViewportPoint(Input.mousePosition)}");
        // Debug.Log($"transform.root:{transform.root.name}, RectTransform:{transform.root.GetComponent<RectTransform>().rect}");
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rootRectTransform, Input.mousePosition, _followCamera, out var newPos);
        // Debug.Log($"mousePosition:{Input.mousePosition}, newPos:{newPos}, _rootRectTransform:{_rootRectTransform.sizeDelta}");
        _rectTransform.anchoredPosition = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameplayEvents.OnDroppedDraggableItem?.Invoke();
    }
    
#endregion
    
    private void GameplayEvents_OnFailedItemMatch()
    {
        if (!_isDragging)
            return;
     
        GoBackDefaultPlace();
    }
    
    private void GameplayEvents_OnSucceededItemMatch(int filledCellCount, DraggableShapeData data)
    {
        if (!_isDragging)
            return;
         
        gameObject.SetActive(false);
        GoBackDefaultPlace();
    }
}