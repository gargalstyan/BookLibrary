namespace BookLibrary.Data.Entities
{
    public class Publisher: EntityBase
    {
        public string Location { get; set; } = "";

        public string Name { get; set; } = "";

        public virtual List<Book> Books { get; set; } = new List<Book>();
    }
}
