namespace Lab3.Behavioral_design_patterns.Patterns;

// Both the HUD and log receive health changes through this contract.
public interface IHealthObserver
{
    void OnHealthChanged(string name, int health);
}
public sealed class HealthDisplay : IHealthObserver
{
    public void OnHealthChanged(string name, int health) =>
        Console.WriteLine($"HUD: {name} has {health} HP");
}
public sealed class BattleLog : IHealthObserver
{
    private readonly List<string> entries = [];
    public IReadOnlyList<string> Entries => entries.AsReadOnly();
    public void OnHealthChanged(string name, int health) =>
        entries.Add($"{name}: {health} HP");
}
