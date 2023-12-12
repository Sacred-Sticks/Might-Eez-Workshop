using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;
using UnityEngine.UIElements;

public class TriggerPause : MonoBehaviour, IInputReceiver
{
    [SerializeField] private FloatInput pauseInput;
    [SerializeField] private UIDocument pauseMenu;

    public bool SubscribeToInputs(Player player)
    {
        return pauseInput.SubscribeToInputAction(OnPauseInputChange, player.PlayerID);
    }

    public bool UnsubscribeToInputs(Player player)
    {
        return pauseInput.UnsubscribeToInputAction(OnPauseInputChange, player.PlayerID);
    }

    private void OnPauseInputChange(float input)
    {
        if (input == 0)
            return;
        pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeInHierarchy);
    }
}
