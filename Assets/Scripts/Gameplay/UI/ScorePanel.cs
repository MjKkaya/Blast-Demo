using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ScorePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _lastScoreText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;

    private int _score;
    private int Score
    {
        get => _score;
        set
        {
            _score = value;
            _scoreText.text = _score.ToString();
        }
    }

    private int _bestScore;
    
    
    private void Awake()
    {
        GameplayEvents.OnCreatedLevel += GameplayEvents_OnCreatedLevel;
        ScoreEvents.OnLoadedScoreData += ScoreEvents_OnLoadedScoreData;
        ScoreEvents.OnAddedPlayerScore += ScoreEvents_OnAddedPlayerScore;
        Score = 0;
    }

    private void OnDestroy()
    {
        GameplayEvents.OnCreatedLevel -= GameplayEvents_OnCreatedLevel;
        ScoreEvents.OnLoadedScoreData -= ScoreEvents_OnLoadedScoreData;
        ScoreEvents.OnAddedPlayerScore -= ScoreEvents_OnAddedPlayerScore;
    }

    
    private void GameplayEvents_OnCreatedLevel(LevelData arg1, List<GridDotData> arg2)
    {
        _lastScoreText.text = $"Last: {_score}";
        if (_score > _bestScore)
        {
            _bestScore = _score;
            _bestScoreText.text = $"Best: {_bestScore}";
        }
            
        Score = 0;
    }


    private void ScoreEvents_OnLoadedScoreData(int bestScore)
    {
        _bestScore = bestScore;
        _bestScoreText.text = $"Best: {_bestScore}";
    }
    

    private void ScoreEvents_OnAddedPlayerScore(int score)
    {
        Score += score;
    }
}