using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Teqit.TeslaInvoices.HostedServices;
using Teqit.TeslaInvoices.Interfaces;
using Teqit.TeslaInvoices.Options;
using Teqit.TeslaInvoices.Services;

var builder = Host.CreateApplicationBuilder();

builder.Logging.AddConsole();

builder.Services
    .AddOptions<InputOptions>()
    .PostConfigure(x =>
    {
        x.InputDirectory = args.Length > 0 ? args[0] : @"C:\temp\Tesla invoices\2025\12";
    })
    .ValidateOnStart();

builder.Services
    .AddSingleton<IPdfDataExtractor<string>, PdfInvoiceNumberExtractor>()
    .AddSingleton<IPdfDataExtractor<DateOnly>, PdfDateExtractor>()
    .AddSingleton<IPdfDataExtractor<decimal>, PdfTotalAmountExtractor>()
    .AddSingleton<IPdfReader, PdfReader>()
    .AddHostedService<TeslaInvoiceProcessor>();

var app = builder.Build();
await app.RunAsync();