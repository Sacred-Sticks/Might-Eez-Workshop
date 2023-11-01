using System.Threading.Tasks;
using UnityEngine;

public class Dispense : ICommand
{
    public Dispense(int millisecondsDelay)
    {
        MillisecondsDelay = millisecondsDelay;
    }

    public int MillisecondsDelay { get; }
    private Resource[] inputs;

    public async Task Activate(Workstation.Inventory workstationInventory)
    {
        (inputs, workstationInventory.Inputs) = (workstationInventory.Inputs, new Resource[workstationInventory.Inputs.Length]);
        await Task.Delay(MillisecondsDelay);
    }
}

public class Melt : ICommand
{
    public Melt(int millisecondsDelay)
    {
        MillisecondsDelay = millisecondsDelay;
    }

    public int MillisecondsDelay { get; }
    private Resource[] inputs;

    public async Task Activate(Workstation.Inventory workstationInventory)
    {
        (inputs, workstationInventory.Inputs) = (workstationInventory.Inputs, new Resource[workstationInventory.Inputs.Length]);
        await Task.Delay(MillisecondsDelay);
    }
}

public class Mold : ICommand
{
    public Mold(int millisecondsDelay)
    {
        MillisecondsDelay = millisecondsDelay;
    }

    public int MillisecondsDelay { get; }
    private Resource[] inputs;

    public async Task Activate(Workstation.Inventory workstationInventory)
    {
        (inputs, workstationInventory.Inputs) = (workstationInventory.Inputs, new Resource[workstationInventory.Inputs.Length]);
        await Task.Delay(MillisecondsDelay);
    }
}

public class Assemble : ICommand
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
        await Task.Delay(MillisecondsDelay);
    }
}
