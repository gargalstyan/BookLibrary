namespace BookLibrary.Data.Entities
{
    public class EntityBase
    {
        public int ID { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
