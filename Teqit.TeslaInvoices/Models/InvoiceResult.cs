namespace Teqit.TeslaInvoices.Models;
public class InvoiceResult
{
    public string FileName { get; set; } = "";
    public string InvoiceNumber { get; set; } = "Unknown";
    public DateOnly? InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
}