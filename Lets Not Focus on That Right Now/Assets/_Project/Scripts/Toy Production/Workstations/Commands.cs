using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public interface ICommand
{
    public int MillisecondsDelay { get; }

    public Task Activate(WorkstationInventory workstationInventory);

    public static ICommand CreateDefaultCommand()
    {
        return new DefaultCommand();
    }
}

public class DefaultCommand : ICommand
{
    public int MillisecondsDelay { get; }
    public async Task Activate(WorkstationInventory workstationInventory)
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

    public async Task Activate(WorkstationInventory workstationInventory)
    {
        await Task.Delay(MillisecondsDelay);
        workstationInventory.SetOutput(ToyFactory.CreateMaterial(material));
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

    public async Task Activate(WorkstationInventory workstationInventory)
    {
        input = workstationInventory.GetInputs<BaseMaterial>().FirstOrDefault();
        workstationInventory.ClearInputs<BaseMaterial>();
        if (input.Material != material)
            return;
        await Task.Delay(MillisecondsDelay);
        workstationInventory.SetOutput(ToyFactory.ProcessMaterial(input));
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

    public async Task Activate(WorkstationInventory workstationInventory)
    {
        input = workstationInventory.GetInputs<ProcessedMaterial>().FirstOrDefault();
        workstationInventory.ClearInputs<ProcessedMaterial>();
        if (input.Material != material)
            return;
        await Task.Delay(MillisecondsDelay);
        workstationInventory.SetOutput(ToyFactory.MoldToy(input, toyPart));
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

    public async Task Activate(WorkstationInventory workstationInventory)
    {
        inputs = workstationInventory.GetInputs<ToyPart>().ToArray();
        workstationInventory.ClearInputs<ToyPart>();
        if (inputs.Any(toyPart => toyPart.Material != material))
            return;
        await Task.Delay(MillisecondsDelay);
        workstationInventory.SetOutput(ToyFactory.AssembleToy(inputs));
    }
}
