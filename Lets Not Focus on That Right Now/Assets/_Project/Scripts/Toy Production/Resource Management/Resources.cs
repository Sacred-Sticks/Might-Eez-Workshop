public class Resource
{
    public Resource() { }

    protected Resource(MaterialType material)
    {
        Material = material;
    }
    
    public MaterialType Material { get; private set; }

    public MaterialColor Color { get; protected set; }
    
    public enum MaterialColor
    {
        Red,
        Yellow,
        Green,
        Blue,
        Purple,
        White,
        Black,
    }
    
    public enum MaterialType
    {
        Plastic,
        Metal,
        Wood,
    }
}

public class BaseMaterial : Resource
{
    public BaseMaterial(MaterialType material, MaterialColor color) : base(material)
    {
        Color = color;
    }
}

public class ProcessedMaterial : BaseMaterial
{
    public ProcessedMaterial(MaterialType material, MaterialColor color) : base(material, color) { }
}

public class ToyPart : BaseMaterial
{
    public ToyPart(MaterialType material, MaterialColor color, ToySection part) : base(material, color)
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

public class Toy : Resource
{
    public Toy(ToyPart[] toyParts, MaterialType material) : base(material)
    {
        ToyParts = toyParts;
    }
    
    public ToyPart[] ToyParts { get; }
}
