using System.IO.Compression;
using Teqit.TeslaInvoices.Interfaces;

namespace Teqit.TeslaInvoices.Services;

internal class PdfArchiver : IPdfArchiver
{
    public async Task UnZip(string invoiceDirectory, CancellationToken stoppingToken)
    {
        var zipFiles = Directory.GetFiles(invoiceDirectory, "*.zip", SearchOption.AllDirectories);
        foreach (var zipFile in zipFiles)
        {
            await ZipFile.ExtractToDirectoryAsync(zipFile, invoiceDirectory, true, stoppingToken);
        }
    }
}