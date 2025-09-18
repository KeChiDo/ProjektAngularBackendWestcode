namespace Backend.Models
{
    public class Antworten
    {
        public int id { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public int FragenId { get; set; }
        public Fragen Fragen { get; set; }
    }
}
