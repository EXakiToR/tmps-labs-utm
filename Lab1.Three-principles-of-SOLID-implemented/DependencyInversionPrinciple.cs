using System;

namespace Lab1.Three_principles_of_SOLID_implemented;

// Demonstrates the Dependency Inversion Principle (DIP)
// High-level modules depend on abstractions (IMessageSender) instead of concrete implementations.
public interface IMessageSender
{
    void Send(string recipient, string message);
}

public class EmailSender : IMessageSender
{
    public void Send(string recipient, string message)
    {
        // In a real app this would call an SMTP client or an external API.
        Console.WriteLine($"[Email] To: {recipient} - {message}");
    }
}

public class SmsSender : IMessageSender
{
    public void Send(string recipient, string message)
    {
        // In a real app this would call an SMS gateway.
        Console.WriteLine($"[SMS] To: {recipient} - {message}");
    }
}

public class MessageProcessor
{
    private readonly IMessageSender _sender;

    public MessageProcessor(IMessageSender sender)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    public void Notify(string recipient, string message)
    {
        _sender.Send(recipient, message);
    }
}
