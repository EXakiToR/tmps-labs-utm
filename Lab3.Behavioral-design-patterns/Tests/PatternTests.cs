using Lab3.Behavioral_design_patterns.Domain;
using Lab3.Behavioral_design_patterns.Patterns;
namespace Lab3.Behavioral_design_patterns.Tests;

public static class PatternTests
{
    public static int Run()
    {
        int passed = 0;
        void Test(string name, Action assertion)
        {
            assertion();
            Console.WriteLine($"PASS {name}");
            passed++;
        }
        void Equal<T>(T expected, T actual)
        {
            if (!Equals(expected, actual)) throw new Exception($"Expected {expected}, got {actual}");
        }
        Hero NewHero() => new HeroBuilder().Build();
        try
        {
            Test("Strategy_SwordDamage", () => Equal(20, new SwordAttack().CalculateDamage()));
            Test("Strategy_RuntimeSwap", () =>
            {
                var hero = NewHero();
                var target = NewHero();
                hero.Attack(target);
                hero.Strategy = new MagicAttack();
                hero.Attack(target);
                Equal(45, target.Health);
            });
            Test("Observer_AllSubscribersNotified", () =>
            {
                var hero = NewHero();
                var first = new BattleLog();
                var second = new BattleLog();
                hero.Subscribe(first);
                hero.Subscribe(second);
                hero.ReceiveDamage(20);
                Equal("Hero: 80 HP", first.Entries.Single());
                Equal("Hero: 80 HP", second.Entries.Single());
            });
            Test("Observer_UnsubscribeAtRuntime", () =>
            {
                var hero = NewHero();
                var log = new BattleLog();
                hero.Subscribe(log);
                hero.ReceiveDamage(20);
                hero.Unsubscribe(log);
                hero.ReceiveDamage(20);
                Equal(1, log.Entries.Count);
            });
            Test("Command_UndoRestoresHealth", () =>
            {
                var target = NewHero();
                var history = new CommandHistory();
                history.Execute(new AttackCommand(NewHero(), target));
                Equal(80, target.Health);
                Equal(true, history.Undo());
                Equal(100, target.Health);
            });
            Test("Command_HistoryIsLifo", () =>
            {
                var attacker = NewHero();
                var target = NewHero();
                var history = new CommandHistory();
                history.Execute(new AttackCommand(attacker, target));
                attacker.Strategy = new MagicAttack();
                history.Execute(new AttackCommand(attacker, target));
                history.Undo();
                Equal(80, target.Health);
                history.Undo();
                Equal(100, target.Health);
                Equal(false, history.Undo());
            });
            Console.WriteLine($"All {passed} tests passed.");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"FAIL {exception.Message}");
            return 1;
        }
    }
}
