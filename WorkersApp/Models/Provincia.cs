﻿namespace WorkersApp.Models;

public sealed class Provincia
{
    public int Id { get; set; }
    public int? IdDepartamento { get; set; } 
    public string NombreProvincia { get; set; } = string.Empty;

}
