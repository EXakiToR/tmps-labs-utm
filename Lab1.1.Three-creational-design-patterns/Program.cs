using Lab1._1.Three_creational_design_patterns.Patterns;

namespace Lab1._1.Three_creational_design_patterns;

internal static class Program
{
    private static int Main(string[] args)
    {
        if (args is ["--self-test"]) return DemoChecks.Run();
        if (args.Length != 0)
        {
            Console.Error.WriteLine("Usage: dotnet run -- [--self-test]");
            return 1;
        }
        Console.WriteLine("=== Dungeon Expedition: Creational Patterns ===");
        Hero knight = HeroDirector.CreateKnight(new HeroBuilder());
        Hero ranger = HeroDirector.CreateRanger(new HeroBuilder());
        Console.WriteLine("\n[Builder] Two heroes assembled using different recipes:");
        foreach (Hero hero in new[] { knight, ranger })
            Console.WriteLine($"{hero.Name}: HP={hero.Health}, Attack={hero.Attack}, Equipment=[{string.Join(", ", hero.Equipment)}]");

        Console.WriteLine("\n[Factory Method] Explore two locations:");
        Encounter[] encounters = [new ForestEncounter(), new MountainEncounter()];
        int totalGold = 0;
        foreach (Encounter encounter in encounters)
        {
            BattleResult result = encounter.Play(knight);
            totalGold += result.GoldEarned;
            Console.WriteLine($"{encounter.GetType().Name}: Victory={result.HeroWon}, HP={result.RemainingHealth}, Gold={result.GoldEarned}");
        }
        GameJournal journal = GameJournal.Instance;
        Console.WriteLine($"\n[Singleton] Same journal instance: {ReferenceEquals(journal, GameJournal.Instance)}");
        foreach (string entry in journal.GetEntries()) Console.WriteLine($"- {entry}");
        Console.WriteLine($"\nExpedition complete. Total gold: {totalGold}");
        return 0;
    }
}
