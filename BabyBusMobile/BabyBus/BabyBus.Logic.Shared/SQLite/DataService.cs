using System;
using System.Diagnostics;



using BabyBus.Logic.Shared;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;

//using SQLite.Net;

//using Cirrious.MvvmCross.Plugins.Sqlite;

using System.Text;
using Cirrious.MvvmCross.Plugins.Sqlite;


namespace BabyBus.Logic.Shared
{
	public class DataService: IDataService
	{
		#region private

		private ISQLiteConnection _connection;

		//        private SQLiteConnection _connection;
		//        private readonly ISqlitePlatformService _idbService;
		private string _destinationPath;

		private readonly IEnvironmentService _environment;

		private void CreateConnection()
		{
			_destinationPath = _environment.GetDateBaseFolderPath("babybus.db");
			var factory = Mvx.Resolve<ISQLiteConnectionFactory>();
			_connection = factory.Create("babybus.db");  

            
//            _connection = new SQLiteConnection(_idbService.GetLitePlatform(), _destinationPath);
		}

		public void InitDatabaseIfNotExists()
		{
			try {
				_connection.CreateTable<UserModel>();
			} catch (Exception ex) {
				_connection.DropTable<UserModel>();
				_connection.CreateTable<UserModel>();
			}

			try {
				_connection.CreateTable<ChildModel>();
			} catch (Exception ex) {
				_connection.DropTable<ChildModel>();
				_connection.CreateTable<ChildModel>();
			}

			try {
				_connection.CreateTable<KindergartenModel>();
			} catch (Exception ex) {
				_connection.DropTable<KindergartenModel>();
				_connection.CreateTable<KindergartenModel>();
			}
			try {
				_connection.CreateTable<KindergartenClassModel>();
			} catch (Exception ex) {
				_connection.DropTable<KindergartenClassModel>();
				_connection.CreateTable<KindergartenClassModel>();
			}
			
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
			try {
				_connection.CreateTable<ECReview>();
			} catch (Exception ex) {
				_connection.DropTable<ECReview>();
				_connection.CreateTable<ECReview>();
			}	
			try {
				_connection.CreateTable<ECPostInfo>();
			} catch (Exception ex) {
				_connection.DropTable<ECPostInfo>();
				_connection.CreateTable<ECPostInfo>();
			}
			try {
				_connection.CreateTable<ECComment>();
			} catch (Exception ex) {
				_connection.DropTable<ECComment>();
				_connection.CreateTable<ECComment>();
			}
//			try {
//				_connection.CreateTable<AdvertisementModel>();
//			} catch (Exception ex) {
//				_connection.DropTable<AdvertisementModel>();
//				_connection.CreateTable<AdvertisementModel>();
//			}
		}

		private void InitDatabase()
		{
			try {
				//TODO:db always is empty when virtual machine is reload?
				if (_environment.FileExists(_destinationPath)) {
				} else {
					//create db,tables
					_connection.DropTable<UserModel>();
//                    _connection.DropTable<CityModel>();
					_connection.DropTable<ChildModel>();
					_connection.DropTable<KindergartenModel>();
					_connection.DropTable<KindergartenClassModel>();
					_connection.DropTable<NoticeModel>();


					_connection.CreateTable<UserModel>();
//                    _connection.CreateTable<CityModel>();
					_connection.CreateTable<ChildModel>();
					_connection.CreateTable<KindergartenModel>();
					_connection.CreateTable<KindergartenClassModel>();
					_connection.CreateTable<NoticeModel>();
					_connection.CreateTable<QuestionModel>();
					_connection.CreateTable<AnswerModel>();

				}
			} catch (Exception e) {
				Mvx.Trace(MvxTraceLevel.Error, e.Message);
				Debug.Assert(false);
			}
		}

       
		#endregion

		public DataService()
		{
			_environment = Mvx.Resolve<IEnvironmentService>();
			//TODO：放到另一个线程如果启动速度受到影响
			CreateConnection();
			InitDatabaseIfNotExists();
//			InitDatabase();
			#if DEBUG
			_connection.Trace = true;
			#endif
		}

		public int CreateTable<T>()
		{
			return _connection.CreateTable<T>();
		}

		public int Insert<T>(T kitten)
		{
			return _connection.Insert(kitten);
		}

		public void Update<T>(T kitten)
		{
			_connection.Update(kitten);
		}

		public void Delete<T>(T kitten)
		{
			_connection.Delete(kitten);
		}

		public void InsertOrReplace<T>(T kitten)
		{
			_connection.InsertOrReplace(kitten);
		}

		public ITableQuery<T> TableQuery<T>() where T : new()
		{
			return _connection.Table<T>();
		}

		//Unused
		public int Count<T>()where T:new()
		{
			return _connection.Table<T>().Count();
		}

		public void DeleteAll<T>() where T : new()
		{
			var list = TableQuery<T>();
			foreach (var o in list) {
				_connection.Delete(o);
			}
		}

		//Unused
		public ITableQuery<T> GetItems<T>() where T : new()
		{
			var enumerator = (from i in TableQuery<T>()
			                  select i);
			return enumerator;
		}

	}

}