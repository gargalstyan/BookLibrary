using BookLibrary.Data.Entities;

namespace BookLibrary.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public int PublisherID { get; set; }
        public string Name { get; set; } = "";

        public string? Genre { get; set; }

        public int? Year { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }
     
        public List<Publisher> Publishers { get; set; } = new List<Publisher>();
        public string Publisher { get; set; } = "";
        public string Location { get; set; } = "";
        public List<Author> Authors { get; set; } = new List<Author>();
        //public List<string> AuthorsName { get; set; } = new List<string>();
        //public int BirthDate { get; set; }
    }
}
