using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel; 
    [SerializeField] private Button _playButton; 
    [SerializeField] private TextMeshProUGUI _currentScoreTest; 
    [SerializeField] private TextMeshProUGUI _bestScoreTest; 
    
    
    private void Awake()
    {
        GameplayEvents.OnCreatedLevel += GameplayEvents_OnCreatedLevel; 
        UIEvents.OpenGameOverPanel += UIEvents_OpenGameOverPanel;
        _playButton.onClick.AddListener(OnClickedPlayButton);
    }

   

    private void OnDestroy()
    {
        GameplayEvents.OnCreatedLevel -= GameplayEvents_OnCreatedLevel; 
        UIEvents.OpenGameOverPanel -= UIEvents_OpenGameOverPanel;
        _playButton.onClick.RemoveListener(OnClickedPlayButton);
    }

    
    private void GameplayEvents_OnCreatedLevel(LevelData arg1, List<GridDotData> arg2)
    {
        _panel.SetActive(false);
    }
    
    private void UIEvents_OpenGameOverPanel(int bestScore, int currentScore)
    {
        _bestScoreTest.text = bestScore.ToString();
        _currentScoreTest.text = currentScore.ToString();
        _playButton.interactable = true;
        _panel.SetActive(true);
    }
    
    private void OnClickedPlayButton()
    {
        _playButton.interactable = false;
        GameplayEvents.PlayAgain?.Invoke();
    }
}
