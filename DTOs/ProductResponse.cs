namespace Test1.DTOs
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal StickerPrice { get; set; }

        public ProductType ProductType { get; set; } = null!;
        public List<VendorResponse> Vendors { get; set; } = new();
    }
}