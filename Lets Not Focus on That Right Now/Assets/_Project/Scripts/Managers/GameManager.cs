using UnityEngine;
using Kickstarter.Inputs;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;

    private void Awake()
    {
        inputManager.Initialize(out int numPlayers);
    }
} 