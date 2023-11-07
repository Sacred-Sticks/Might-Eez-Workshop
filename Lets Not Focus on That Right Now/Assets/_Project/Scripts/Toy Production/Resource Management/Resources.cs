public class Resource
{
    public MaterialType Material { get; protected set; }
    
    public enum MaterialType
    {
        Plastic,
        Metal,
        Wood,
    }
}

public class BaseMaterial : Resource
{
    public BaseMaterial(MaterialType material)
    {
        Material = material;
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
        Torso,
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
