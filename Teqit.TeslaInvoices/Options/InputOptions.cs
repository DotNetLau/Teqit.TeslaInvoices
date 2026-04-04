using System.ComponentModel.DataAnnotations;

namespace Teqit.TeslaInvoices.Options;
internal class InputOptions
{
    [Required] 
    public required string InputDirectory { get; set; }
}
