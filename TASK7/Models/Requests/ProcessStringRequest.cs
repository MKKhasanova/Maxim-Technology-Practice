namespace StringProcessor.API.Models.Requests
{
    public class ProcessStringRequest
    {
        public string Input { get; set; }
        public int SortMethod { get; set; } = 1;
    }
}
