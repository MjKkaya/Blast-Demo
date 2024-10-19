using UnityEngine;


[RequireComponent(typeof(RectTransform))]
public abstract class BaseGridItem : MonoBehaviour, IPoolable
{
    protected static readonly Vector3 _originalScale = new Vector3(1f, 1f, 1f);
    
    protected RectTransform _rectTransform;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
}
