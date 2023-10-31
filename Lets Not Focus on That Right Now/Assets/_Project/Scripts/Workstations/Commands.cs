using System.Threading.Tasks;
using UnityEngine;

public class Dispense : ICommand
{
    public Dispense(int millisecondsDelay, GameObject output)
    {
        MillisecondsDelay = millisecondsDelay;
        CommandOutput = output;
    }

    public int MillisecondsDelay { get; }
    public GameObject CommandOutput { get; set; }

    public async Task Activate(Workstation.Inventory workstationInventory)
    {
        workstationInventory.Inputs = new GameObject[workstationInventory.NumInputs];
        await Task.Delay(MillisecondsDelay);
        workstationInventory.Output = CommandOutput;
    }
}

public class Melt : ICommand
{
    public Melt(int millisecondsDelay)
    {
        MillisecondsDelay = millisecondsDelay;
    }

    public int MillisecondsDelay { get; }
    public GameObject CommandOutput { get; set; }

    public async Task Activate(Workstation.Inventory workstationInventory)
    {
        workstationInventory.Inputs = new GameObject[workstationInventory.NumInputs];
        await Task.Delay(MillisecondsDelay);
        workstationInventory.Output = CommandOutput;
    }
}

public class Mold : ICommand
{
    public Mold(int millisecondsDelay)
    {
        MillisecondsDelay = millisecondsDelay;
    }

    public int MillisecondsDelay { get; }
    public GameObject CommandOutput { get; set; }

    public async Task Activate(Workstation.Inventory workstationInventory)
    {
        workstationInventory.Inputs = new GameObject[workstationInventory.NumInputs];
        await Task.Delay(MillisecondsDelay);
        workstationInventory.Output = CommandOutput;
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
    private GameObject[] inputs;

    public async Task Activate(Workstation.Inventory workstationInventory)
    {
        (inputs, workstationInventory.Inputs) = (workstationInventory.Inputs, new GameObject[workstationInventory.Inputs.Length]);
        await Task.Delay(MillisecondsDelay);
        workstationInventory.Output = CommandOutput;
    }
}
