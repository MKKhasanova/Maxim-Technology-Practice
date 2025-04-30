namespace StringProcessor.API.Models.Config
{
    public class RandomApiConfig
    {
        public string RandomApi { get; set; }
        public Settings Settings { get; set; }
    }

    public class Settings
    {
        public List<string> BlackList { get; set; }
    }
}