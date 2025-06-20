namespace WorkersApp.Models;

public sealed class Distrito
{
    public int Id { get; set; }
    public int? IdProvincia { get; set; }
    public string NombreDistrito { get; set; } = string.Empty;
}
