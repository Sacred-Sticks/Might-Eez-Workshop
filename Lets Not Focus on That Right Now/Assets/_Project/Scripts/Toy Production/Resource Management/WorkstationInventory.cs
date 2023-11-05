using System.Collections.Generic;

public abstract class WorkstationInventory
{
    protected WorkstationInventory(int numInputs)
    {
        NumInputs = numInputs;
    }

    public static WorkstationInventory<Resource, Resource> CreateDefaultInventory()
    {
        return new WorkstationInventory<Resource, Resource>(0);
    }
        
    public int NumInputs { get; }

    public void ClearInputs<T>()
    {
        var inventoryType = GetType();
        var inputProperty = inventoryType.GetProperty("Inputs");
        var inputValue = (List<T>)inputProperty.GetValue(this);
        inputValue.Clear();
    }
        
    public bool AddInput<T>(T resource)
    {
        var inputs = GetInputs<T>();
        if (inputs == null)
            return false;
        if (inputs.Count >= NumInputs)
            return false;
        inputs.Add(resource);
        return true;
    }

    public List<T> GetInputs<T>()
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

    public void SetOutput<T>(T output)
    {
        var inventoryType = GetType();
        var outputProperty = inventoryType.GetProperty("Output");
        var outputValue = (T)outputProperty.GetValue(this);
        outputProperty.SetValue(this, output);
    }

    public T TakeOutput<T>()
    {
        var inventoryType = GetType();
        var outputProperty = inventoryType.GetProperty("Output");
        var outputValue = (T)outputProperty.GetValue(this);
        return outputValue;
    }
}

public sealed class WorkstationInventory<TInput, TOutput> : WorkstationInventory where TInput : Resource where TOutput : Resource
{
    public WorkstationInventory(int numInputs) : base(numInputs)
    {
        Inputs = new List<TInput>();
    }
        
    public List<TInput> Inputs { get; }
    public TOutput Output { get; set; }
}
