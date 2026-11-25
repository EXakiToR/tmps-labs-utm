namespace Lab3.Behavioral_design_patterns.Patterns;

public interface IHealthObserver
{
    // Hero calls this contract whenever its health changes.
    void OnHealthChanged(string name, int health);
}
public sealed class HealthDisplay : IHealthObserver
{
    // This subscriber renders the health update on the console.
    public void OnHealthChanged(string name, int health) =>
        Console.WriteLine($"HUD: {name} has {health} HP");
}
public sealed class BattleLog : IHealthObserver
{
    private readonly List<string> entries = [];
    public IReadOnlyList<string> Entries => entries.AsReadOnly();
    // This subscriber records the same event independently of the display.
    public void OnHealthChanged(string name, int health) =>
        entries.Add($"{name}: {health} HP");
}
