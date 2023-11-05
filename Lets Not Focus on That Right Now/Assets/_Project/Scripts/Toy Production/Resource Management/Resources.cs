public class Resource
{
    
}

public class BaseMaterial : Resource
{
    public BaseMaterial(MaterialType material)
    {
        Material = material;
    }
    
    public MaterialType Material { get; }
    
    public enum MaterialType
    {
        Plastic,
        Metal,
        Wood,
    }
}

public class ProcessedMaterial : BaseMaterial
{
    public ProcessedMaterial(MaterialType material) : base(material) { }
}

public class ToyPart : BaseMaterial
{
    public ToyPart(MaterialType material, ToySection part) : base(material)
    {
        Part = part;
    }

    public ToySection Part { get; }
    
    public enum ToySection
    {
        Arm,
        Body,
        Leg,
        Head,
    }
}

public class Toy : BaseMaterial
{
    public Toy(ToyPart[] toyParts, MaterialType material) : base(material)
    {
        ToyParts = toyParts;
    }
    
    public ToyPart[] ToyParts { get; }
}
