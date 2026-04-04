using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Teqit.TeslaInvoices.HostedServices;
using Teqit.TeslaInvoices.Interfaces;
using Teqit.TeslaInvoices.Options;
using Teqit.TeslaInvoices.Services;

var builder = Host.CreateApplicationBuilder();

builder.Logging.AddConsole();

builder.Services.AddSingleton<IPdfDataExtractor<string>, PdfInvoiceNumberExtractor>();
builder.Services.AddSingleton<IPdfDataExtractor<DateOnly>, PdfDateExtractor>();
builder.Services.AddSingleton<IPdfDataExtractor<decimal>, PdfTotalAmountExtractor>();
builder.Services.AddSingleton<IPdfReader, PdfReader>();

builder.Services.AddSingleton<InputOptions>(x =>
{
    var inputDirectory = args.Length > 0 ? args[0] : @"G:\\Other computers\\Current\\Google Drive\\Loyal Interim\\Tesla facturen\\2025\\12"; ;
    return new InputOptions(inputDirectory);
});

builder.Services.AddHostedService<TeslaInvoiceProcessor>();

var app = builder.Build();
await app.RunAsync();