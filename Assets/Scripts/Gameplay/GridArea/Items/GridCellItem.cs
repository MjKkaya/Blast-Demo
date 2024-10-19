using UnityEngine;
using UnityEngine.UI;


public class GridCellItem : BaseGridItem, ISettleable
{
    private bool _isFull;
    public bool IsFull
    {
        get => _isFull;
    }
    
    private GridCellData _cellData;
    
    GridDotItem _leftBottomDot;
    GridDotItem _rightBottomDot;
    GridDotItem _leftTopDot;
    GridDotItem _rightTopDot;
    
    GridLineItem _bottomLine;
    GridLineItem _leftLine;
    GridLineItem _rightLine;
    GridLineItem _topLine;
    
    
    public void Init(GridCellData cellData, Transform newParent, Vector3 position)
    {
        transform.SetParent(newParent);
        _cellData = cellData;
        _rectTransform.anchoredPosition3D = position;
        _rectTransform.localScale = _originalScale;
    }

    public void AssignRelatedDots(GridDotItem leftBottomDot, GridDotItem rightBottomDot, GridDotItem leftTopDot, GridDotItem rightTopDot)
    {
        _leftBottomDot = leftBottomDot;
        _rightBottomDot = rightBottomDot;
        _leftTopDot = leftTopDot;
        _rightTopDot = rightTopDot;
    }

    public void AssignRelatedLines(GridLineItem bottomLine, GridLineItem leftLine, GridLineItem rightLine, GridLineItem topLine)
    {
        _bottomLine = bottomLine;
        _leftLine = leftLine;
        _rightLine = rightLine;
        _topLine = topLine;
    }

    public bool StartCheckProcess()
    {
        if (_isFull)
            return false;

        //todo: trigger event!
        // ShapeMatchController.StartMatch();
        
        //todo: delete!
        GetComponent<Image>().color = Color.red;
        
        _bottomLine.GetComponent<Image>().color = Color.green;
        _leftLine.GetComponent<Image>().color = Color.green;
        _rightLine.GetComponent<Image>().color = Color.green;
        _topLine.GetComponent<Image>().color = Color.green;

        return true;
    }
    
    public void StopCheckProcess()
    {
        GetComponent<Image>().color = Color.blue;
        _bottomLine.GetComponent<Image>().color = Color.white;
        _leftLine.GetComponent<Image>().color = Color.white;
        _rightLine.GetComponent<Image>().color = Color.white;
        _topLine.GetComponent<Image>().color = Color.white;
    }
}