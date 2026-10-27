namespace Lab2.Three_structural_desgn_patterns;

public sealed class EquipmentRack
{
    public IWeapon? EquippedWeapon { get; private set; }

    public void Equip(IWeapon weapon)
    {
        ArgumentNullException.ThrowIfNull(weapon);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(weapon.Damage);
        EquippedWeapon = weapon;
    }
}

public sealed record BattleResult(bool Won, int Turns, int RemainingHealth);
public sealed record QuestResult(BattleResult Battle, int GoldEarned, int TotalGold);

public sealed class BattleArena
{
    public BattleResult Fight(IWeapon weapon)
    {
        ArgumentNullException.ThrowIfNull(weapon);
        int damage = weapon.Damage;
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(damage);
        int heroHealth = 100;
        int guardianHealth = 60;
        int turns = 0;
        while (heroHealth > 0 && guardianHealth > 0)
        {
            turns++;
            guardianHealth -= damage;
            if (guardianHealth > 0) heroHealth -= 15;
        }
        return new BattleResult(heroHealth > 0, turns, Math.Max(0, heroHealth));
    }
}

public sealed class GoldWallet
{
    public int Balance { get; private set; }

    public void Deposit(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        Balance = checked(Balance + amount);
    }
}

// One entry point coordinates three independently usable subsystems.
public sealed class ExpeditionFacade
{
    private readonly EquipmentRack _equipment;
    private readonly BattleArena _arena;
    private readonly GoldWallet _wallet;

    public ExpeditionFacade(EquipmentRack equipment, BattleArena arena, GoldWallet wallet)
    {
        ArgumentNullException.ThrowIfNull(equipment);
        ArgumentNullException.ThrowIfNull(arena);
        ArgumentNullException.ThrowIfNull(wallet);
        _equipment = equipment;
        _arena = arena;
        _wallet = wallet;
    }

    public QuestResult CompleteQuest(IWeapon weapon)
    {
        _equipment.Equip(weapon);
        BattleResult battle = _arena.Fight(_equipment.EquippedWeapon!);
        int reward = battle.Won ? 20 : 0;
        if (reward > 0) _wallet.Deposit(reward);
        return new QuestResult(battle, reward, _wallet.Balance);
    }
}
