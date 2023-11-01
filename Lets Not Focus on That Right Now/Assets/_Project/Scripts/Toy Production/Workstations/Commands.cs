using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public interface ICommand
{
    public int MillisecondsDelay { get; }

    public Task Activate(Workstation.Inventory inventory);
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

    public async Task Activate(Workstation.Inventory inventory)
    {
        await Task.Delay(MillisecondsDelay);
        inventory.SetOutput(ToyFactory.CreateMaterial(material));
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

    public async Task Activate(Workstation.Inventory inventory)
    {
        input = inventory.GetInputs<BaseMaterial>().FirstOrDefault();
        inventory.ClearInputs<BaseMaterial>();
        if (input.Material != material)
            return;
        await Task.Delay(MillisecondsDelay);
        inventory.SetOutput(ToyFactory.ProcessMaterial(input));
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

    public async Task Activate(Workstation.Inventory inventory)
    {
        input = inventory.GetInputs<ProcessedMaterial>().FirstOrDefault();
        inventory.ClearInputs<ProcessedMaterial>();
        if (input.Material != material)
            return;
        await Task.Delay(MillisecondsDelay);
        inventory.SetOutput(ToyFactory.MoldToy(input, toyPart));
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

    public async Task Activate(Workstation.Inventory inventory)
    {
        inputs = inventory.GetInputs<ToyPart>().ToArray();
        inventory.ClearInputs<ToyPart>();
        if (inputs.Any(toyPart => toyPart.Material != material))
            return;
        await Task.Delay(MillisecondsDelay);
        inventory.SetOutput(ToyFactory.AssembleToy(inputs));
    }
}
