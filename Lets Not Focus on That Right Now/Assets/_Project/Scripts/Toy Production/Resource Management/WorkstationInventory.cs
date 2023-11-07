using System.Collections.Generic;
using Unity.VisualScripting;

public abstract class WorkstationInventory
{
    protected WorkstationInventory(int numInputs, Resource.MaterialType materialType)
    {
        NumInputs = numInputs;
        this.materialType = materialType;
    }

    public static WorkstationInventory<Resource, Resource> CreateDefaultInventory()
    {
        return new WorkstationInventory<Resource, Resource>(0, Resource.MaterialType.Plastic);
    }

    public int NumInputs { get; }
    private Resource.MaterialType materialType { get; }

    public void ClearInputs<T>() where T : Resource
    {
        var inventoryType = GetType();
        var inputProperty = inventoryType.GetProperty("Inputs");
        var inputValue = (List<T>)inputProperty.GetValue(this);
        inputValue.Clear();
    }

    public bool AddInput<T>(T resource) where T : Resource
    {
        var inputs = GetInputs<T>();
        if (inputs == null)
            return false;
        if (inputs.Count >= NumInputs)
            return false;
        var resourceType = resource.GetType();
        if (resourceType == typeof(Toy) || resourceType == typeof(ToyPart))
            return AddInput();
        return resource.Material == materialType && AddInput();

        bool AddInput()
        {
            inputs.Add(resource);
            return true;
        }
    }

    public List<T> GetInputs<T>() where T : Resource
    {
        var type = typeof(T);
        var inventoryType = GetType();
        var genericType = inventoryType.GetGenericArguments()[0];
        if (genericType != typeof(T))
            return null;
        var inputProperty = inventoryType.GetProperty("Inputs");
        var inputValue = (List<T>)inputProperty.GetValue(this);
        return inputValue;
    }

    public void SetOutput<T>(T output) where T : Resource
    {
        var inventoryType = GetType();
        var outputProperty = inventoryType.GetProperty("Output");
        var outputValue = (T)outputProperty.GetValue(this);
        outputProperty.SetValue(this, output);
    }

    public T TakeOutput<T>() where T : Resource
    {
        var inventoryType = GetType();
        var outputProperty = inventoryType.GetProperty("Output");
        var outputValue = (T)outputProperty.GetValue(this);
        return outputValue;
    }
}

public sealed class WorkstationInventory<TInput, TOutput> : WorkstationInventory where TInput : Resource where TOutput : Resource
{
    public WorkstationInventory(int numInputs, Resource.MaterialType materialType) : base(numInputs, materialType)
    {
        Inputs = new List<TInput>();
    }

    public List<TInput> Inputs { get; }
    public TOutput Output { get; set; }
}
