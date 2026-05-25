namespace RpgSaga.Services;

public class Logger
{
    private readonly List<string> _logs = new();

    public void Log(string message)
    {
        Console.WriteLine(message);
        _logs.Add(message);
    }

    public IReadOnlyList<string> GetLogs() => _logs.AsReadOnly();
    public void Clear() => _logs.Clear();
}
