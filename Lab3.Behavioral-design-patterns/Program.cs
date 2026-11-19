using Lab3.Behavioral_design_patterns.Domain;
using Lab3.Behavioral_design_patterns.Patterns;
using Lab3.Behavioral_design_patterns.Tests;

    if (args is ["--self-test"]) return PatternTests.Run();
    if (args.Length > 0)
    {
        Console.Error.WriteLine("Usage: dotnet run -- [--self-test]");
        return 1;
    }
    // Builder creates the objects used by all three behavioral patterns.
    Hero hero = new HeroBuilder().Named("Aria").WithStrategy(new SwordAttack()).Build();
    Hero guardian = new HeroBuilder().Named("Guardian").Build();
    var display = new HealthDisplay();
    var log = new BattleLog();
    // One health change reaches both independent observers.
    guardian.Subscribe(display);
    guardian.Subscribe(log);
    var history = new CommandHistory();
    Console.WriteLine("Sword attack through Command -> Strategy -> Observer");
    // Command delegates to Hero, which uses Strategy and notifies Observers.
    history.Execute(new AttackCommand(hero, guardian));
    Console.WriteLine("Switch to magic at runtime");
    // Swap behavior on the same hero while the application is running.
    hero.Strategy = new MagicAttack();
    history.Execute(new AttackCommand(hero, guardian));
    Console.WriteLine("Undo the magic attack");
    // Undo restores health and publishes the restored value to subscribers.
    history.Undo();
    Console.WriteLine("Unsubscribe HUD, then attack again");
    // The log remains subscribed after the HUD stops receiving updates.
    guardian.Unsubscribe(display);
    history.Execute(new AttackCommand(hero, guardian));
    Console.WriteLine($"Guardian HP: {guardian.Health}");
    Console.WriteLine("Battle log (including undo notifications):");
    foreach (string entry in log.Entries) Console.WriteLine(entry);
    return 0;
