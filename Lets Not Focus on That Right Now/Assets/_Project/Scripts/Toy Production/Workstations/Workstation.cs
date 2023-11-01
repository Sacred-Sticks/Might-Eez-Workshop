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
    public WorkstationInventory Inventory { get; private set; }
    
    #region Workstation Delays
    private const int dispenseDelay = 1000;
    private const int meltDelay = 1000;
    private const int moldDelay = 1000;
    private const int assembleDelay = 1000;
    #endregion

    #region Unity Events
    private void Awake()
    {
        int inputCount;
        switch (workstationType)
        {
            case  WorkstationCategory.Dispenser:
                command = new Dispense(dispenseDelay, materialType);
                inputCount = 0;
                Inventory = new WorkstationInventory<Resource, BaseMaterial>(inputCount);
                break;
            case WorkstationCategory.Processor:
                command = new ProcessMaterial(meltDelay, materialType);
                inputCount = 1;
                Inventory = new WorkstationInventory<BaseMaterial, ProcessedMaterial>(inputCount);
                break;
            case WorkstationCategory.Molder:
                command = new MoldMaterial(moldDelay, toyPart, materialType);
                inputCount = 1;
                Inventory = new WorkstationInventory<ProcessedMaterial, ToyPart>(inputCount);
                break;
            case WorkstationCategory.Assembler:
                command = new Assemble(assembleDelay, materialType);
                inputCount = numToyParts;
                Inventory = new WorkstationInventory<ToyPart, Toy>(inputCount);
                break;
        }
    }
    #endregion

    public void Activate()
    {
        command?.Activate(Inventory);
    }
}
