using System.ComponentModel.DataAnnotations;

namespace TwoFactorAuthentication.Models
{
    public class RolesPermissions
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int FeatureId { get; set; }
        [Required]
        public string RoleId { get; set; }
        public bool IsCreate { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsView { get; set; }
    }
}
