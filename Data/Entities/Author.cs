namespace BookLibrary.Data.Entities
{
    public class Author: EntityBase
    {
        public string Name { get; set; } = "";

        public int? BirthData { get; set; }

        public virtual List<Book> Books { get; set; } = new List<Book>();
    }
}
