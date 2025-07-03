using Microsoft.Extensions.Hosting;
using Teqit.TeslaInvoices.Models;
using Teqit.TeslaInvoices.Options;
using Teqit.TeslaInvoices.Services;

namespace Teqit.TeslaInvoices.HostedServices;
public class TeslaInvoiceProcessor(InputOptions _inputOptions, PDFReader _pdfReader) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Tesla Invoice Reader - .NET 9");
        Console.WriteLine("================================");

        var invoiceDirectory = _inputOptions.InputDirectory;

        var pdfFiles = Directory.GetFiles(invoiceDirectory, "*.pdf", SearchOption.AllDirectories);

        if (pdfFiles.Length == 0)
        {
            Console.WriteLine($"No PDF files found in directory: {invoiceDirectory}");
            return;
        }

        Console.WriteLine($"Found {pdfFiles.Length} PDF file(s) to process...\n");

        var invoiceResults = new List<InvoiceResult>();
        decimal grandTotal = 0;

        foreach (string pdfFile in pdfFiles)
        {
            try
            {
                var result = _pdfReader.ProcessTeslaInvoice(pdfFile);
                invoiceResults.Add(result);
                grandTotal += result.TotalAmount;

                Console.WriteLine($"{Path.GetFileName(pdfFile)}");
                Console.WriteLine($"Invoice Number: {result.InvoiceNumber}");
                Console.WriteLine($"Date: {result.InvoiceDate}");
                Console.WriteLine($"Total (incl. BTW): {result.TotalAmount:F2} euro");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {Path.GetFileName(pdfFile)}: {ex.Message}");
                Console.WriteLine();
            }
        }


        if (grandTotal < 0)
        {
            Console.WriteLine("Warning: Total amount is negative, please check the invoices and/or application logic for errors. Or perhaps the invoice lay-out has changed.");
        }
        else
        {
            Console.WriteLine("================================");
            Console.WriteLine("SUMMARY");
            Console.WriteLine("================================");
            Console.WriteLine($"Total invoices processed: {invoiceResults.Count}");
            Console.WriteLine($"Grand total (incl. BTW): {grandTotal:F2} euro");

            if (invoiceResults.Count != 0)
            {
                Console.WriteLine($"Average per invoice (incl. BTW): {grandTotal / invoiceResults.Count:F2} euro");
            }
            Console.WriteLine("================================");
        }
    }
}
