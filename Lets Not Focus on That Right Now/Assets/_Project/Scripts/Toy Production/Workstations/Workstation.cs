using System.Collections;
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
    public Inventory WorkstationInventory { get; private set; }
    
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
                WorkstationInventory = new Inventory<Resource, BaseMaterial>(inputCount);
                break;
            case WorkstationCategory.Processor:
                command = new ProcessMaterial(meltDelay, materialType);
                inputCount = 1;
                WorkstationInventory = new Inventory<BaseMaterial, ProcessedMaterial>(inputCount);
                break;
            case WorkstationCategory.Molder:
                command = new MoldMaterial(moldDelay, toyPart, materialType);
                inputCount = 1;
                WorkstationInventory = new Inventory<ProcessedMaterial, ToyPart>(inputCount);
                break;
            case WorkstationCategory.Assembler:
                command = new Assemble(assembleDelay, materialType);
                inputCount = numToyParts;
                WorkstationInventory = new Inventory<ToyPart, Toy>(inputCount);
                break;
        }
    }
    #endregion

    public void Activate()
    {
        command?.Activate(WorkstationInventory);
    }

    public abstract class Inventory
    {
        protected Inventory(int numInputs)
        {
            NumInputs = numInputs;
        }
        
        public int NumInputs { get; }

        public void ClearInputs<T>()
        {
            var inventoryType = GetType();
            var inputProperty = inventoryType.GetProperty("Inputs");
            var inputValue = (List<T>)inputProperty.GetValue(this);
            inputValue.Clear();
        }
        
        public void AddInput<T>(T resource)
        {
            var inputs = GetInputs<T>();
            if (inputs.Count < NumInputs)
                inputs.Add(resource);
        }

        public List<T> GetInputs<T>()
        {
            var inventoryType = GetType();
            var inputProperty = inventoryType.GetProperty("Inputs");
            var inputValue = (List<T>)inputProperty.GetValue(this);
            return inputValue;
        }

        public void SetOutput<T>(T output)
        {
            var inventoryType = GetType();
            var outputProperty = inventoryType.GetProperty("Output");
            var outputValue = (T)outputProperty.GetValue(this);
            outputProperty.SetValue(this, output);
        }

        public T GetOutput<T>()
        {
            var inventoryType = GetType();
            var outputProperty = inventoryType.GetProperty("Output");
            var outputValue = (T)outputProperty.GetValue(this);
            return outputValue;
        }
    }

    public sealed class Inventory<TInput, TOutput> : Inventory where TInput : Resource where TOutput : Resource
    {
        public Inventory(int numInputs) : base(numInputs)
        {
            Inputs = new List<TInput>();
        }
        
        public List<TInput> Inputs { get; }
        public TOutput Output { get; set; }
    }
}
