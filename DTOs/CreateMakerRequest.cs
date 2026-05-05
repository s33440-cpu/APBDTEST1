namespace Test1.DTOs
{
    public class CreateMakerRequest
    {
        public string Name { get; set; } = null!;
        public List<CreateProductRequest>? Products { get; set; }
    }
}
