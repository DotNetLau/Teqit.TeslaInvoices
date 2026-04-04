using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Teqit.TeslaInvoices.Interfaces;
using Teqit.TeslaInvoices.Models;

namespace Teqit.TeslaInvoices.Services;
internal class PdfReader (IPdfDataExtractor<string> _pdfInvoiceNumberExtractor, IPdfDataExtractor<DateOnly>_pdfDateExtractor, IPdfDataExtractor<decimal> _pdfTotalAmountExtractor) : IPdfReader
{
    public InvoiceResult ProcessTeslaInvoice(string pdfPath)
    {
        using var pdfReader = new iText.Kernel.Pdf.PdfReader(pdfPath);
        using var pdfDocument = new PdfDocument(pdfReader);

        var strategy = new SimpleTextExtractionStrategy();
        var text = "";

        // Extract text from all pages
        for (var i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
        {
            text += PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i), strategy);
        }

        return ExtractInvoiceData(text, Path.GetFileName(pdfPath));
    }

    private InvoiceResult ExtractInvoiceData(string text, string fileName)
    {
        var result = new InvoiceResult
        {
            FileName = fileName,
            InvoiceNumber = _pdfInvoiceNumberExtractor.ExtractData(text),
            InvoiceDate = _pdfDateExtractor.ExtractData(text),
            TotalAmount = _pdfTotalAmountExtractor.ExtractData(text),
        };

        return result;
    }
}
