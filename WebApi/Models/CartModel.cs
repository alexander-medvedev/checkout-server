using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class CartModel
    {
        [Required]
        public string Drink { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}