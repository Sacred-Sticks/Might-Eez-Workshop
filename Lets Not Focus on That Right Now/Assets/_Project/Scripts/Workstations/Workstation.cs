using UnityEngine;

public class Workstation : MonoBehaviour
{
    [SerializeField] private WorkstationType workstationType;
    [SerializeField] private GameObject output; // This is a broken method and must be replaced quickly

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
        command = workstationType switch
        {
            WorkstationType.Dispenser => new Dispense(dispenseDelay),
            WorkstationType.Melter => new Melt(meltDelay),
            WorkstationType.Molder => new Mold(moldDelay),
            WorkstationType.Assembler => new Assemble(assembleDelay),
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
