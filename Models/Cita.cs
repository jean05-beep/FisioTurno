using SQLite;
using Microsoft.Maui.Controls;

namespace FisioTurno.Models
{
    public class Cita
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // ID del paciente (usuario)
        public int PacienteId { get; set; }

        // ID del fisioterapeuta asignado
        public int FisioterapeutaId { get; set; }

        // Datos del paciente
        public string NombrePaciente { get; set; } = string.Empty;

        // Datos del fisioterapeuta
        public string NombreFisioterapeuta { get; set; } = string.Empty;

        // Información de la cita
        public string Fecha { get; set; } = string.Empty;
        public string Hora { get; set; } = string.Empty;
        public string Servicio { get; set; } = string.Empty;
        public string Notas { get; set; } = string.Empty;

        // Estado → Pendiente / Atendida / Cancelada
        public string Estado { get; set; } = "Pendiente";

        // Fecha completa para ordenar
        public DateTime FechaCompleta { get; set; }

        // 📸 Foto enviada a API (Base64)
        public string Foto { get; set; }

        // Ruta proporcionada por API
        public string RutaFoto { get; set; }

        // Imagen lista para mostrar en XAML
        [Ignore]
        public ImageSource FotoImagen { get; set; }
    }
}








