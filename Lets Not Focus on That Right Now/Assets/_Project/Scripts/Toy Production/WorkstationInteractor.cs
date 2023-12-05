using Kickstarter.Identification;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class WorkstationInteractor : MonoBehaviour
{
    [SerializeField] private Vector3 interactionOffset;
    [SerializeField] private float maxInteractionDistance;

    public void Interact(Workstation workstation)
    {
        workstation.Activate();
    }
    
    public Workstation FindWorkstation()
    {
        var ray = new Ray(transform.position + interactionOffset, transform.forward);
        if (!Physics.Raycast(ray, out var hit, maxInteractionDistance))
            return null;
        hit.transform.parent.TryGetComponent(out Workstation workstation);
        return workstation;
    }
}