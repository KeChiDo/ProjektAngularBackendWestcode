namespace Backend.Models
{
    public class Datei
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }

        public ICollection<Fragen> Fragen { get; set; }
    }
}
