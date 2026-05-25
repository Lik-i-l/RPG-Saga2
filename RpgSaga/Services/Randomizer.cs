namespace RpgSaga.Services;

public interface IRandomizer
{
    int Next(int min, int max);
    double NextDouble();
}

public class Randomizer : IRandomizer
{
    private readonly Random _random = new();

    public int Next(int min, int max) => _random.Next(min, max);
    public double NextDouble() => _random.NextDouble();
}
