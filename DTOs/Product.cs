using System.ComponentModel.DataAnnotations;

namespace Test1.DTOs;

public class Product
{
    public int Id { get; set; }


    [MaxLength(150)]
    public string Name { get; set; } = null!;

    public string Descrption { get; set; }


    public decimal StickerPrice { get; set; }

    public int ProductTypeId { get; set; }

    public int MakerId { get; set; } 


}