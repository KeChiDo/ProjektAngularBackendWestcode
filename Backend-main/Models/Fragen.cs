namespace Backend.Models
{
    public class Fragen
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string QuestionText { get; set; }
        public string? hint {  get; set; }
        public int DateiId { get; set; }
        public Datei Datei { get; set; }
        public ICollection<Antworten> Antworten { get; set; }

    }
}
