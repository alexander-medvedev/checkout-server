using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class DrinkModel
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}