using System.Text.RegularExpressions;
using Teqit.TeslaInvoices.Interfaces;

namespace Teqit.TeslaInvoices.Services;
internal partial class PdfInvoiceNumberExtractor : IPdfDataExtractor<string>
{
    public string ExtractData(string text)
    {
        var invoiceNumberMatch = ExtractInvoiceNumberRegex().Match(text);
        if (invoiceNumberMatch.Success)
        {
            return invoiceNumberMatch.Groups[1].Value;
        }

        return "Unknown";
    }

    [GeneratedRegex(@"(?:INV|Invoice|Factuurnummer)\s*([A-Z0-9\-]{6,})", RegexOptions.IgnoreCase)]
    private static partial Regex ExtractInvoiceNumberRegex();
}
