using System.ComponentModel.DataAnnotations;

namespace Planner1.Models
{
    public class PlanPurchased
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PlanName { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public DateTime PurchasedDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime ExpiredDate { get; set; }
    }
}
