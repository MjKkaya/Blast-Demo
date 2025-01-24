using System;
using System.Collections.Generic;
using UnityEngine;
//todo: add namespaces!!!

public class ScoreManager : MonoBehaviour
{
    private const string _bestScoreKey = "BS";
    private const int CellFillScore = 10;

    private int _bestScore;
    private int _currentScore;
    
    private int _comboCount;
    
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        
        _comboCount = 0;
        GameplayEvents.OnCreatedLevel += GameplayEvents_OnCreatedLevel;
        GameplayEvents.OnSucceededItemMatch += GameplayEvents_OnSucceededItemMatch;
        GameplayEvents.OnFailedItemMatch += GameplayEvents_OnFailedItemMatch;
        GameplayEvents.OnFilledWholeLine += GameplayEvents_OnFilledWholeLine;
        GameplayEvents.OnGameOver += GameplayEvents_OnGameOver;
    }

    private void Start()
    {
        GetBestScore();
    }

    private void OnDestroy()
    {
        GameplayEvents.OnCreatedLevel -= GameplayEvents_OnCreatedLevel;
        GameplayEvents.OnSucceededItemMatch -= GameplayEvents_OnSucceededItemMatch;
        GameplayEvents.OnFailedItemMatch -= GameplayEvents_OnFailedItemMatch;
        GameplayEvents.OnFilledWholeLine -= GameplayEvents_OnFilledWholeLine;
        GameplayEvents.OnGameOver -= GameplayEvents_OnGameOver;
    }

    private void GetBestScore()
    {
        _bestScore = PlayerPrefs.GetInt(_bestScoreKey, 0);
        ScoreEvents.OnLoadedScoreData?.Invoke(_bestScore);
    }
    
    private void UpdateBestScore()
    {
        if (_currentScore > _bestScore)
        {
            _bestScore = _currentScore;
            PlayerPrefs.SetInt(_bestScoreKey, _bestScore);
        }
    }
    
    
    
    private void GameplayEvents_OnFailedItemMatch()
    {
        _comboCount = 0;
        // Debug.Log($"GameplayEvents_OnFailedItemMatch:{_comboCount}");
    }

    private void GameplayEvents_OnSucceededItemMatch(int filledCellCount, DraggableShapeData data)
    {
        if(filledCellCount == 0)
            _comboCount = 0;
        else
            _comboCount += filledCellCount;
        
        int score = (data.ShapeDirection.Length +1) + (_comboCount * CellFillScore);
        _currentScore += score;
        // Debug.Log($"_comboCount:{_comboCount}, filledCellCount:{filledCellCount}, additionalCount:{additionalCount}");
        ScoreEvents.OnAddedPlayerScore(score);
    }

    private void GameplayEvents_OnFilledWholeLine(int cellCount)
    {
        int score = (cellCount + 1) * (_comboCount * CellFillScore);
        _currentScore += score;
        // Debug.Log($"GameplayEvents_OnFilledWholeLine-cellCount:{cellCount}, additionalCount:{additionalCount}");
        ScoreEvents.OnAddedPlayerScore(score);
    }

    private void GameplayEvents_OnCreatedLevel(LevelData arg1, List<GridDotData> arg2)
    {
        _currentScore = 0;
    }
    
    
    private void GameplayEvents_OnGameOver()
    {
        UpdateBestScore();
        UIEvents.OpenGameOverPanel?.Invoke(_bestScore, _currentScore);
    }
}
