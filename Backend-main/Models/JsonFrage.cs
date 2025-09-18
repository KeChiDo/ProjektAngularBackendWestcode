namespace Backend.Models
{
    public class JsonFrage
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string QuestionText { get; set; }
        public List<JsonAntwort> Answers { get; set; }
        public string hint { get; set; }
    }
}
