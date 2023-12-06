public interface ICarriedResourceDisplay
{
    public static int GetColorIndex(Resource argument)
    {
        return argument.Color switch
        {
            Resource.MaterialColor.Red => 0,
            Resource.MaterialColor.Yellow => 1,
            Resource.MaterialColor.Green => 2,
            Resource.MaterialColor.Blue => 3,
            Resource.MaterialColor.Purple => 4,
            Resource.MaterialColor.White => 5,
            Resource.MaterialColor.Black => 6,
            _ => -1,
        };
    }
}
