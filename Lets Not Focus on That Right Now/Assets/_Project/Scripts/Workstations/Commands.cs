using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public interface ICommand
{
    public int MillisecondsDelay { get; }
    
    public Task Activate(Workstation.Inventory workstationInventory);
}

public class Dispense<T> : ICommand where T : BaseMaterial
{
    public Dispense(int millisecondsDelay)
    {
        MillisecondsDelay = millisecondsDelay;
    }

    public int MillisecondsDelay { get; }

    public async Task Activate(Workstation.Inventory workstationInventory)
    {
        await Task.Delay(MillisecondsDelay);
        workstationInventory.Output = ToyFactory.CreateMaterial<T>();
    }
}

public class ProcessMaterial<T> : ICommand where T : BaseMaterial
{
    public ProcessMaterial(int millisecondsDelay)
    {
        MillisecondsDelay = millisecondsDelay;
    }

    public int MillisecondsDelay { get; }
    private Resource input;

    public async Task Activate(Workstation.Inventory workstationInventory)
    {
        (input, workstationInventory.Inputs) = (workstationInventory.Inputs.FirstOrDefault(), new Resource[workstationInventory.Inputs.Length]);
        if (input is not BaseMaterial baseMaterial)
            return;
        await Task.Delay(MillisecondsDelay);
        workstationInventory.Output = ToyFactory.ProcessMaterial<T>(baseMaterial);
    }
}

public class MoldMaterial<T> : ICommand where T : BaseMaterial
{
    public MoldMaterial(int millisecondsDelay, ToyPart<T>.ToySection toySection)
    {
        MillisecondsDelay = millisecondsDelay;
        toyPart = toySection;
    }

    public int MillisecondsDelay { get; }
    private ToyPart<T>.ToySection toyPart;
    private Resource input;

    public async Task Activate(Workstation.Inventory workstationInventory)
    {
        (input, workstationInventory.Inputs) = (workstationInventory.Inputs.FirstOrDefault(), new Resource[workstationInventory.Inputs.Length]);
        if (input is not ProcessedMaterial<T> processedMaterial)
            return;
        await Task.Delay(MillisecondsDelay);
        workstationInventory.Output = ToyFactory.MoldToy(processedMaterial, toyPart);
    }
}

public class Assemble<T> : ICommand where T : BaseMaterial
{
    public Assemble(int millisecondsDelay)
    {
        MillisecondsDelay = millisecondsDelay;
    }

    public int MillisecondsDelay { get; }
    public GameObject CommandOutput { get; set; }
    private Resource[] inputs;

    public async Task Activate(Workstation.Inventory workstationInventory)
    {
        (inputs, workstationInventory.Inputs) = (workstationInventory.Inputs, new Resource[workstationInventory.Inputs.Length]);
        if (inputs is not ToyPart<T>[] toyParts)
            return;
        await Task.Delay(MillisecondsDelay);
        workstationInventory.Output = ToyFactory.AssembleToy(toyParts);
    }
}
