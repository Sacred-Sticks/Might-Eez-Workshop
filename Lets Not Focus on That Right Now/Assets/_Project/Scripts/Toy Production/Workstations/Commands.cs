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
        inventory.Output = ToyFactory.CreateMaterial(material);
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
    private Resource input;

    public async Task Activate(Workstation.Inventory inventory)
    {
        input = inventory.Inputs.FirstOrDefault();
        inventory.Inputs.Clear();
        if (input is not BaseMaterial baseMaterial)
            return;
        if (baseMaterial.Material != material)
            return;
        await Task.Delay(MillisecondsDelay);
        inventory.Output = ToyFactory.ProcessMaterial(baseMaterial);
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
    private Resource input;

    public async Task Activate(Workstation.Inventory inventory)
    {
        input = inventory.Inputs.FirstOrDefault();
        inventory.Inputs.Clear();
        if (input is not ProcessedMaterial processedMaterial)
            return;
        if (processedMaterial.Material != material)
            return;
        await Task.Delay(MillisecondsDelay);
        inventory.Output = ToyFactory.MoldToy(processedMaterial, toyPart);
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
    private Resource[] inputs;

    public async Task Activate(Workstation.Inventory inventory)
    {
        inputs = inventory.Inputs.ToArray();
        inventory.Inputs.Clear();
        if (inputs is not ToyPart[] toyParts)
            return;
        if (toyParts.Any(toyPart => toyPart.Material != material))
            return;
        await Task.Delay(MillisecondsDelay);
        inventory.Output = ToyFactory.AssembleToy(toyParts);
    }
}
