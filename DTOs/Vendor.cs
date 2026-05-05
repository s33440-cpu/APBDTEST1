using System.ComponentModel.DataAnnotations;

namespace Test1.DTOs
{
    public class Vendor
    {
        [MaxLength(10)]
        public char Code { get; set; }

        [MaxLength(100)]

        public string Name { get; set; } = null!;
    }
}
