using SQLite;

namespace FisioTurno.Models
{
    public class Cita
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // ID del usuario (Paciente dueño de la cita)
        public int PacienteId { get; set; }

        // Datos básicos
        public string NombrePaciente { get; set; } = string.Empty;
        public string Fecha { get; set; } = string.Empty;
        public string Hora { get; set; } = string.Empty;
        public string Servicio { get; set; } = string.Empty;
        public string Notas { get; set; } = string.Empty;

        // Estado de la cita: Pendiente, Atendida, Cancelada
        public string Estado { get; set; } = "Pendiente";

        // Fecha completa para ordenamiento
        public DateTime FechaCompleta { get; set; }
    }
}





