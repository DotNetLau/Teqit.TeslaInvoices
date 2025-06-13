namespace Teqit.TeslaInvoices.Interfaces;
public interface IPdfDataExtractor<T> where T : notnull
{
    T ExtractData(string text);
}
