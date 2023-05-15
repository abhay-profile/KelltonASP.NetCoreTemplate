using System.ComponentModel.DataAnnotations;

namespace TwoFactorAuthentication.Models
{
    public class UserPermissions
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int FeatureId { get; set; }
        public string UserId { get; set; }
        public bool IsCreate { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsView { get; set; }
    }
}
