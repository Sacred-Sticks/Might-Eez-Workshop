using System;
using Kickstarter.Events;
using Kickstarter.Identification;
using Kickstarter.Inputs;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(WorkstationInteractor))]
public class ResourceCarrier : MonoBehaviour, IInputReceiver
{
    [SerializeField] private FloatInput resourceExchangeInput;
    [Space]
    [Header("Outgoing Events")]
    [SerializeField] private Service showCurrentResource;

    private WorkstationInteractor workstationInteractor;

    private Resource resource = new Resource();
    public Resource Resource
    {
        get => resource;
        private set
        {
            resource = value;
            showCurrentResource.Trigger(new ShowResource(resource));
        }
    }

    #region Unity Events
    private void Awake()
    {
        workstationInteractor = GetComponent<WorkstationInteractor>();
    }

    private void Start()
    {
        resource = new Resource();
    }
    #endregion
    
    #region Inputs
    public bool SubscribeToInputs(Player player)
    {
        return resourceExchangeInput.SubscribeToInputAction(OnInteractInputChange, player.PlayerID);
    }

    public bool UnsubscribeToInputs(Player player)
    {
        return resourceExchangeInput.UnsubscribeToInputAction(OnInteractInputChange, player.PlayerID);
    }

    private void OnInteractInputChange(float input)
    {
        if (input < 1)
            return;

        var workstation = workstationInteractor.FindWorkstation();
        if (workstation == null)
            return;
        
        if (resource.GetType() == typeof(Resource))
        {
            TakeResource(workstation);
            workstationInteractor.Interact(workstation);
            return;
        }
        GiveResource(workstation);
        workstationInteractor.Interact(workstation);
    }
    #endregion

    private void TakeResource(Workstation workstation)
    {
        if (workstation.WorkstationActive == Workstation.Status.Active)
            return;
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
        }
        resource ??= new Resource();
        Resource = resource;
        workstation.WorkstationActive = Workstation.Status.Idle;
    }

    private void GiveResource(Workstation workstation)
    {
        if (workstation.Inventory.InputType == typeof(Resource))
        {
            workstation.Inventory.AddResource(resource);
            Resource = new Resource();
            return;
        }
        bool giveSuccessful = resource switch
        {
            Toy toy => workstation.Inventory.AddInput(toy),
            ToyPart toyPart => workstation.Inventory.AddInput(toyPart),
            ProcessedMaterial processedMaterial => workstation.Inventory.AddInput(processedMaterial),
            BaseMaterial baseMaterial => workstation.Inventory.AddInput(baseMaterial),
            _ => throw new ArgumentOutOfRangeException(),
        };
        if (!giveSuccessful)
            return;
        Resource = new Resource();
    }

    #region Events
    public class ShowResource : EventArgs
    {
        public ShowResource(Resource resource)
        {
            Resource = resource;
        }
        
        public Resource Resource { get; }
    }
    #endregion
}
