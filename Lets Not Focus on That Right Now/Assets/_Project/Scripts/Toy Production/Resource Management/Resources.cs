using System.Linq;

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
    protected bool Equals(ToyPart other)
    {
        return Part == other.Part;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((ToyPart)obj);
    }

    public override int GetHashCode()
    {
        return (int)Part;
    }

    public ToyPart(MaterialType material, MaterialColor color, ToySection part) : base(material, color)
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

    public static bool operator ==(ToyPart a, ToyPart b)
    {
        if (a.Part != b.Part)
            return false;
        if (a.Material != b.Material)
            return false;
        return a.Color == b.Color;
    }

    public static bool operator !=(ToyPart a, ToyPart b)
    {
        return !(a == b);
    }
}

public class Toy : Resource
{
    protected bool Equals(Toy other)
    {
        return Equals(ToyParts, other.ToyParts);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == this.GetType() && Equals((Toy)obj);
    }

    public override int GetHashCode()
    {
        return (ToyParts != null ? ToyParts.GetHashCode() : 0);
    }

    public Toy(ToyPart[] toyParts, MaterialType material) : base(material)
    {
        ToyParts = toyParts;
    }
    
    public ToyPart[] ToyParts { get; }

    public static bool operator ==(Toy a, Toy b)
    {
        var aParts = a.ToyParts.ToList();
        var bParts = b.ToyParts.ToList();
        for (int i = aParts.Count - 1; i >= 0; i--)
        {
            bool matchFound = false;
            for (int j = bParts.Count - 1; j >= 0; j--)
            {
                if (aParts[i] != bParts[j])
                    continue;
                matchFound = true;
                aParts.RemoveAt(i);
                bParts.RemoveAt(j);
                break;
            }
            if (!matchFound)
                return false;
        }
        return true;
    }

    public static bool operator !=(Toy a, Toy b)
    {
        return !(a == b);
    }
}
