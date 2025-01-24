using UnityEngine;
using UnityEngine.UI;


public class GridCellItem : BaseGridItem
{
    private GridCellData _cellData;
    
    [SerializeField]private Image _image;
    
    public void Init(GridCellData cellData, Transform newParent, Vector3 position)
    {
        transform.SetParent(newParent);
        _cellData = cellData;
        _rectTransform.anchoredPosition3D = position;
        _rectTransform.localScale = _originalScale;
    }
    
    public void SetAsUnoccupied(Color color)
    {
        _image.color = color;
        _cellData.IsOccupied = false;
    }
    
    public void SetAsOccupied(Color color)
    {
        _image.color = color;
        _cellData.IsOccupied = true;
    }
}