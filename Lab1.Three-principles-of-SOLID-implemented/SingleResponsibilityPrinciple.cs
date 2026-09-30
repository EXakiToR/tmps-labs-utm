using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Lab1.Three_principles_of_SOLID_implemented;

// Demonstrates the Single Responsibility Principle (SRP)
// Invoice holds data only. Calculation and printing are separated into their own classes.
public record Item(string Name, decimal Price, int Quantity);

public class Invoice
{
    private readonly List<Item> _items;

    // Expose a read-only view to prevent external modification of the internal list
    public ReadOnlyCollection<Item> Items => _items.AsReadOnly();

    public Invoice(IEnumerable<Item> items)
    {
        _items = items?.ToList() ?? new List<Item>();
    }
}

public class InvoiceCalculator
{
    public decimal CalculateTotal(Invoice invoice)
    {
        if (invoice == null) throw new ArgumentNullException(nameof(invoice));
        return invoice.Items.Sum(i => i.Price * i.Quantity);
    }
}

public class InvoicePrinter
{
    public void Print(Invoice invoice, InvoiceCalculator calculator)
    {
        if (invoice == null) throw new ArgumentNullException(nameof(invoice));
        if (calculator == null) throw new ArgumentNullException(nameof(calculator));

        var total = calculator.CalculateTotal(invoice);
        Console.WriteLine("--- Invoice ---");
        foreach (var it in invoice.Items)
        {
            Console.WriteLine($"{it.Name}: {it.Quantity} x {it.Price:C} = {it.Price * it.Quantity:C}");
        }
        Console.WriteLine($"Total: {total:C}");
    }
}
