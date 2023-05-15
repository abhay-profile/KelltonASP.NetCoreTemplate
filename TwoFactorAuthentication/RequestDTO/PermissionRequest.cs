namespace TwoFactorAuthentication.RequestDTO
{
    public class PermissionRequest
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public List<FeaturePermissionRequest> FeaturePermissionRequests { get; set; }
    }
}
