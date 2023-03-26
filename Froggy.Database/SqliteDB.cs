using Dapper;
using System.Data.SQLite;

namespace Froggy.Database;

public class SqliteDB : IDapperDB
{
    private string _dbPath;
    private string _createTableScriptPath;
    private string _connectionString;

    public SqliteDB(string dbPath, string createTableScriptPath)
    {
        _dbPath = dbPath;
        _createTableScriptPath = createTableScriptPath;
        _connectionString = $"Data Source={dbPath}";
    }

    public async Task CreateOrCheckDB()
    {
        try
        {
            string? directory = Path.GetDirectoryName(_dbPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory);
            if (!File.Exists(_dbPath))
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    await connection.ExecuteAsync(File.ReadAllText(_createTableScriptPath));
                }
            }
        }
        catch { throw; }
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string tableName, string? condition = null, object? param = null)
    {
        try
        {
            var column_names = typeof(T).GetProperties().Where(x => x.DeclaringType == typeof(T)).Select(x => x.Name);
            string sql = $"SELECT {string.Join(',', column_names)} FROM {tableName}";
            if (!string.IsNullOrEmpty(condition)) sql = sql + $" WHERE {condition}";
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return await connection.QueryAsync<T>(sql, param);
            }
        }
        catch { throw; }
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecound, TReturn>(string sql, Func<TFirst, TSecound, TReturn> mapping, string splitOnID, object? param = null)
    {
        try
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                return await connection.QueryAsync(sql, mapping, param, splitOn: splitOnID);
            }
        }
        catch { throw; }
    }

    public async Task Insert<T>(string tableName, IEnumerable<string> columnNames, T values)
    {
        try
        {
            string value_names = string.Join(',', columnNames.Select(x => '@' + x));
            string sql = $"INSERT INTO {tableName} ({string.Join(',', columnNames)}) VALUES ({value_names})";
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, values);
            }
        }
        catch { throw; }
    }

    public async Task Insert<T>(string tableName, IEnumerable<string> columnNames, IEnumerable<T> values)
    {
        try
        {
            string value_names = string.Join(',', columnNames.Select(x => '@' + x));
            string sql = $"INSERT INTO {tableName} ({string.Join(',', columnNames)}) VALUES ({value_names})";
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, values);
            }
        }
        catch { throw; }
    }

    public async Task Delete(string tableName, string condition, object? param = null)
    {
        try
        {
            string sql = $"DELETE FROM {tableName} WHERE {condition}";
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, param);
            }
        }
        catch { throw; }
    }

    public async Task Update(string tableName, string columnName, object value, string condition, object? param = null)
    {
        try
        {
            if (value != null)
            {
                string update_value = (value is string ? $"'{value}'" : value.ToString())!;
                string sql = $"UPDATE {tableName} SET {columnName} = {update_value} WHERE {condition}";
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    await connection.ExecuteAsync(sql, param);
                }
            }
        }
        catch { throw; }
    }
}
