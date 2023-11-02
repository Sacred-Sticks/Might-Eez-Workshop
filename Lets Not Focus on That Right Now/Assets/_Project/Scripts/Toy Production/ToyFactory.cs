using System.Linq;

public static class ToyFactory
{
    public static ICommand CreateWorkstationCommand(Workstation.WorkstationCategory workstationType, BaseMaterial.MaterialType materialType, ToyPart.ToySection toyPart,
        (int dispense, int process, int mold, int assemble) delays)
    {
        var command = workstationType switch
        {
            Workstation.WorkstationCategory.Dispenser => new Dispense(delays.dispense, materialType),
            Workstation.WorkstationCategory.Processor => new ProcessMaterial(delays.process, materialType),
            Workstation.WorkstationCategory.Molder => new MoldMaterial(delays.mold, toyPart, materialType),
            Workstation.WorkstationCategory.Assembler => new Assemble(delays.assemble, materialType),
            _ => ICommand.CreateDefaultCommand(),
        };
        return command;
    }

    public static WorkstationInventory CreateWorkstationInventory(Workstation.WorkstationCategory workstationType, int numToyParts)
    {
        WorkstationInventory inventory = workstationType switch
        {
            Workstation.WorkstationCategory.Dispenser => new WorkstationInventory<Resource, BaseMaterial>(0),
            Workstation.WorkstationCategory.Processor => new WorkstationInventory<BaseMaterial, ProcessedMaterial>(1),
            Workstation.WorkstationCategory.Molder => new WorkstationInventory<ProcessedMaterial, ToyPart>(1),
            Workstation.WorkstationCategory.Assembler => new WorkstationInventory<ToyPart, Toy>(numToyParts),
            _ => WorkstationInventory.CreateDefaultInventory(),
        };
        return inventory;
    }

    public static BaseMaterial CreateMaterial(BaseMaterial.MaterialType material)
    {
        return new BaseMaterial(material);
    }

    public static ProcessedMaterial ProcessMaterial(BaseMaterial baseMaterial)
    {
        return new ProcessedMaterial(baseMaterial.Material);
    }

    public static ToyPart MoldToy(ProcessedMaterial processedMaterial, ToyPart.ToySection toySection)
    {
        return new ToyPart(processedMaterial.Material, toySection);
    }

    public static Toy AssembleToy(ToyPart[] toyParts)
    {
        return new Toy(toyParts, toyParts.FirstOrDefault().Material);
    }
}
