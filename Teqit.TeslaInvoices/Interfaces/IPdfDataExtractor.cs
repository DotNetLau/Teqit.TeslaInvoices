namespace Teqit.TeslaInvoices.Interfaces;
internal interface IPdfDataExtractor<T> where T : notnull
{
    T ExtractData(string text);
}
