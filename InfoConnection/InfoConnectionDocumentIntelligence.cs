namespace DocumentIntelligence.InfoConnection;

internal class InfoConnectionDocumentIntelligence
{
    private static readonly string key = "#YOUR-KEY-HERE#";
    public string getKey()
    {
        return key;
    }

    private static readonly string endpoint = "#YOUR-ENDPOINT-HERE#";
    public string getEndpoint()
    {
        return endpoint;
    }

    private static readonly string location = "#YOUR-LOCATION-HERE#";
    public string getLocation()
    {
        return location;
    }
}