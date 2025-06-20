namespace WorkersApp.Models;

public class Trabajador
{
    public int Id { get; set; }
    public string TipoDocumento { get; set; } = string.Empty;
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public char Sexo { get; set; } = 'M';
    public int IdDepartamento { get; set; }
    public int IdProvincia { get; set; }
    public int IdDistrito { get; set; }

}
