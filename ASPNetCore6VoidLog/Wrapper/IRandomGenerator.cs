namespace ASPNetCore6VoidLog.Wrapper
{
    public interface IRandomGenerator
    {
        int Next();
        int Next(int maxValue);
        int Next(int minValue, int maxValue);
        double NextDouble();
    }
}
