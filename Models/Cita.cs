using SQLite;

namespace FisioTurno.Models;

public class Cita
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string NombrePaciente { get; set; } = string.Empty;
    public string Fecha { get; set; } = string.Empty;
    public string Hora { get; set; } = string.Empty;
    public string Servicio { get; set; } = string.Empty;
    public string Notas { get; set; } = string.Empty;
}



