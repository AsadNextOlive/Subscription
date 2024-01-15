using System.ComponentModel.DataAnnotations;

namespace Planner1.Models
{
    public class Plans
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PlanName { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Duration { get; set; }
    }
}
