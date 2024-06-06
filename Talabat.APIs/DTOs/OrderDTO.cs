using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class OrderDTO
    {
        
        public string BuyerEmail { get; set; }
        [Required]
        public string BasketId { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required]
        public ShippingAddressDTO ShippingAddress { get; set; }
    }
}