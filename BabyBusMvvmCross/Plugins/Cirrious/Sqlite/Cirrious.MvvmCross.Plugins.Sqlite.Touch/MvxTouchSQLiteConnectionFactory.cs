// MvxTouchSQLiteConnectionFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.IO;
using SQLite;

namespace Cirrious.MvvmCross.Plugins.Sqlite.Touch {
    public class MvxTouchSQLiteConnectionFactory : ISQLiteConnectionFactory {
        public ISQLiteConnection Create(string address) {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            path = Path.Combine(path, address);
            SQLite3.Config(SQLite3.ConfigOption.Serialized);

            return new SQLiteConnection(path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex, true);
        }
    }
}