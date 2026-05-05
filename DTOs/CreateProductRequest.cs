namespace Test1.DTOs
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal StrickerPrice { get; set; }
        public string Type { get; set; } = null!;
    }
}
