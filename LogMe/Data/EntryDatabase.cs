using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using LogMe.Models;

namespace LogMe.Data
{
    public class EntryDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public EntryDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<LogEntry>().Wait();
            _database.CreateTableAsync<FlareTrigger>().Wait();
            _database.CreateTableAsync<AffectedArea>().Wait();
            _database.CreateTableAsync<LogEntryTrigger>().Wait();
            _database.CreateTableAsync<LogEntryAffectedArea>().Wait();
        }

        public Task<List<T>> GetEntryAsync<T>(bool withChildren = false) where T: class, IEntry, new()
        {
            return withChildren ? _database.GetAllWithChildrenAsync<T>() : _database.Table<T>().ToListAsync();
        }

        public Task<T> GetEntryAsync<T>(int id, bool withChildren = true) where T : class, IEntry, new()
        {
            return withChildren ? _database.GetWithChildrenAsync<T>(id) : _database.GetAsync<T>(id);
        }

        public async Task<bool> SaveEntryAsync<T>(T entry) where T : class, IEntry, new()
        {
            bool insertSuccess;
            try
            {
                insertSuccess = entry.ID != null || await _database.InsertAsync(entry) > 0;
            }
            catch (SQLiteException)
            {
                return false;
            }
            if (insertSuccess)
            {
                await _database.UpdateWithChildrenAsync(entry);
            }
            return insertSuccess;
        }

        public Task<int> DeleteEntryAsync<T>(T entry) where T : class, IEntry, new()
        {
            return _database.DeleteAsync<T>(entry.ID);
        }
    }
}
