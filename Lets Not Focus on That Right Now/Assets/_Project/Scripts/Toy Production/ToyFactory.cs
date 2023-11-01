using System.Linq;

public static class ToyFactory
{
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
