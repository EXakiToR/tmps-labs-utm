namespace Lab1._1.Three_creational_design_patterns.Patterns;

public sealed class GameJournal
{
    private static readonly Lazy<GameJournal> _instance = new(() => new GameJournal());
    private readonly List<string> _entries = [];
    private readonly object _gate = new();

    private GameJournal() { }
    public static GameJournal Instance => _instance.Value;

    public void Record(string entry)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(entry);
        lock (_gate) _entries.Add(entry);
    }

    public IReadOnlyList<string> GetEntries()
    {
        lock (_gate) return Array.AsReadOnly(_entries.ToArray());
    }
}
