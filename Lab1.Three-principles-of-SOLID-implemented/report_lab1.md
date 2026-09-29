# Report - Lab 1: Three SOLID Principles Implemented

## Introduction

This short report describes three of the SOLID principles implemented in this project. Each principle is demonstrated in a separate file placed in the project root. The goal is to show small, focused examples that highlight the intent and practical application of each principle.

Principles chosen:
- Single Responsibility Principle (SRP)
- Open/Closed Principle (OCP)
- Dependency Inversion Principle (DIP)

## Files and Where Principles Are Used

- SingleResponsibilityPrinciple.cs
  - Principle: Single Responsibility Principle (SRP)
  - What/Where: The file contains an Invoice record (data), an InvoiceCalculator (calculates totals) and an InvoicePrinter (handles presentation). Each class has one reason to change: data structure, calculation rules, or printing format.
  - How it works: The Invoice class only stores items. The InvoiceCalculator computes total using LINQ. The InvoicePrinter takes an Invoice and an InvoiceCalculator and prints line items and total. This separation makes each class easier to maintain and test.

- OpenClosedPrinciple.cs
  - Principle: Open/Closed Principle (OCP)
  - What/Where: The file defines an IShape abstraction and concrete shapes (Rectangle, Circle). AreaCalculator computes the total area of shapes by calling the Area() method on the abstraction.
  - How it works: AreaCalculator depends on the IShape interface. To add new shapes (e.g., Triangle), implement IShape without modifying AreaCalculator. The class is closed for modification but open for extension.

- DependencyInversionPrinciple.cs
  - Principle: Dependency Inversion Principle (DIP)
  - What/Where: The file defines IMessageSender (abstraction) and concrete senders (EmailSender, SmsSender). MessageProcessor depends on IMessageSender and accepts it through constructor injection.
  - How it works: MessageProcessor is a high-level module that uses IMessageSender abstraction. Concrete implementations are provided at composition time, allowing easy swapping, mocking for tests, or adding new transports without changing high-level logic.

## How Each Example Demonstrates the Principle

- SRP: By moving calculation and printing responsibilities out of the Invoice data holder, changes to how totals are calculated or how invoices are printed won't affect the other components.

- OCP: AreaCalculator operates on an abstraction; adding new shapes does not require changes to AreaCalculator. The new behavior is added via new types implementing IShape.

- DIP: MessageProcessor depends on an abstraction (IMessageSender) rather than concrete email or SMS implementations. This decouples high-level logic from low-level implementation details.

## Conclusions

These small examples are intentionally simple but follow the intent of each SOLID principle. Applying SRP, OCP, and DIP in real applications reduces coupling, improves testability, and makes the codebase easier to extend and maintain. The examples also show how dependency injection and small focused classes benefit evolution of a codebase.

For further work, consider adding unit tests that assert behavior for each class and a small composition root (e.g., in Program.cs) that wires implementations together and demonstrates runtime usage.
