using System.Collections.Generic;
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
        Molder,
        Assembler,
    }

    private ICommand command;
    private Inventory inventory;
    
    #region Workstation Delays
    private const int dispenseDelay = 1000;
    private const int meltDelay = 1000;
    private const int moldDelay = 1000;
    private const int assembleDelay = 1000;
    #endregion

    #region Unity Events
    private void Awake()
    {
        command = WorkstationType switch
        {
            WorkstationCategory.Dispenser => new Dispense(dispenseDelay, materialType),
            WorkstationCategory.Processor => new ProcessMaterial(meltDelay, materialType),
            WorkstationCategory.Molder => new MoldMaterial(moldDelay, toyPart, materialType),
            WorkstationCategory.Assembler => new Assemble(assembleDelay, materialType),
            _ => command,
        };

        int numInputs = workstationType switch
        {
            WorkstationCategory.Dispenser => 0,
            WorkstationCategory.Assembler => numToyParts,
            _ => 1,
        };
        
        inventory = new Inventory(numInputs);
    }
    #endregion

    public void AddToInventory(Resource resource)
    {
        if (inventory.Inputs.Count < inventory.NumInputs)
            inventory.Inputs.Add(resource);
    }

    public void Activate()
    {
        command?.Activate(inventory);
    }

    public Resource TakeInventory()
    {
        var output = inventory.Output;
        inventory.Output = null;
        return output;
    }

    public class Inventory
    {
        public Inventory(int numInputs)
        {
            Inputs = new List<Resource>();
            NumInputs = numInputs;
        }

        public int NumInputs { get; }
        public List<Resource> Inputs { get; set; }
        public Resource Output { get; set; }
    }
}
