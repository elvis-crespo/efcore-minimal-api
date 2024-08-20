namespace EFC_MinimalApis.Models
{
    public class Book
    {
        public int id { get; set; }
        public string? Name{ get; set; }
        public string? ISBN { get; set; }
    }

    public record BookRequest(string Name, string Isbn);
}
