using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton(new DocumentIntelligenceClient(
    new Uri(builder.Configuration["Azure:DocumentIntelligence:Endpoint"]),
    new AzureKeyCredential(builder.Configuration["Azure:DocumentIntelligence:ApiKey"])
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "Azure Document Intelligence Api")
    );
}

app.UseHttpsRedirection();

app.MapPost("/validate/creditcard", async (IFormFileCollection files, DocumentIntelligenceClient client) =>
{
    var listCreditCard = new List<object>();
    var creditCard = new Dictionary<string, object>();

    if (!files.Any())
        return Results.BadRequest("Nenhum arquivo enviado.");

    Console.WriteLine($"Quantidade de arquivos enviados: {files.Count}");

    foreach (var file in files)
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
                //TO-DO: Adicionar regra de negócio pra validação dos campos do cartão de crédito
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

    return Results.Ok(listCreditCard);
})
.WithName("ValidateCreditCard")
.WithOpenApi(x => new OpenApiOperation(x)
{
    Summary = "Valida imagem de cartão de crédito",
    Description = "Retorna se o cartão de crédito é valido com suas informações",
    Tags = new List<OpenApiTag> { new() { Name = "Hub IA Services" } }
})
.DisableAntiforgery();

app.Run();