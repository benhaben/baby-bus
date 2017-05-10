using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BabyBus.Helpers;
using BabyBus.Models;
using BabyBus.Models.Communication;
using BabyBus.Services;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;

//using SQLite.Net;
using SQLiteNetExtensions.Extensions;

//using Cirrious.MvvmCross.Plugins.Sqlite;

using System.Text;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.SQLite {
    public class DataService: IDataService {
        #region private

        private ISQLiteConnection _connection;

        //        private SQLiteConnection _connection;
        //        private readonly ISqlitePlatformService _idbService;
        private string _destinationPath;

        private readonly IEnvironmentService _environment;

        private void CreateConnection() {
            _destinationPath = _environment.GetDateBaseFolderPath("babybus.db");
            var factory = Mvx.Resolve<ISQLiteConnectionFactory>();
            _connection = factory.Create("babybus.db");            
            
//            _connection = new SQLiteConnection(_idbService.GetLitePlatform(), _destinationPath);
        }

        public void InitDatabaseIfNotExists() {
            
            _connection.CreateTable<User>();
            _connection.CreateTable<CityModel>();
            _connection.CreateTable<ChildModel>();
            _connection.CreateTable<KindergartenModel>();
            _connection.CreateTable<KindergartenClassModel>();
            //DB Version Update
            try {
                _connection.CreateTable<NoticeModel>();
            } catch (Exception ex) {
                _connection.DropTable<NoticeModel>();
                _connection.CreateTable<NoticeModel>();
            }
            try {
                _connection.CreateTable<QuestionModel>();
            } catch (Exception ex) {
                _connection.DropTable<QuestionModel>();
                _connection.CreateTable<QuestionModel>();
            }
            try {
                _connection.CreateTable<AnswerModel>();
            } catch (Exception ex) {
                _connection.DropTable<AnswerModel>();
                _connection.CreateTable<AnswerModel>();
            }
        }

        private void InitDatabase() {
            try {
                //TODO:db always is empty when virtual machine is reload?
                if (_environment.FileExists(_destinationPath)) {
                } else {
                    //create db,tables
                    _connection.DropTable<User>();
                    _connection.DropTable<CityModel>();
                    _connection.DropTable<ChildModel>();
                    _connection.DropTable<KindergartenModel>();
                    _connection.DropTable<KindergartenClassModel>();
                    _connection.DropTable<NoticeModel>();


                    _connection.CreateTable<User>();
                    _connection.CreateTable<CityModel>();
                    _connection.CreateTable<ChildModel>();
                    _connection.CreateTable<KindergartenModel>();
                    _connection.CreateTable<KindergartenClassModel>();
                    _connection.CreateTable<NoticeModel>();
                    _connection.CreateTable<QuestionModel>();
                    _connection.CreateTable<AnswerModel>();
					_connection.CreateTable<CommentModel>();

                    //create init data to test
                    var user = new User();
                    user.LoginName = "yin";
                    _connection.Insert(user);
                    var city = new CityModel();
                    city.CityName = "上海";
                    _connection.Insert(city);
                    user.CityId = city.CityId;
                    _connection.Update(user);
                    //Init Notice Data To Test
//					CreateNoticeTestData ();

#if DEBUG
                    int i = _connection.Table<User>().Count();
                    Debug.Assert(i == 1);
                    var user1 = _connection.Table<User>().FirstOrDefault();
                    _connection.GetChildren(user1);
                    _connection.GetChildren(user);
                    Debug.Assert(user.City != null);
                    Debug.Assert(user.CityId == user1.CityId);
#endif
                }
            } catch (Exception e) {
                Mvx.Trace(MvxTraceLevel.Error, e.Message);
                Debug.Assert(false);
            }
        }

       
        #endregion

        public DataService() {
            _environment = Mvx.Resolve<IEnvironmentService>();
            //TODO：放到另一个线程如果启动速度受到影响
            CreateConnection();
            InitDatabaseIfNotExists();
//			InitDatabase();
            #if DEBUG
            _connection.Trace = true;
            #endif
        }

        public int CreateTable<T>() {
            return _connection.CreateTable<T>();
        }

        public int Insert<T>(T kitten) {
            return _connection.Insert(kitten);
        }

        public void Update<T>(T kitten) {
            _connection.Update(kitten);
        }

        public void Delete<T>(T kitten) {
            _connection.Delete(kitten);
        }

        public void InsertOrReplace<T>(T kitten) {
            _connection.InsertOrReplace(kitten);
        }

        public ITableQuery<T> TableQuery<T>() where T : new() {
            return _connection.Table<T>();  
            //.OrderBy<int>(x => x.ID)
            //.Where(x => x.ChildName.Contains(nameFilter))
            //.ToList();
        }

        //Unused
        public int Count<T>()where T:new() {
            return _connection.Table<T>().Count();
        }

        public void DeleteAll<T>() where T : new() {
            var list = GetItems<T>();
            _connection.DeleteAll(list);
        }

        //Unused
        public IEnumerable<T> GetItems<T>() where T : new() {
            var enumerator = (from i in TableQuery<T>()
                                       select i);
            if (enumerator.Count() > 0) {
                return enumerator.ToList();
            } else {
                return new List<T>();
            }
        }

    }

}