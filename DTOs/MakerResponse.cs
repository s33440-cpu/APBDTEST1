using Test1.DTOs;

namespace Test1.DTOs;

public class MakerResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public List<ProductResponse> Products { get; set; } = new();
}