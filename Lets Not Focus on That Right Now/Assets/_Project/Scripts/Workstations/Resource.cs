public abstract class Resource
{
    
}

public abstract class BaseMaterial : Resource
{
    
}

public class Plastic : BaseMaterial
{
    
}

public class Metal : BaseMaterial
{
    
}

public class Melted<T> : Resource where T : BaseMaterial
{
    
}

public class ToyPart : Resource
{
    
}

public class Toy : Resource
{
    
}
