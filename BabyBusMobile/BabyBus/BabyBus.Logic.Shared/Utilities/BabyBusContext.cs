using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.Sqlite;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;
using BabyBusSSApi.ServiceModel.DTO.Create;

namespace BabyBus.Logic.Shared
{
	//this class just a wrapper of sqlite data service
	public class BabyBusContext
	{
		private static readonly IDataService dataService;

		public static int StackCount = 0;

		static BabyBusContext()
		{ 
			dataService = new DataService();
		}

		#region Current User

		private static UserModel _currentUser;

		public static long UserId {
			get {
				if (_currentUser != null)
					return _currentUser.UserId;
				else
					return 0;
			}
		}

		public static int  WidthInDp { get; set; }

		public static int  HeightInDp{ get; set; }

		public static bool IsMember {
			get { 
				if (_currentUser != null)
					return _currentUser.IsMember;
				else
					return false;
			}
		}

		public static long KindergartenId {
			get {
				if (_currentUser != null)
					return _currentUser.KindergartenId;
				else
					return 0;
			}
		}

		public static KindergartenModel Kindergarten {
			get {
				if (_currentUser != null && _currentUser.Kindergarten != null) {
					return _currentUser.Kindergarten;
				} else {
					return new KindergartenModel();
				}
			}
		}

		public static long ClassId {
			get {
				if (_currentUser != null)
					return _currentUser.ClassId;
				else
					return 0;
			}
		}

		public static KindergartenClassModel Class {
			get {
				if (_currentUser != null && _currentUser.Class != null) {
					return _currentUser.Class;
				} else {
					return new KindergartenClassModel();
				}
			}
		}

		public static long ChildId {
			get {
				if (_currentUser != null)
					return _currentUser.ChildId;
				else
					return 0;
			}
		}

		public static RoleType RoleType {
			get {
				if (_currentUser != null)
					return _currentUser.RoleType;
				else
					return RoleType.Parent;
			}
		}

		public static bool  IsAvailable {
			get;
			set;
		}

		public static bool UpdateStatus {
			get {
				if (_currentUser != null)
					return _currentUser.UpdateStatus;
				else
					return true;
			}
			set {
				if (_currentUser != null)
					_currentUser.UpdateStatus = value;
			}
		}

		public static UserModel UserAllInfo {
			get {
				if (_currentUser != null)
					return _currentUser;//如果不是第一次进入到该页面，则直接返回用户名信息
				var user = dataService.TableQuery<UserModel>().FirstOrDefault();//凑个服务器获取连接
				if (user == null || user.UserId == 0) {
					return null;
				}//用户安全检查
				user.Child = dataService.TableQuery<ChildModel>().FirstOrDefault(x => x.ChildId == user.ChildId);
				user.Class = dataService.TableQuery<KindergartenClassModel>().FirstOrDefault(x => x.ClassId == user.ClassId);
				user.Kindergarten =
                    dataService.TableQuery<KindergartenModel>()
                        .FirstOrDefault(x => x.KindergartenId == user.KindergartenId);
				_currentUser = user;

				return user;
			}
			set {
				try {
					if (value == null && _currentUser != null) {
						dataService.Delete(_currentUser);
					} else {
						var users = dataService.GetItems<UserModel>().ToList();
                      
						//User
						dataService.InsertOrReplace(value);
						//Child
						if (value.Child != null) {
							dataService.InsertOrReplace(value.Child);
						}
						//Class
						if (value.Class != null) {
							dataService.InsertOrReplace(value.Class);
						}
						//Kindergarten
						if (value.Kindergarten != null) {
							dataService.InsertOrReplace(value.Kindergarten);
						}
						
					}
				} catch (Exception e) {
					Mvx.Trace(MvxTraceLevel.Error, e.Message);
					throw;
				}
				_currentUser = value;
			}
		}

		public static void SaveUser()
		{
			//User
			dataService.InsertOrReplace(_currentUser);
			//Child
			if (_currentUser.Child != null) {
				dataService.InsertOrReplace(_currentUser.Child);
			}
			//Class
			if (_currentUser.Class != null) {
				dataService.InsertOrReplace(_currentUser.Class);
			}
			//Kindergarten
			if (_currentUser.Kindergarten != null) {
				dataService.InsertOrReplace(_currentUser.Kindergarten);
			}
		}

		#endregion

		public static ITableQuery<NoticeModel> BaseNoticeList {
			get {
				var list = dataService.GetItems<NoticeModel>()
                    .Where(x => x.KindergartenId == KindergartenId);
				return list;
			}
		}

		public static ITableQuery<ECPostInfo> BaseECPostInfoList {
			get {
				var list = dataService.GetItems<ECPostInfo>();
				return list;
			}
		}
		//		public static ITableQuery<AdvertisementModel> BaseAdvertisementList
		//		{
		//			get
		//			{
		//				var list =  dataService.GetItems<AdvertisementModel>();
		//				return list;
		//			}
		//		}
		public static List<QuestionModel> QuestionList {
			get {
				//TODO: Need Refactork. Entity contains a List of Entities, that will be taked off in the furture.
				var list = dataService.GetItems<QuestionModel>().OrderByDescending(x => x.QuestionId).ToList();
				var answers = dataService.GetItems<AnswerModel>().OrderByDescending(x => x.AnswerId);
				foreach (var question in list) {
					var temp = answers.Where(x => x.QuestionId == question.QuestionId);
					if (temp.Any()) {
						question.Answers = temp.ToList();
					}
				}
				return list;
			}
		}

		public static List<ECReview> ECReviewList {
			get { 
				var list = dataService.GetItems<ECReview>();
				return list.ToList();
			}
		}

		public static List<ECComment> ECCommentList {
			get { 
				var list = dataService.GetItems<ECComment>();
				return list.ToList();
			}
		}

		public static List<ChildModel> ChildList {
			get {
				var list = dataService.GetItems<ChildModel>();
				return list.ToList();
			}
		}

		public static ITableQuery<NoticeModel> NoticeList {
			get {
				var list = 
					BaseNoticeList.Where(x => x.NoticeType != NoticeType.GrowMemory);
				return list;
			}
		}

		public static ITableQuery<ECPostInfo> ECCourceInfoList {
			get {
				var list = BaseECPostInfoList.Where(x => x.ColumnType == (int)ECColumnType.Course);
				return list;
			}
		}

		public static ITableQuery<ECPostInfo> ECActivityInfoList {
			get {
				var list = BaseECPostInfoList.Where(x => x.ColumnType == (int)ECColumnType.Activity);
				return list;
			}
		}

		public static ITableQuery<ECPostInfo> ECPostInfoList {
			get {
				var list = BaseECPostInfoList;
				return list;
			}
		}
		//		public static List<AdvertisementModel> AdvertisementList
		//		{
		//			get
		//			{
		//				var list = BaseAdvertisementList;
		//				return list.ToList();
		//			}
		//		}
		public static ITableQuery<ECPostInfo> ECForumInfoList {
			get {
				var list = BaseECPostInfoList.Where(x => x.ColumnType == (int)ECColumnType.Forum);
				return list;
			}
		}

		public static ITableQuery<NoticeModel> GrowMemoryList {
			get {
				var list = BaseNoticeList.Where(x => x.NoticeType == NoticeType.GrowMemory);
				return list;
			}
		}

		public static void Insert<T>(T t)
		{
			try {
				dataService.InsertOrReplace(t);
			} catch (Exception ex) {
				//Insights.Report (ex, Insights.Severity.Error);
			}
		}

		public static void Update<T>(T t)
		{
			try {
				dataService.Update(t);
			} catch (Exception ex) {
				//Insights.Report (ex, Insights.Severity.Error);
			}
		}

		public static void InsertAll<T>(List<T> list)
		{
			foreach (var item in list) {
				dataService.InsertOrReplace(item);
			}
		}

		public static void InsertQuestions(List<QuestionModel> list)
		{
			foreach (var question in list) {
				dataService.InsertOrReplace(question);
				if (question.Answers != null) {
					foreach (var answer in question.Answers) {
						dataService.InsertOrReplace(answer);
					}
				}
			}
		}

		public static void DeleteQuestions()
		{
			dataService.DeleteAll<QuestionModel>();  
		}

		public static void ClearInfo()
		{
			dataService.DeleteAll<NoticeModel>();
			dataService.DeleteAll<QuestionModel>();   
			dataService.DeleteAll<ECPostInfo>();  
		}

		//        public static void InsertAttendance(string title, string content)
		//        {
		//            var top = BaseNoticeList
		//                .Where(x => x.NoticeType != NoticeType.GrowMemory)
		//                .OrderByDescending(x => x.CreateTime)
		//                .FirstOrDefault();
		//            var model = new NoticeModel()
		//            {
		//                Content = content,
		//                Title = title,
		//                NoticeType = NoticeType.ClassEmergency,
		//                CreateTime = DateTime.Now,
		//                KindergartenId = KindergartenId,
		//                ClassId = ClassId,
		//            };
		//            if (top != null)
		//            {
		//                model.NoticeId = top.NoticeId + 1;
		//                dataService.InsertOrReplace(model);
		//            }
		//            else
		//            {
		//                model.NoticeId = 1;
		//                dataService.InsertOrReplace(model);
		//            }
		//        }
	}
}
