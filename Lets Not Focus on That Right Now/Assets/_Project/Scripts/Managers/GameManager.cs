using UnityEngine;
using Kickstarter.Inputs;
using Kickstarter.Variables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private StringVariable endgameStatus;
    [SerializeField] private int endgameSceneIndex;

    private const string gameWin = "You successfully maintained your business!";
    private const string gameLose = "You failed to keep your order count under 9";
    
    public enum EndGameStatus
    {
        Win,
        Lose,
    }
    
    public static GameManager instance { get; private set; }
    
    private void Awake()
    {
        if (!InitializeSingleton())
            return;
        inputManager.Initialize(out int numPlayers);
        OrderManager.ClearCustomers();
    }

    private bool InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            return true;
        }
        Destroy(gameObject);
        return false;
    }

    public void EndGame(EndGameStatus status)
    {
        endgameStatus.Value = status switch
        {
            EndGameStatus.Win => gameWin,
            EndGameStatus.Lose => gameLose,
            _ => endgameStatus.Value,
        };
        LoadSceneByIndex(endgameSceneIndex);
    }

    public static void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
} 