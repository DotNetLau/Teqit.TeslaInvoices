namespace Teqit.TeslaInvoices.Models;
public class InvoiceResult
{
    public string FileName { get; set; } = "";
    public string InvoiceNumber { get; set; } = "Unknown";
    public DateOnly? InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }

    public void Print()
    {
        Console.WriteLine($"{FileName}");
        Console.WriteLine($"Invoice Number: {InvoiceNumber}");
        Console.WriteLine($"Date: {InvoiceDate}");
        Console.WriteLine($"Total (incl. BTW): {TotalAmount:F2} euro");
        Console.WriteLine();
    }
}