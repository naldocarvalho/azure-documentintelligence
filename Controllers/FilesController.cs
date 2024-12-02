using Azure;
using Azure.AI.DocumentIntelligence;
using DocIntelligence.Models;
using DocumentIntelligence.InfoConnection;
using DocumentIntelligence.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        string jsonValidation = "";
        var listCreditCard = new List<object>();
        var creditCard = new Dictionary<string, object>();

        try
        {
            if (data.Files != null && data.Files.Count > 0)
            {
                InfoConnectionDocumentIntelligence infoConnectionDocIntelligence = new InfoConnectionDocumentIntelligence();

                _logger.LogInformation($"Quantidade de arquivos enviados: {data.Files.Count}");

                foreach (var file in data.Files)
                {
                    _logger.LogInformation($"Arquivo: {file.FileName} - Content-Type = {file.ContentType}");

                    using var stream = file.OpenReadStream();
                    using var fileContentStream = new MemoryStream();
                    await stream.CopyToAsync(fileContentStream);

                    var client = new DocumentIntelligenceClient(new Uri(infoConnectionDocIntelligence.getEndpoint()), new AzureKeyCredential(infoConnectionDocIntelligence.getKey()));
                    var content = new AnalyzeDocumentContent();

                    if (fileContentStream.Length > 0)
                    {
                        content.Base64Source = BinaryData.FromBytes(fileContentStream.ToArray());
                    }

                    // To see the list of all the supported fields returned by service and its corresponding types for each supported model,
                    // access https://aka.ms/di-prebuilt please.
                    Operation<AnalyzeResult> operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-creditCard", content);
                    AnalyzeResult result = operation.Value;

                    // Extract DocumentField: 
                    for (int i = 0; i < result.Documents.Count; i++)
                    {
                        Console.WriteLine($"Document {i}:");

                        AnalyzedDocument document = result.Documents[i];
                        
                        creditCard = new Dictionary<string, object>();

                        creditCard.Add("File", file.FileName);

                        if (document.Fields.Count > 0)
                        {
                            foreach (var item in document.Fields)
                            {
                                creditCard.Add(item.Key, item.Value.Content);
                                _logger.LogInformation($"campo {item.Key}, valor {item.Value.Content}");
                            }

                            creditCard.Add("IsValid", "True");

                            listCreditCard.Add(creditCard);
                        }
                        else
                        {
                            creditCard.Add("IsValid", "False");

                            listCreditCard.Add(creditCard);
                        }
                    }

                    jsonValidation = JsonSerializer.Serialize(listCreditCard);
                }

                _logger.LogInformation("Concluído o processamento dos arquivos!");

                return Ok(jsonValidation);
            }

            return BadRequest("O arquivo com imagem do cartão de crédito é obrigatório");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro ao processar o cartão de crédito: {ex.Message}\n");
            return BadRequest(ex.Message);
        }
    }
}