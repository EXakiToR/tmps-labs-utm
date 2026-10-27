namespace Lab1._1.Three_creational_design_patterns.Patterns;

// Immutable configuration; current battle health belongs to an encounter.
public sealed class Hero
{
    public string Name { get; }
    public int Health { get; }
    public int Attack { get; }
    public IReadOnlyList<string> Equipment { get; }

    internal Hero(string name, int health, int attack, IEnumerable<string> equipment)
    {
        Name = name;
        Health = health;
        Attack = attack;
        Equipment = Array.AsReadOnly(equipment.ToArray());
    }
}

public interface IHeroBuilder
{
    IHeroBuilder Named(string name);
    IHeroBuilder WithHealth(int health);
    IHeroBuilder WithAttack(int attack);
    IHeroBuilder AddEquipment(string item);
    Hero Build();
}

public sealed class HeroBuilder : IHeroBuilder
{
    private string _name = string.Empty;
    private int _health = 100;
    private int _attack = 10;
    private readonly List<string> _equipment = [];

    public IHeroBuilder Named(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _name = name.Trim();
        return this;
    }

    public IHeroBuilder WithHealth(int health)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(health);
        _health = health;
        return this;
    }

    public IHeroBuilder WithAttack(int attack)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(attack);
        _attack = attack;
        return this;
    }

    public IHeroBuilder AddEquipment(string item)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(item);
        _equipment.Add(item.Trim());
        return this;
    }

    // Return an independent snapshot. Further edits do not change earlier heroes.
    public Hero Build()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new InvalidOperationException("A hero must have a name before building.");
        return new Hero(_name, _health, _attack, _equipment);
    }
}

// Director recipes expect a fresh builder; Build does not reset builder state.
public static class HeroDirector
{
    public static Hero CreateKnight(IHeroBuilder builder) => builder
        .Named("Aria").WithHealth(100).WithAttack(25)
        .AddEquipment("Sword").AddEquipment("Shield").Build();

    public static Hero CreateRanger(IHeroBuilder builder) => builder
        .Named("Rowan").WithHealth(75).WithAttack(30)
        .AddEquipment("Bow").AddEquipment("Cloak").Build();
}
