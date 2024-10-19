using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridLineItem  : BaseGridItem
{
    private GridLineData _lineData;
    
    // private readonly List<ISettleable> _intersectingCells = new(2);
    
    [SerializeField]private Image _image;
    
    public void Init(GridLineData lineData, Transform newParent, Vector3 position, Quaternion rotation)
    {
        transform.SetParent(newParent);
        _lineData = lineData;
        _rectTransform.anchoredPosition3D = position;
        _rectTransform.localScale = _originalScale;
        _rectTransform.localRotation = rotation;
        // _intersectingCells.Clear();
    }
    
    public void AddIntersectingCell(ISettleable cellItem)
    {
        // _intersectingCells.Add(cellItem);
    }

    public void BackToDefaultView()
    {
        _image.color = Color.white;
    }
    
    public void SetHighlightedView()
    {
        _image.color = Color.green;
    }
    
    public void SetOccupied()
    {
        _image.color = Color.red;
        _lineData.IsOccupied = true;
    }
}