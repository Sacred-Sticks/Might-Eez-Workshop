using System.Linq;

public static class ToyFactory
{
    public static ICommand CreateWorkstationCommand(Workstation.WorkstationCategory workstationType, Resource.MaterialType material, Resource.MaterialColor color,
        ToyPart.ToySection toyPart, (int dispense, int process, int mold, int assemble, int output) delays)
    {
        return workstationType switch
        {
            Workstation.WorkstationCategory.Dispenser => new Dispense(delays.dispense, material, color),
            Workstation.WorkstationCategory.Processor => new ProcessMaterial(delays.process, material),
            Workstation.WorkstationCategory.Constructor => new MoldMaterial(delays.mold, toyPart, material),
            Workstation.WorkstationCategory.Assembler => new Assemble(delays.assemble, material),
            Workstation.WorkstationCategory.Output => new Output(delays.output),
            _ => ICommand.CreateDefaultCommand(),
        };
    }

    public static WorkstationInventory CreateWorkstationInventory(Workstation.WorkstationCategory workstationType, int numToyParts,
        Resource.MaterialType materialType)
    {
        WorkstationInventory inventory = workstationType switch
        {
            Workstation.WorkstationCategory.Dispenser => new WorkstationInventory<Resource, BaseMaterial>(0, materialType),
            Workstation.WorkstationCategory.Processor => new WorkstationInventory<BaseMaterial, ProcessedMaterial>(1, materialType),
            Workstation.WorkstationCategory.Constructor => new WorkstationInventory<ProcessedMaterial, ToyPart>(1, materialType),
            Workstation.WorkstationCategory.Assembler => new WorkstationInventory<ToyPart, Toy>(numToyParts, materialType),
            Workstation.WorkstationCategory.Output => new WorkstationInventory<Toy, Resource>(numToyParts, materialType),
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

    public static void OutputToy(Toy toy)
    {
        OrderManager.FillOrder(toy);
    }
}
