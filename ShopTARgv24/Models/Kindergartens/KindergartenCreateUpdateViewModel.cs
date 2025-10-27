namespace ShopTARgv24.Models.Kindergartens
{
    public class KindergartenCreateUpdateViewModel
    {
        public Guid? Id { get; set; }
        public string? GroupName { get; set; }
        public int? ChildrenCount { get; set; }
        public string? KindergartenName { get; set; }
        public string? Teacher { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string? Address { get; set; }
        public string? ContactPhone { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }

        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
        public List<KindergartenImageViewModel> Images { get; set; } = new List<KindergartenImageViewModel>();

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}