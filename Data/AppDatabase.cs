using SQLite;
using System.IO;
using System.Threading.Tasks;
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

        // Crear tablas
        public async Task InitializeAsync()
        {
            await _db.CreateTableAsync<Usuario>();
        }

        // Login
        public Task<Usuario?> LoginAsync(string username, string password)
        {
            return _db.Table<Usuario>()
                      .Where(x => x.Username == username && x.Password == password)
                      .FirstOrDefaultAsync();
        }

        // Ver si el usuario existe
        public async Task<bool> ExisteUsuarioAsync(string username)
        {
            var result = await _db.Table<Usuario>()
                                  .Where(x => x.Username == username)
                                  .FirstOrDefaultAsync();

            return result != null;
        }

        // Registrar usuario
        public Task<int> RegistrarUsuarioAsync(Usuario u)
        {
            return _db.InsertAsync(u);
        }
    }
}
