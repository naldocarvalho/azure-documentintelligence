using System.ComponentModel.DataAnnotations;

namespace DocumentIntelligence.Models;

public class FormData
{
    [Required(ErrorMessage = "Pelo menos um arquivo deve ser enviado.")]
    public List<IFormFile>? Files { get; set; }

    public string? Note { get; set; }
}