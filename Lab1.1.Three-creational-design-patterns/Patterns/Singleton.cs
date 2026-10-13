namespace Lab1._1.Three_creational_design_patterns.Patterns;

public sealed class Singleton
{
    private static readonly object _lock = new();
    private static Singleton? _instance;

    // Example state
    public int Counter { get; private set; }

    private Singleton()
    {
        Counter = 0;
    }

    public static Singleton Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new Singleton();
                }
            }
            return _instance!;
        }
    }

    public void Increment()
    {
        Counter++;
    }

    public string GetInfo() => $"Singleton Counter={Counter}";
}
