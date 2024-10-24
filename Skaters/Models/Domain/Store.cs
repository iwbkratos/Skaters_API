namespace Skaters.Domain.Model
{
    public class Store
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public required string UserId { get; set; }


        public List<Product> products { get; set; }
    }
}
