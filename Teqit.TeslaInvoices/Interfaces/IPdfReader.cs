using Teqit.TeslaInvoices.Models;

namespace Teqit.TeslaInvoices.Interfaces;

internal interface IPdfReader
{
    InvoiceResult ProcessTeslaInvoice(string pdfPath);
}