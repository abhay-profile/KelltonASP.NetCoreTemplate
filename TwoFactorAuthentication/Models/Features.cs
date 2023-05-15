using System.ComponentModel.DataAnnotations;

namespace TwoFactorAuthentication.Models
{
    public class Features
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
