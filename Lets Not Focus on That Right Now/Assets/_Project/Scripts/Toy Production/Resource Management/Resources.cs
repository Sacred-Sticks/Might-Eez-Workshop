public abstract class Resource
{
    
}

public abstract class BaseMaterial : Resource
{
    
}

public class Plastic : BaseMaterial
{
    public Plastic() { }
}

public class Metal : BaseMaterial
{
    public Metal() { }
}

public class Wood : BaseMaterial
{
    public Wood() { }
}

public class ProcessedMaterial<T> : Resource where T : BaseMaterial
{
    
}

public class ToyPart<T> : Resource where T : BaseMaterial
{
    public ToyPart(ToySection part)
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

public class Toy<T> : Resource where T : BaseMaterial
{
    public Toy(ToyPart<T>[] toyParts)
    {
        ToyParts = toyParts;
    }
    
    public ToyPart<T>[] ToyParts { get; }
}
