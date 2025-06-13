using System.Text.RegularExpressions;
using Teqit.TeslaInvoices.Interfaces;

namespace Teqit.TeslaInvoices.Services;
internal partial class PdfDateExtractor : IPdfDataExtractor<DateOnly>
{
    public DateOnly ExtractData(string text)
    {
        var dateMatch = ExtracDateRegex().Match(text);
        if (dateMatch.Success && DateOnly.TryParse(dateMatch.Groups[1].Value, out DateOnly parsedDate))
        {
            return parsedDate;
        }

        return DateOnly.MinValue;
    }

    [GeneratedRegex(@"Factuurdatum\s*:?\s*(\d{4}[\/\-]\d{2}[\/\-]\d{2})", RegexOptions.IgnoreCase)]
    private static partial Regex ExtracDateRegex();
}
