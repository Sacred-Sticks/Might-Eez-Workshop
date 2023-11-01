using System;

public static class ToyFactory
{
    public static BaseMaterial CreateMaterial<T>() where T : BaseMaterial
    {
        return (T) Activator.CreateInstance(typeof(T));
    }
    
    public static ProcessedMaterial<T> ProcessMaterial<T>(BaseMaterial baseMaterial) where T : BaseMaterial
    {
        return new ProcessedMaterial<T>();
    }

    public static ToyPart<T> MoldToy<T>(ProcessedMaterial<T> processedMaterial, ToyPart<T>.ToySection toySection) where T : BaseMaterial
    {
        return new ToyPart<T>(toySection);
    }

    public static Toy<T> AssembleToy<T>(ToyPart<T>[] toyParts) where T : BaseMaterial
    {
        return new Toy<T>(toyParts);
    }
}
