using System.Linq;

public static class ToyFactory
{
    private const int dispenseDelay = 10000;
    private const int processDelay = 5000;
    private const int constructDelay = 3000;
    private const int assembleDelay = 10000;
    private const int outputDelay = 0;
    private const int garbageDelay = 0;
    
    public static ICommand CreateWorkstationCommand(Workstation.WorkstationCategory workstationType, Resource.MaterialType material, Resource.MaterialColor color,
        ToyPart.ToySection toyPart)
    {
        return workstationType switch
        {
            Workstation.WorkstationCategory.Dispenser => new Dispense(dispenseDelay, material, color),
            Workstation.WorkstationCategory.Processor => new ProcessMaterial(processDelay, material),
            Workstation.WorkstationCategory.Constructor => new ConstructMaterial(constructDelay, toyPart, material),
            Workstation.WorkstationCategory.Assembler => new Assemble(assembleDelay, material),
            Workstation.WorkstationCategory.Output => new Output(outputDelay),
            Workstation.WorkstationCategory.Garbage => new Garbage(garbageDelay),
            _ => ICommand.CreateDefaultCommand(),
        };
    }

    public static WorkstationInventory CreateWorkstationInventory(Workstation.WorkstationCategory workstationType, int numToyParts, Resource.MaterialType materialType)
    {
        WorkstationInventory inventory = workstationType switch
        {
            Workstation.WorkstationCategory.Dispenser => new WorkstationInventory<Resource, BaseMaterial>(0, materialType),
            Workstation.WorkstationCategory.Processor => new WorkstationInventory<BaseMaterial, ProcessedMaterial>(1, materialType),
            Workstation.WorkstationCategory.Constructor => new WorkstationInventory<ProcessedMaterial, ToyPart>(1, materialType),
            Workstation.WorkstationCategory.Assembler => new WorkstationInventory<ToyPart, Toy>(numToyParts, materialType),
            Workstation.WorkstationCategory.Output => new WorkstationInventory<Toy, Resource>(numToyParts, materialType),
            Workstation.WorkstationCategory.Garbage => new WorkstationInventory<Resource, Resource>(1, materialType),
            _ => WorkstationInventory.CreateDefaultInventory(),
        };
        return inventory;
    }

    public static BaseMaterial CreateMaterial(Resource.MaterialType material, Resource.MaterialColor color)
    {
        return new BaseMaterial(material, color);
    }

    public static ProcessedMaterial ProcessMaterial(BaseMaterial baseMaterial)
    {
        return new ProcessedMaterial(baseMaterial.Material, baseMaterial.Color);
    }

    public static ToyPart ConstructToyPart(ProcessedMaterial processedMaterial, ToyPart.ToySection toySection)
    {
        return new ToyPart(processedMaterial.Material, processedMaterial.Color, toySection);
    }

    public static Toy AssembleToy(ToyPart[] toyParts)
    {
        return new Toy(toyParts, toyParts.FirstOrDefault().Material);
    }

    public static bool OutputToy(Toy toy)
    {
        return OrderManager.FillOrder(toy);
    }
}
