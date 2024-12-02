using Azure;
using DocumentIntelligence.InfoConnection;
using System.Text;

namespace BlobStorage;

public class ConnectionDocumentIntelligenceAPI
{
    public async Task connectionAPI(Uri endpoint, AzureKeyCredential credential)
    {
        InfoConnectionDocumentIntelligence infoConnectionTranslate = new InfoConnectionDocumentIntelligence();
    }

    private string result;
    public string getResult()
    {
        return result;
    }

    public void setResult(string result)
    {
        this.result = result;
    }
}