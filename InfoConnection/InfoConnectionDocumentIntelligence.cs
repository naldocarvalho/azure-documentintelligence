namespace DocumentIntelligence.InfoConnection;

internal class InfoConnectionDocumentIntelligence
{
    private static readonly string key = "WeE8OvFtJH6S9O0nF1dL2bN1ZmmPBi9841wHVhTAkhgElXYyO6ccJQQJ99ALACYeBjFXJ3w3AAALACOGRabo";
    public string getKey()
    {
        return key;
    }

    private static readonly string endpoint = "https://documention-validation-openai-eastus.cognitiveservices.azure.com/";
    public string getEndpoint()
    {
        return endpoint;
    }

    private static readonly string location = "eastus";
    public string getLocation()
    {
        return location;
    }
}