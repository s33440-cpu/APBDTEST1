using System.ComponentModel.DataAnnotations;

namespace S32750Test.DTOs
{
    public class ProductType
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = null!;
    }
}
