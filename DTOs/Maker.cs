using System.ComponentModel.DataAnnotations;

namespace Test1.DTOs
{
    public class Maker
    {
        public int Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; } = null!;
    }
}
