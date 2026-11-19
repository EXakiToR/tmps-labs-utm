using Lab3.Behavioral_design_patterns.Domain;
namespace Lab3.Behavioral_design_patterns.Patterns;

public interface ICommand
{
    void Execute();
    void Undo();
}
public sealed class AttackCommand(Hero attacker, Hero target) : ICommand
{
    private int previousHealth;
    private bool executed;
    public void Execute()
    {
        if (executed) throw new InvalidOperationException("Undo before executing again.");
        // Capture receiver state before delegating the attack to the domain object.
        previousHealth = target.Health;
        attacker.Attack(target);
        executed = true;
    }
    public void Undo()
    {
        if (!executed) throw new InvalidOperationException("Execute before undo.");
        // Restore the exact previous health, including any damage clamped at zero.
        target.RestoreHealth(previousHealth);
        executed = false;
    }
}
public sealed class CommandHistory
{
    private readonly Stack<ICommand> commands = [];
    public void Execute(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        // Only successfully executed commands enter the undo history.
        command.Execute();
        commands.Push(command);
    }
    public bool Undo()
    {
        // Reverse the latest action first to preserve earlier health snapshots.
        if (!commands.TryPeek(out var command)) return false;
        command.Undo();
        commands.Pop();
        return true;
    }
}
