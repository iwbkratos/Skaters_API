namespace Skaters.Models.DTO.CartProductDto
{
    public class OrdersResponse
    {
      public List<CartproductResponse> products { get; set; }
        public Guid CartId { get; set; }
        public string Status { get; set; }
        public double Total { get; set; }
        public  DateTime date { get; set; }

    }
}
