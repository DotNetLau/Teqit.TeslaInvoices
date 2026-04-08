namespace Teqit.TeslaInvoices.Interfaces;

internal interface IPdfArchiver
{
    Task UnZip(string inputDirectory, CancellationToken stoppingToken);
}