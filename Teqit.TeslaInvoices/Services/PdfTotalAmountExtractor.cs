using System.Text.RegularExpressions;
using Teqit.TeslaInvoices.Interfaces;

namespace Teqit.TeslaInvoices.Services;
internal partial class PdfTotalAmountExtractor : IPdfDataExtractor<decimal>
{
    public decimal ExtractData(string text)
    {
        var match = ExtractTotalAmountRegex().Match(text);
        if (match.Success && decimal.TryParse(match.Groups[1].Value, out var amount))
        {
            return amount;
        }

        return -decimal.MaxValue;
    }

    [GeneratedRegex(@"Totaalbedrag\s*\(EUR\)\s*([\d,]+\.?\d{0,2})", RegexOptions.IgnoreCase)]
    private static partial Regex ExtractTotalAmountRegex();
}
