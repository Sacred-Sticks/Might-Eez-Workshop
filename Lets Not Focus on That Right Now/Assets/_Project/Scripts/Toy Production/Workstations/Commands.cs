using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public interface ICommand
{
    public int MillisecondsDelay { get; }

    public Task Activate(Workstation workstation);

    public static ICommand CreateDefaultCommand()
    {
        return new DefaultCommand();
    }
}

public class DefaultCommand : ICommand
{
    public int MillisecondsDelay { get; }
    public async Task Activate(Workstation workstation)
    {
        Debug.LogWarning("Default Command Used");
    }
}

public class Dispense : ICommand
{
    public Dispense(int millisecondsDelay, BaseMaterial.MaterialType materialType)
    {
        MillisecondsDelay = millisecondsDelay;
        material = materialType;
    }

    public int MillisecondsDelay { get; }
    private BaseMaterial.MaterialType material { get; }

    public async Task Activate(Workstation workstation)
    {
        workstation.WorkstationActive = true;
        await Task.Delay(MillisecondsDelay);
        workstation.Inventory.SetOutput(ToyFactory.CreateMaterial(material));
        workstation.WorkstationActive = false;
    }
}

public class ProcessMaterial : ICommand
{
    public ProcessMaterial(int millisecondsDelay, BaseMaterial.MaterialType materialType)
    {
        MillisecondsDelay = millisecondsDelay;
        material = materialType;
    }

    public int MillisecondsDelay { get; }
    private BaseMaterial.MaterialType material { get; }
    private BaseMaterial input;

    public async Task Activate(Workstation workstation)
    {
        input = workstation.Inventory.GetInputs<BaseMaterial>().FirstOrDefault();
        workstation.Inventory.ClearInputs<BaseMaterial>();
        if (input.Material != material)
            return;
        workstation.WorkstationActive = true;
        await Task.Delay(MillisecondsDelay);
        workstation.Inventory.SetOutput(ToyFactory.ProcessMaterial(input));
        workstation.WorkstationActive = false;
    }
}

public class MoldMaterial : ICommand
{
    public MoldMaterial(int millisecondsDelay, ToyPart.ToySection toySection, BaseMaterial.MaterialType materialType)
    {
        MillisecondsDelay = millisecondsDelay;
        toyPart = toySection;
        material = materialType;
    }

    public int MillisecondsDelay { get; }
    private ToyPart.ToySection toyPart { get; }
    private BaseMaterial.MaterialType material { get; }
    private ProcessedMaterial input;

    public async Task Activate(Workstation workstation)
    {
        input = workstation.Inventory.GetInputs<ProcessedMaterial>().FirstOrDefault();
        workstation.Inventory.ClearInputs<ProcessedMaterial>();
        if (input.Material != material)
            return;
        workstation.WorkstationActive = true;
        await Task.Delay(MillisecondsDelay);
        workstation.Inventory.SetOutput(ToyFactory.MoldToy(input, toyPart));
        workstation.WorkstationActive = false;
    }
}

public class Assemble : ICommand
{
    public Assemble(int millisecondsDelay, BaseMaterial.MaterialType materialType)
    {
        MillisecondsDelay = millisecondsDelay;
        material = materialType;
    }

    public int MillisecondsDelay { get; }
    private BaseMaterial.MaterialType material { get; }
    private ToyPart[] inputs;

    public async Task Activate(Workstation workstation)
    {
        inputs = workstation.Inventory.GetInputs<ToyPart>().ToArray();
        if (inputs.Length != workstation.Inventory.NumInputs)
            return;
        workstation.Inventory.ClearInputs<ToyPart>();
        if (inputs.Any(toyPart => toyPart.Material != material))
            return;
        workstation.WorkstationActive = true;
        await Task.Delay(MillisecondsDelay);
        workstation.Inventory.SetOutput(ToyFactory.AssembleToy(inputs));
        workstation.WorkstationActive = false;
    }
}
