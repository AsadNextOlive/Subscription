using System.ComponentModel.DataAnnotations;

namespace Planner1.Models
{
    public class OscarSignIn
    {
        [Key]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
