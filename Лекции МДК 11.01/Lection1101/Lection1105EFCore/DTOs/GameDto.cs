namespace Lection1105EFCore.Models;

public class GameDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string CategoryName { get; set; } = null!;
}
