using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _defaultParent;
    private Vector2 _defaultAnchoredPosition;
    
    private Transform _transform;
    private RectTransform _rectTransform;

    [SerializeField]private DraggableShapeData draggableShapeData;
    private bool _isDragging;

    private void Awake()
    {
        _transform = transform;
        _rectTransform = GetComponent<RectTransform>();
        _defaultParent = _transform.parent;
        _defaultAnchoredPosition = _rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        GameplayEvents.OnFailedItemMatch += GameplayEvents_OnFailedItemMatch;
        GameplayEvents.OnSucceededItemMatch += GameplayEvents_OnSucceededItemMatch;
    }

    private void OnDisable()
    {
        GameplayEvents.OnFailedItemMatch -= GameplayEvents_OnFailedItemMatch;
        GameplayEvents.OnSucceededItemMatch -= GameplayEvents_OnSucceededItemMatch;
    }

    private void GoBackDefaultPlace()
    {
        _transform.SetParent(_defaultParent);
        _rectTransform.anchoredPosition = _defaultAnchoredPosition;
        _isDragging = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
        _transform.SetParent(_transform.root);
        GameplayEvents.OnSelectedDraggableItem?.Invoke(draggableShapeData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameplayEvents.OnDroppedDraggableItem?.Invoke();
    }
    
    private void GameplayEvents_OnFailedItemMatch()
    {
        if (_isDragging)
        {
            GoBackDefaultPlace();
        }
    }
    
    private void GameplayEvents_OnSucceededItemMatch()
    {
        if (_isDragging)
        {
            gameObject.SetActive(false);
            GoBackDefaultPlace();
        }
    }
}