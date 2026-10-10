using System;

namespace Entidades;

public class Product
{
    public int ProductId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? Category { get; set; }
    public bool Flag { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
}
