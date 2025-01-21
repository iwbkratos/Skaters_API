using Skaters.Domain.Model;


namespace Skaters.Models.Domain
{
    public class Cart
    {
        public Guid Id { get; set; }
        public required string UserId { get; set; }
        public string Status { get; set; }
        public double Total { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<Product>? Products { get; set; }
    }
}
