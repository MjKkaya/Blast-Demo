using UnityEngine;


public class DraggableShape : MonoBehaviour, IPoolable
{
    private readonly Vector3 _startPosition = new Vector3(0, 0, 0);
    private readonly Vector3 _startScale = new Vector3(0.5f, 0.5f, 1f);
    
    [SerializeField] private DraggableShapeData data;
    public DraggableShapeData Data => data;
    
    private Transform _transform;
    private RectTransform _rectTransform;
    private Vector2 _size;
    private bool _isInitialized;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _transform = transform;
        _size = _rectTransform.rect.size;
    }


    
    public void Initialize(Transform newParent)
    {
        SetParent(newParent);
        if (_isInitialized)
            return;
        
        data.SetPivotPositionGap(_size, _rectTransform.localScale);
        _isInitialized = true;
    }
    private void SetParent(Transform newParent)
    {
        _transform.SetParent(newParent);
        _rectTransform.anchoredPosition3D = _startPosition;
        _rectTransform.localScale = _startScale;
    }

    public void Prepare()
    {
        gameObject.SetActive(true);
    }
    
    public void Clear()
    {
        gameObject.SetActive(false);
    }
    
    public void Dispose()
    {
        data = null;
    }
}
