using UnityEngine;

public class Workstation : MonoBehaviour
{
    [SerializeField] private WorkstationCategory workstationType;
    [SerializeField] private BaseMaterial.MaterialType materialType;
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
    }

    private ICommand command;
    public WorkstationInventory Inventory { get; private set; }
    public bool WorkstationActive { get; set; }

    #region Workstation Delays
    private const int dispenseDelay = 1000;
    private const int meltDelay = 1000;
    private const int moldDelay = 1000;
    private const int assembleDelay = 1000;
    private const int outputDelay = 0;
    #endregion

    #region Unity Events
    private void Awake()
    {
        command = ToyFactory.CreateWorkstationCommand(workstationType, materialType, toyPart, 
            (dispenseDelay, meltDelay, moldDelay, assembleDelay, outputDelay));
        Inventory = ToyFactory.CreateWorkstationInventory(workstationType, numToyParts, materialType);
    }
    #endregion

    public void Activate()
    {
        if (!WorkstationActive)
            command?.Activate(this);
    }
}
