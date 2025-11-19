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

        public async Task InitializeAsync()
        {
            await _db.CreateTableAsync<Usuario>();
        }

        public Task<Usuario?> LoginAsync(string u, string p)
        {
            return _db.Table<Usuario>().FirstOrDefaultAsync(
                x => x.Username == u && x.Password == p
            );
        }

        public Task<int> CrearUsuarioAsync(Usuario u)
        {
            return _db.InsertAsync(u);
        }
    }
}
