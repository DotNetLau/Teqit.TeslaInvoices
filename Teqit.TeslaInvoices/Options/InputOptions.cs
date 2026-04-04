using System.ComponentModel.DataAnnotations;

namespace Teqit.TeslaInvoices.Options;
internal class InputOptions (string _inputDirectory)
{
    [Required]
    public string InputDirectory => _inputDirectory;
}
