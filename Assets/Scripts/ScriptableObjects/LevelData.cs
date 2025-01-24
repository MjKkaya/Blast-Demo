using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LevelData", menuName = "Blast-Demo/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("structural Data")]
    public byte CellCountInColumn;
    public byte CellCountInRow;
    
    [Tooltip("Start position of the first Cell and Dot")]
    public Vector2 CellStartPos;
    public Vector2 DotStartPos;
    
    public float GapBetweenCell;
    
    
    [Header("Visual Data")]
    public Sprite DotIcon;
    [Space]
    public Color CellDefaultColor;
    public Color CellFullColor;
    [Space]
    public Color LineDefaultColor;
    public Color LineHighlightedColor;
    public Color LineFullColor;
}