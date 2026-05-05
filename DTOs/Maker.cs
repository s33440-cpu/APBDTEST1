using System.ComponentModel.DataAnnotations;

namespace S32750Test.DTOs
{
    public class Maker
    {
        public int Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; } = null!;
    }
}
