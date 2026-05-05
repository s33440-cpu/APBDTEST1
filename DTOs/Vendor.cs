using System.ComponentModel.DataAnnotations;

namespace S32750Test.DTOs
{
    public class Vendor
    {
        [MaxLength(10)]
        public char Code { get; set; }

        [MaxLength(100)]

        public string Name { get; set; } = null!;
    }
}
