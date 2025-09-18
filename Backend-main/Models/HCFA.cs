namespace Backend.Models
{
    //Hilfsklasse (HC) Fragen und Antworten
    public class HCFA
    {
        public string type { get; set; }
        public string questionText { get; set; }
        
        public string hint { get; set; }

        public List<Dictionary<string, object>> Antworten { get; set; }
    }
}
