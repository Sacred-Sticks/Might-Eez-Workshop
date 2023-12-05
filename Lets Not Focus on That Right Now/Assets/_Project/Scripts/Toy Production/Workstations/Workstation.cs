using Kickstarter.Observer;
using UnityEngine;

[SelectionBase]
public class Workstation : Observable
{
    [SerializeField] private WorkstationCategory workstationType;
    [SerializeField] private Resource.MaterialType materialType;
    [SerializeField] private Resource.MaterialColor materialColor;
    [Space]
    [SerializeField] private ToyPart.ToySection toyPart;
    [Space]
    [SerializeField] private int numToyParts = 1;

    public WorkstationCategory WorkstationType => workstationType;

    public enum WorkstationCategory
    {
        Dispenser,
        Processor,
        Constructor,
        Assembler,
        Output,
        Garbage,
    }

    public enum Status
    {
        Idle,
        Active,
        Completed,
    }

    private ICommand command;
    public WorkstationInventory Inventory { get; private set; }
    private Status workstationActive;
    public Status WorkstationActive
    {
        get => workstationActive;
        set
        {
            if (workstationActive == value)
                return;
            workstationActive = value;
            NotifyObservers(workstationActive);
        }
    }

    #region Unity Events
    private void Awake()
    {
        command = ToyFactory.CreateWorkstationCommand(workstationType, materialType, materialColor, toyPart);
        Inventory = ToyFactory.CreateWorkstationInventory(workstationType, numToyParts, materialType);
    }

    private void Start()
    {
        NotifyObservers(Status.Idle);
    }
    #endregion

    public void Activate()
    {
        if (WorkstationActive != Status.Active)
            command?.Activate(this);
    }
}