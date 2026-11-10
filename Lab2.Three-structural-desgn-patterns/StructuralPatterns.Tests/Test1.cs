using Lab2.Three_structural_desgn_patterns;
using Xunit;

namespace StructuralPatterns.Tests;

public sealed class AdapterTests
{
    [Fact]
    public void AdapterTranslatesLegacyModelAndPower()
    {
        IWeapon weapon = new WandAdapter(new LegacyWand("Oak Wand", 3));

        Assert.Equal("Oak Wand", weapon.Name);
        Assert.Equal(18, weapon.Damage);
    }

    [Fact]
    public void FactoryCreatesWeaponsThroughTheCommonAbstraction()
    {
        IWeaponFactory factory = new WeaponFactory();

        IWeapon weapon = factory.CreateWand("Birch Wand", 4);

        Assert.Equal("Birch Wand", weapon.Name);
        Assert.Equal(24, weapon.Damage);
    }
}

public sealed class DecoratorTests
{
    [Fact]
    public void DecoratorsCanBeStacked()
    {
        IWeapon weapon = new PoisonEnchantment(
            new FireEnchantment(new Sword()));

        Assert.Equal("Iron Sword + Fire + Poison", weapon.Name);
        Assert.Equal(35, weapon.Damage);
    }

    [Fact]
    public void DecoratorDoesNotMutateTheWrappedWeapon()
    {
        IWeapon original = new Sword();
        IWeapon decorated = new FireEnchantment(original);

        Assert.Equal(20, original.Damage);
        Assert.Equal(30, decorated.Damage);
    }
}

public sealed class FacadeTests
{
    [Fact]
    public void FacadeCoordinatesEquipmentBattleAndReward()
    {
        var wallet = new GoldWallet();
        var facade = new ExpeditionFacade(new EquipmentRack(), new BattleArena(), wallet);

        QuestResult result = facade.CompleteQuest(new Sword());

        Assert.True(result.Battle.Won);
        Assert.Equal(20, result.GoldEarned);
        Assert.Equal(20, result.TotalGold);
        Assert.Equal(20, wallet.Balance);
    }

    [Fact]
    public void FacadeDoesNotRewardADefeat()
    {
        var wallet = new GoldWallet();
        var facade = new ExpeditionFacade(new EquipmentRack(), new BattleArena(), wallet);

        QuestResult result = facade.CompleteQuest(new WeakWeapon());

        Assert.False(result.Battle.Won);
        Assert.Equal(0, result.GoldEarned);
        Assert.Equal(0, result.TotalGold);
    }

    private sealed record WeakWeapon : IWeapon
    {
        public string Name => "Training Stick";
        public int Damage => 1;
    }
}
