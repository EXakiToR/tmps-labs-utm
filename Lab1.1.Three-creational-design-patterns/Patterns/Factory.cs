namespace Lab1._1.Three_creational_design_patterns.Patterns;

public interface IEnemy
{
    string Name { get; }
    int Health { get; }
    int Attack { get; }
    int Reward { get; }
}

public sealed class Goblin : IEnemy
{
    public string Name => "Goblin";
    public int Health => 40;
    public int Attack => 8;
    public int Reward => 10;
}

public sealed class Dragon : IEnemy
{
    public string Name => "Dragon";
    public int Health => 90;
    public int Attack => 20;
    public int Reward => 50;
}

public sealed record BattleResult(bool HeroWon, int RemainingHealth, int GoldEarned);

// Creator: the shared algorithm works with IEnemy, supplied by a factory method.
public abstract class Encounter
{
    public abstract IEnemy CreateEnemy();

    public BattleResult Play(Hero hero)
    {
        ArgumentNullException.ThrowIfNull(hero);
        IEnemy enemy = CreateEnemy();
        int heroHealth = hero.Health;
        int enemyHealth = enemy.Health;
        GameJournal.Instance.Record($"{hero.Name} encounters a {enemy.Name}.");
        // Hero attacks first; every encounter starts at full health.
        while (heroHealth > 0 && enemyHealth > 0)
        {
            enemyHealth -= hero.Attack;
            if (enemyHealth > 0) heroHealth -= enemy.Attack;
        }
        bool won = heroHealth > 0;
        int gold = won ? enemy.Reward : 0;
        GameJournal.Instance.Record(won
            ? $"{hero.Name} defeats {enemy.Name} with {heroHealth} HP and earns {gold} gold."
            : $"{hero.Name} falls to {enemy.Name}; no gold earned.");
        return new BattleResult(won, Math.Max(0, heroHealth), gold);
    }
}

public sealed class ForestEncounter : Encounter
{
    public override IEnemy CreateEnemy() => new Goblin();
}

public sealed class MountainEncounter : Encounter
{
    public override IEnemy CreateEnemy() => new Dragon();
}
