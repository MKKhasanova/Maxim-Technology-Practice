namespace TASK7.Models.Config
{
    public class RandomApiConfig
    {
        public string RandomApi { get; set; }
        public Settings Settings { get; set; }
    }

    public class Settings
    {
        public List<string> BlackList { get; set; }
        public int ParallelLimit { get; set; } = 5; // Значение по умолчанию
    }
}
