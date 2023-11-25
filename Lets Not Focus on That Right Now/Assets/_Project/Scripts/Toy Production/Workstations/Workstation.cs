using UnityEngine;

[SelectionBase]
public class Workstation : MonoBehaviour
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

    private ICommand command;
    public WorkstationInventory Inventory { get; private set; }
    public bool WorkstationActive { get; set; }

    #region Unity Events
    private void Awake()
    {
        command = ToyFactory.CreateWorkstationCommand(workstationType, materialType, materialColor, toyPart);
        Inventory = ToyFactory.CreateWorkstationInventory(workstationType, numToyParts, materialType);
    }
    #endregion

    public void Activate()
    {
        if (!WorkstationActive)
            command?.Activate(this);
    }
}
