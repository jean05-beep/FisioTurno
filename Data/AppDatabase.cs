using SQLite;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using FisioTurno.Models;

namespace FisioTurno.Data
{
    public class AppDatabase
    {
        private readonly SQLiteAsyncConnection _db;

        public AppDatabase()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "fisioturno.db3");
            _db = new SQLiteAsyncConnection(dbPath);
        }

        // ============================
        //   INICIALIZACIÓN DE TABLAS
        // ============================
        public async Task InitializeAsync()
        {
            await _db.CreateTableAsync<Usuario>();
            await _db.CreateTableAsync<Cita>();
        }

        // ============================
        //            LOGIN
        // ============================
        public Task<Usuario?> LoginAsync(string username, string password)
        {
            return _db.Table<Usuario>()
                      .Where(x => x.Username == username && x.Password == password)
                      .FirstOrDefaultAsync();
        }

        // ============================
        //          USUARIOS
        // ============================
        public async Task<bool> ExisteUsuarioAsync(string username)
        {
            var u = await _db.Table<Usuario>()
                             .Where(x => x.Username == username)
                             .FirstOrDefaultAsync();

            return u != null;
        }

        public Task<int> RegistrarUsuarioAsync(Usuario u)
        {
            return _db.InsertAsync(u);
        }

        // ============================
        //          CITAS
        // ============================

        // Registrar una cita
        public Task<int> GuardarCitaAsync(Cita c)
        {
            return _db.InsertAsync(c);
        }

        // Obtener todas las citas (uso general)
        public Task<List<Cita>> ObtenerCitasAsync()
        {
            return _db.Table<Cita>()
                      .OrderByDescending(c => c.Id)
                      .ToListAsync();
        }

        // Obtener citas por nombre (no recomendado pero lo dejo por compatibilidad)
        public Task<List<Cita>> ObtenerCitasPorNombreAsync(string nombre)
        {
            return _db.Table<Cita>()
                      .Where(x => x.NombrePaciente == nombre)
                      .OrderByDescending(x => x.Id)
                      .ToListAsync();
        }

        // 👉 MÉTODO CORRECTO — CITAS POR PACIENTE ID
        public Task<List<Cita>> ObtenerCitasPorIdPacienteAsync(int pacienteId)
        {
            return _db.Table<Cita>()
                      .Where(c => c.PacienteId == pacienteId)
                      .OrderByDescending(c => c.Id)
                      .ToListAsync();
        }

        // Eliminar cita
        public Task<int> EliminarCitaAsync(Cita c)
        {
            return _db.DeleteAsync(c);
        }

        // Actualizar cita (por si luego agregas estados)
        public Task<int> ActualizarCitaAsync(Cita c)
        {
            return _db.UpdateAsync(c);
        }
    }
}


