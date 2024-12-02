using DocumentIntelligence.Models;
using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.AspNetCore.Mvc;

namespace DocumentIntelligence.Controllers;

[ApiController]
[Route("[controller]")]
public class FilesController : ControllerBase
{
    private readonly ILogger<FilesController> _logger;

    public FilesController(ILogger<FilesController> logger)
    {
        _logger = logger;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadFormData([FromForm]FormData data)
    {
        if (data.Files != null && data.Files.Count > 0)
        {
            _logger.LogInformation($"Quantidade de arquivos enviados: {data.Files.Count}");

            foreach (var file in data.Files)
            {
                _logger.LogInformation($"Arquivo: {file.FileName} - Content-Type = {file.ContentType}");

                using var stream = file.OpenReadStream();
                using var fileContentStream = new MemoryStream();
                await stream.CopyToAsync(fileContentStream);

                //var client = new DocumentIntelligenceClient(new Uri(this.docIntelligenceEndPoint), new AzureKeyCredential(this.docIntelligenceApiKey));
                //var content = new AnalyzeDocumentContent();

                //await System.IO.File.WriteAllBytesAsync(newFile, fileContentStream.ToArray());
                //if (System.IO.File.Exists(newFile))
                //    _logger.LogInformation($"Novo arquivo: {newFile} | Nome original: {file.FileName}");
            }

            _logger.LogInformation("Concluído o processamento dos arquivos!");
        }

        return Ok();
    }
}