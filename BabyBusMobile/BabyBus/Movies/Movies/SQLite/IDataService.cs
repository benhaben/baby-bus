using System.Collections;
using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.SQLite
{
    public interface IDataService
    {
        ITableQuery<T> TableQuery<T>() where T: new();

        int CreateTable<T>();
        int Insert<T>(T parameter);
        void Update<T>(T parameter);
        void Delete<T>(T parameter);
        int Count<T>() where T : new();

        void DeleteAll<T>() where T : new();

        IEnumerable<T> GetItems<T>() where T:new();
        void InsertOrReplace<T>(T kitten);

    }
}