namespace TwoFactorAuthentication.VMModels
{
    public class FeatureViewDataModel2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEdit { get; set; }
        public bool IsCreate { get; set; }
        public bool IsView { get; set; }
    }
}
