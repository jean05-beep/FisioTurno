using SQLite;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using FisioTurno.Models;
using System.Diagnostics;

namespace FisioTurno.Data
{
    public class AppDatabase
    {
        private readonly SQLiteAsyncConnection _db;

        public AppDatabase()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "fisioturno.db3");

            Debug.WriteLine("=====================================");
            Debug.WriteLine("📌 Ruta real BD:");
            Debug.WriteLine(dbPath);
            Debug.WriteLine("=====================================");

            _db = new SQLiteAsyncConnection(dbPath);
        }

        // ============================
        //   INICIALIZACIÓN DE TABLAS
        // ============================
        public async Task InitializeAsync()
        {
            await _db.CreateTableAsync<Usuario>();
            await _db.CreateTableAsync<Cita>();

            // Crear admin si no existe
            var admin = await _db.Table<Usuario>()
                                 .Where(x => x.Rol == "ADMIN")
                                 .FirstOrDefaultAsync();

            if (admin == null)
            {
                var nuevoAdmin = new Usuario
                {
                    NombreCompleto = "Administrador General",
                    Username = "admin",
                    Password = "admin123",
                    Rol = "ADMIN"
                };

                await _db.InsertAsync(nuevoAdmin);
                Debug.WriteLine("✔ ADMIN creado");
            }
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

        // ✔ Obtener SOLO fisioterapeutas
        public Task<List<Usuario>> ObtenerFisioterapeutasAsync()
        {
            return _db.Table<Usuario>()
                      .Where(u => u.Rol == "FISIOTERAPEUTA")
                      .ToListAsync();
        }

        // ✔ ACTUALIZAR Fisioterapeuta
        public Task<int> ActualizarUsuarioAsync(Usuario u)
        {
            return _db.UpdateAsync(u);
        }

        // ✔ ELIMINAR Fisioterapeuta
        public Task<int> EliminarUsuarioAsync(Usuario u)
        {
            return _db.DeleteAsync(u);
        }

        // ============================
        //          CITAS
        // ============================
        public Task<int> GuardarCitaAsync(Cita c)
        {
            return _db.InsertAsync(c);
        }

        public Task<List<Cita>> ObtenerCitasAsync()
        {
            return _db.Table<Cita>()
                      .OrderByDescending(c => c.Id)
                      .ToListAsync();
        }

        public Task<List<Cita>> ObtenerCitasPorNombreAsync(string nombre)
        {
            return _db.Table<Cita>()
                      .Where(x => x.NombrePaciente == nombre)
                      .OrderByDescending(x => x.Id)
                      .ToListAsync();
        }

        public Task<List<Cita>> ObtenerCitasPorIdPacienteAsync(int pacienteId)
        {
            return _db.Table<Cita>()
                      .Where(c => c.PacienteId == pacienteId)
                      .OrderByDescending(c => c.Id)
                      .ToListAsync();
        }

        public Task<int> EliminarCitaAsync(Cita c)
        {
            return _db.DeleteAsync(c);
        }

        public Task<int> ActualizarCitaAsync(Cita c)
        {
            return _db.UpdateAsync(c);
        }
    }
}

