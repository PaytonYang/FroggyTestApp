namespace Froggy.Database
{
    public interface IDapperDB
    {
        Task CreateOrCheckDB();
        Task Delete(string tableName, string condition, object? param = null);
        Task Insert<T>(string tableName, IEnumerable<string> columnNames, IEnumerable<T> values);
        Task Insert<T>(string tableName, IEnumerable<string> columnNames, T values);
        Task<IEnumerable<T>> QueryAsync<T>(string tableName, string? condition = null, object? param = null);
        Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecound, TReturn>(string sql, Func<TFirst, TSecound, TReturn> mapping, string splitOnID, object? param = null);
        Task Update(string tableName, string columnName, object value, string condition, object? param = null);
    }
}