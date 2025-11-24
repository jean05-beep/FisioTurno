using SQLite;

namespace FisioTurno.Models
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // Nombre visible
        public string NombreCompleto { get; set; } = string.Empty;

        // Usuario para login
        [Unique, NotNull]
        public string Username { get; set; } = string.Empty;

        [NotNull]
        public string Password { get; set; } = string.Empty;

        // Posibles valores: ADMIN, PACIENTE, FISIOTERAPEUTA
        public string Rol { get; set; } = "PACIENTE";
    }
}



