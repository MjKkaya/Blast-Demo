using UnityEngine;
using UnityEngine.UI;


public class GridLineItem  : BaseGridItem
{
    private GridLineData _lineData;
    
    [SerializeField]private Image _image;
    
    
    public void Init(GridLineData lineData, Transform newParent, Vector3 position, Quaternion rotation)
    {
        transform.SetParent(newParent);
        _lineData = lineData;
        _rectTransform.anchoredPosition3D = position;
        _rectTransform.localScale = _originalScale;
        _rectTransform.localRotation = rotation;
    }
    
    public void BackToDefaultView(Color color)
    {
        _image.color = color;
    }
    
    public void SetHighlightedView(Color color)
    {
        _image.color = color;
    }
    
    public void SetAsOccupied(Color color)
    {
        _image.color = color;
        _lineData.IsOccupied = true;
    }
    
    public void SetAsUnoccupied(Color color)
    {
        BackToDefaultView(color);
        _lineData.IsOccupied = false;
    }
}