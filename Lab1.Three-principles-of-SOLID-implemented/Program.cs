namespace Lab1.Three_principles_of_SOLID_implemented;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Choose a SOLID principle to demonstrate: (S)RP, (O)CP, (D)IP");
        Console.Write("Enter first letter: ");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("No input provided. Exiting.");
            return;
        }

        var choice = char.ToUpperInvariant(input.Trim()[0]);
        switch (choice)
        {
            case 'S':
                // Single Responsibility Principle demo
                var items = new List<Item>
                {
                    new Item("Pen", 1.20m, 3),
                    new Item("Notebook", 2.50m, 2)
                };
                var invoice = new Invoice(items);
                var calculator = new InvoiceCalculator();
                var printer = new InvoicePrinter();
                printer.Print(invoice, calculator);
                break;

            case 'O':
                // Open/Closed Principle demo
                var shapes = new List<IShape>
                {
                    new Rectangle(3, 4),
                    new Circle(2)
                };
                var areaCalc = new AreaCalculator();
                Console.WriteLine($"Total area: {areaCalc.TotalArea(shapes):F2}");
                break;

            case 'D':
                // Dependency Inversion Principle demo
                IMessageSender sender = new EmailSender();
                var processor = new MessageProcessor(sender);
                processor.Notify("user@example.com", "This is a DIP demonstration message.");
                break;

            default:
                Console.WriteLine("Unknown selection. Use S, O or D.");
                break;
        }

        Console.WriteLine("Demo complete. Press any key to exit...");
        Console.ReadKey();
    }
}
