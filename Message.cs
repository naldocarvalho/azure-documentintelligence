using BlobStorage;

namespace DocIntelligence
{
    internal class Message
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private string text;

        public string getText()
        {
            return text;
        }

        public void setText(string text)
        {
            this.text = text;
        }

        private string systemText;
        public string getSystemText()
        {
            return systemText;
        }

        public void setSystemText(string systemText)
        {
            this.systemText = systemText;
        }

        private string systemTextUser;
        public string getSystemTextUser()
        {
            return systemTextUser;
        }

        public void setSystemTextUser(string systemTextUser)
        {
            this.systemTextUser = systemTextUser;
        }

        private string languageFrom;
        public string getLanguageFrom()
        {
            return languageFrom;
        }

        public void setLanguageFrom(string languageFrom)
        {
            this.languageFrom = languageFrom;
        }

        private string languageTo;
        public string getLanguageTo()
        {
            return languageTo;
        }

        public void setLanguageTo(string languageTo)
        {
            this.languageTo = languageTo;
        }

        public async Task readText()
        {

        }
    }
}