using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.Data.Entities
{
    public class Book: EntityBase
    {
        public string Name { get; set; } = "";

        public string? Genre { get; set; }

        public int? Year { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public virtual List<Author> Authors { get; set; } = new List<Author>();

        public virtual Publisher Publisher { get; set; } = new Publisher();

        [ForeignKey("Publisher")]
        public int PublisherID { get; set; }
    }
}
