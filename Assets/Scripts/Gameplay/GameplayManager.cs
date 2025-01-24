using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] private GridAreaController _gridAreaController;
    
    private void Start()
    {
        GameplayEvents.PlayAgain += GameplayEvents_PlayAgain;
    }

    private void OnDestroy()
    {
        GameplayEvents.PlayAgain -= GameplayEvents_PlayAgain;
    }

    private void GameplayEvents_PlayAgain()
    {
        _gridAreaController.PlayAgain();
    }
}