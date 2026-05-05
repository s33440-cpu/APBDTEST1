using S32750Test.DTOs;

namespace s32750Test.DTOs;

public class MakerResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public List<ProductResponse> Products { get; set; } = new();
}