using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using LogMe.Models;

namespace LogMe.Data
{
    public class LogItemDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public LogItemDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<LogItem>().Wait();
        }

        public Task<List<LogItem>> GetLogItemsAsync()
        {
            return _database.Table<LogItem>().ToListAsync();
        }

        public Task<LogItem> GetLogItemAsync(int id)
        {
            return _database.Table<LogItem>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveLogItemAsync(LogItem logItem)
        {
            if (logItem.ID != 0)
            {
                return _database.UpdateAsync(logItem);
            }
            else
            {
                return _database.InsertAsync(logItem);
            }
        }

        public Task<int> DeleteLogItemAsync(LogItem logItem)
        {
            return _database.DeleteAsync(logItem);
        }
    }
}
