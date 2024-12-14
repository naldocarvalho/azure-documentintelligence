using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton(new DocumentIntelligenceClient (
    new Uri(builder.Configuration["Azure:DocumentIntelligence:Endpoint"]),
    new AzureKeyCredential(builder.Configuration["Azure:DocumentIntelligence:ApiKey"])
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
        options.WithTitle("Azure Document Intelligence")
        .WithTheme(ScalarTheme.Saturn)
        .WithDefaultHttpClient(ScalarTarget.Shell, ScalarClient.Curl)
    );
}

app.UseHttpsRedirection();

app.MapPost("/validate/creditcard", async (HttpRequest request, DocumentIntelligenceClient client) =>
{
    string jsonValidation = "";
    var listCreditCard = new List<object>();
    var creditCard = new Dictionary<string, object>();

    if (!request.Form.Files.Any())
        return Results.BadRequest("Nenhum arquivo enviado.");

    Console.WriteLine($"Quantidade de arquivos enviados: {request.Form.Files.Count}");

    foreach (var file in request.Form.Files)
    {
        Console.WriteLine($"Arquivo: {file.FileName} - Content-Type = {file.ContentType}");

        using var stream = file.OpenReadStream();
        using var fileContentStream = new MemoryStream();
        var content = new AnalyzeDocumentContent();

        await stream.CopyToAsync(fileContentStream);

        if (fileContentStream.Length > 0)
        {
            content.Base64Source = BinaryData.FromBytes(fileContentStream.ToArray());
        }

        var operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-creditCard", content);
        var result = operation.Value;

        // Extract DocumentField: 
        for (int i = 0; i < result.Documents.Count; i++)
        {
            Console.WriteLine($"Document {i}:");

            AnalyzedDocument document = result.Documents[i];

            creditCard = new Dictionary<string, object>();

            creditCard.Add("File", file.FileName);

            if (document.Fields.Count > 0)
            {
                //TO-DO: Adicionar regra de neg�cio pra valida��o dos campos do cart�o de cr�dito
                foreach (var item in document.Fields)
                {
                    creditCard.Add(item.Key, item.Value.Content);
                    Console.WriteLine($"campo {item.Key}, valor {item.Value.Content}");
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
    }

    jsonValidation = JsonSerializer.Serialize(listCreditCard);

    return Results.Ok(jsonValidation);

})
.WithName("ValidateCreditCard")
.WithOpenApi(x => new OpenApiOperation(x)
{
    Summary = "Valida imagem de cart�o de cr�dito",
    Description = "Retorna se o cart�o de cr�dito � valido com suas informa��es",
    Tags = new List<OpenApiTag> { new() { Name = "Hub IA Services" } }
})
.Accepts<List<IFormFile>>("multipart/form-data");

app.Run();