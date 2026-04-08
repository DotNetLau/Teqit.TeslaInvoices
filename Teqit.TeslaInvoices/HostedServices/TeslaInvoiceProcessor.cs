using System.Diagnostics;
using System.IO.Compression;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Teqit.TeslaInvoices.Interfaces;
using Teqit.TeslaInvoices.Options;

namespace Teqit.TeslaInvoices.HostedServices;
internal class TeslaInvoiceProcessor(IOptions<InputOptions> _inputOptions, IPdfReader _pdfReader, IPdfArchiver _pdfArchiver) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Tesla Invoice Reader");
        Console.WriteLine("================================");

        var sw = Stopwatch.StartNew();
        
        var invoiceDirectory = _inputOptions.Value.InputDirectory;

        await _pdfArchiver.UnZip(invoiceDirectory, stoppingToken);
        
        var pdfFiles = Directory.GetFiles(invoiceDirectory, "*.pdf", SearchOption.AllDirectories);
        if (pdfFiles.Length == 0)
        {
            Console.WriteLine($"No PDF files found in directory: {invoiceDirectory}");
            return;
        }

        Console.WriteLine($"Found {pdfFiles.Length} PDF file(s) to process...\n");

        var tasks = pdfFiles.Select(pdfFile => Task.Run(() => _pdfReader.ProcessTeslaInvoice(pdfFile), stoppingToken)).ToList();
        var invoiceResults = await Task.WhenAll(tasks);

        foreach (var invoiceResult in invoiceResults)
        {
            invoiceResult.Print();
        }
        
        var grandTotal = invoiceResults.Sum(x => x.TotalAmount);
        if (grandTotal < 0)
        {
            Console.WriteLine("Warning: Total amount is negative, please check the invoices and/or application logic for errors. Or perhaps the invoice lay-out has changed.");
        }
        else
        {
            Console.WriteLine("================================");
            Console.WriteLine("SUMMARY");
            Console.WriteLine("================================");
            Console.WriteLine($"Total invoices processed: {invoiceResults.Length}");
            Console.WriteLine($"Grand total (incl. VAT): {grandTotal:F2} euro");

            if (invoiceResults.Length != 0)
            {
                Console.WriteLine($"Average per invoice (incl. VAT): {grandTotal / invoiceResults.Length:F2} euro");
            }
            Console.WriteLine("================================");
        }

        sw.Stop();
        var elapsedTimeInSeconds = sw.Elapsed.TotalSeconds;
        Console.WriteLine($"Processing took {elapsedTimeInSeconds:F2} seconds.");
    }
}
