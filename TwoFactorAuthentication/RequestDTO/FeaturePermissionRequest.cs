namespace TwoFactorAuthentication.RequestDTO
{
    public class FeaturePermissionRequest
    {
        public int FeatureId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEdit { get; set; }
        public bool IsCreate { get; set; }
        public bool IsView { get; set; }
    }
}
