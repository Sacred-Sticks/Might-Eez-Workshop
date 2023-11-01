using UnityEngine;

public class Workstation : MonoBehaviour
{
    [SerializeField] private WorkstationType workstationType;

    public enum WorkstationType
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
        command = workstationType switch  // remove code smell here
        {
            WorkstationType.Dispenser => new Dispense<Plastic>(dispenseDelay),
            WorkstationType.Melter => new ProcessMaterial<Plastic>(meltDelay),
            WorkstationType.Molder => new MoldMaterial<Plastic>(moldDelay, ToyPart<Plastic>.ToySection.Arm),
            WorkstationType.Assembler => new Assemble<Plastic>(assembleDelay),
            _ => command,
        };
        
    }
    #endregion

    public void Activate()
    {
        if (command == null)
            return;
        command.Activate(workstationInventory);
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
