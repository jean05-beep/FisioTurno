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

        public async Task InitializeAsync()
        {
            await _db.CreateTableAsync<Usuario>();
            await _db.CreateTableAsync<Cita>();
        }

        // LOGIN
        public Task<Usuario?> LoginAsync(string username, string password)
        {
            return _db.Table<Usuario>()
                      .Where(x => x.Username == username && x.Password == password)
                      .FirstOrDefaultAsync();
        }

        // USUARIOS
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

        // -----------------------------
        //     CRUD DE CITAS
        // -----------------------------
        public Task<int> GuardarCitaAsync(Cita c)
        {
            return _db.InsertAsync(c);
        }

        public Task<List<Cita>> ObtenerCitasAsync()
        {
            return _db.Table<Cita>().ToListAsync();
        }

        public Task<List<Cita>> ObtenerCitasPorNombreAsync(string nombre)
        {
            return _db.Table<Cita>()
                      .Where(x => x.NombrePaciente == nombre)
                      .ToListAsync();
        }
    }
}

