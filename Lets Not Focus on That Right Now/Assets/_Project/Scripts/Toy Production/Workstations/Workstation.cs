using UnityEngine;

public class Workstation : MonoBehaviour
{
    [SerializeField] private WorkstationCategory workstationType;
    [SerializeField] private BaseMaterial.MaterialType materialType;
    [Space]
    [SerializeField] private ToyPart.ToySection toyPart;

    public WorkstationCategory WorkstationType => workstationType;

    public enum WorkstationCategory
    {
        Dispenser,
        Melter,
        Molder,
        Assembler,
    }

    private ICommand command;
    private Inventory workstationInventory;
    
    #region Workstation Delays
    private const int dispenseDelay = 0;
    private const int meltDelay = 0;
    private const int moldDelay = 0;
    private const int assembleDelay = 0;
    #endregion

    #region Unity Events
    private void Awake()
    {
        command = WorkstationType switch
        {
            WorkstationCategory.Dispenser => new Dispense(dispenseDelay, materialType),
            WorkstationCategory.Melter => new ProcessMaterial(meltDelay, materialType),
            WorkstationCategory.Molder => new MoldMaterial(moldDelay, toyPart, materialType),
            WorkstationCategory.Assembler => new Assemble(assembleDelay, materialType),
            _ => command,
        };
        
    }
    #endregion

    public void Activate()
    {
        command?.Activate(workstationInventory);
    }

    public Resource TakeInventory()
    {
        var output = workstationInventory.Output;
        workstationInventory.Output = null;
        return output;
    }

    public class Inventory
    {
        public Inventory(int numInputs)
        {
            NumInputs = numInputs;
            Inputs = new Resource[NumInputs];
        }

        public int NumInputs { get; }
        public Resource[] Inputs { get; set; }
        public Resource Output { get; set; }
    }
}
