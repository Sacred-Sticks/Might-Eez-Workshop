using System;
using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class ResourceCarrier : MonoBehaviour, IInputReceiver
{
    [SerializeField] private FloatInput interactInput;
    [Space]
    [SerializeField] private float maxInteractionDistance;

    public Resource resource { get; private set; }

    #region Unity Events
    private void Start()
    {
        resource = new Resource();
    }
    #endregion
    
    #region Inputs
    public void SubscribeToInputs(Player player)
    {
        interactInput.SubscribeToInputAction(OnInteractInputChange, player.PlayerID);
    }

    public void UnsubscribeToInputs(Player player)
    {
        interactInput.UnsubscribeToInputAction(OnInteractInputChange, player.PlayerID);
    }

    private void OnInteractInputChange(float input)
    {
        if (input < 1)
            return;

        var workstation = FindWorkstation();
        if (workstation == null)
            return;
        
        if (resource.GetType() == typeof(Resource))
        {
            TakeResource(workstation);
            return;
        }
        GiveResource(workstation);
    }
    #endregion

    private Workstation FindWorkstation()
    {
        var ray = new Ray(transform.position, transform.forward);
        if (!Physics.Raycast(ray, out var hit, maxInteractionDistance))
            return null;
        hit.transform.TryGetComponent(out Workstation workstation);
        return workstation;
    }

    private void TakeResource(Workstation workstation)
    {
        var genericType = workstation.Inventory.GetType().GetGenericArguments()[1];
        switch (genericType)
        {
            case {} when genericType == typeof(BaseMaterial):
                resource = workstation.Inventory.TakeOutput<BaseMaterial>();
                workstation.Inventory.SetOutput<BaseMaterial>(null);
                break;
            case {} when genericType == typeof(ProcessedMaterial):
                resource = workstation.Inventory.TakeOutput<ProcessedMaterial>();
                workstation.Inventory.SetOutput<ProcessedMaterial>(null);
                break;
            case {} when genericType == typeof(ToyPart):
                resource = workstation.Inventory.TakeOutput<ToyPart>();
                workstation.Inventory.SetOutput<ToyPart>(null);
                break;
            case {} when genericType == typeof(Toy):
                resource = workstation.Inventory.TakeOutput<Toy>();
                workstation.Inventory.SetOutput<Toy>(null);
                break;
            default:
                throw new NotImplementedException();
        }
        resource ??= new Resource();
        Debug.Log($"New Resource Acquired: {resource.GetType()}");
    }

    private void GiveResource(Workstation workstation)
    {
        bool giveSuccessful = true;
        switch (resource)
        {
            case Toy toy:
                giveSuccessful = workstation.Inventory.AddInput(toy);
                break;
            case ToyPart toyPart:
                giveSuccessful = workstation.Inventory.AddInput(toyPart);
                break;
            case ProcessedMaterial processedMaterial:
                giveSuccessful =  workstation.Inventory.AddInput(processedMaterial);
                break;
            case BaseMaterial baseMaterial:
                giveSuccessful = workstation.Inventory.AddInput(baseMaterial);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        resource = new Resource();
        if (!giveSuccessful)
            return;
        workstation.Activate();
    }
}
