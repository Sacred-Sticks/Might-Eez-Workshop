using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class WorkstationInteractor : MonoBehaviour, IInputReceiver
{
    [SerializeField] private FloatInput interactInput;
    [Space]
    [SerializeField] private float maxInteractionDistance;

    #region Inputs
    public bool SubscribeToInputs(Player player)
    {
        return interactInput.SubscribeToInputAction(OnInteractInputChange, player.PlayerID);
    }

    public bool UnsubscribeToInputs(Player player)
    {
        return interactInput.UnsubscribeToInputAction(OnInteractInputChange, player.PlayerID);
    }

    private void OnInteractInputChange(float input)
    {
        if (input < 1)
            return;

        Interact();
    }
    #endregion

    private void Interact()
    {
        var workstation = FindWorkstation();
        workstation.Activate();
    }
    
    public Workstation FindWorkstation()
    {
        var ray = new Ray(transform.position, transform.forward);
        if (!Physics.Raycast(ray, out var hit, maxInteractionDistance))
            return null;
        hit.transform.TryGetComponent(out Workstation workstation);
        return workstation;
    }
}