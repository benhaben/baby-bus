using System;
using System.Collections.Generic;
using System.Linq;
using BabyBus.Models;
using BabyBus.Models.Communication;
using BabyBus.Models.Enums;
using BabyBus.Models.Main;
using BabyBus.SQLite;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;

namespace BabyBus.Utilities {
    //this class just a wrapper of sqlite data service
    public class BabyBusContext {
        private static readonly IDataService dataService;

        public static int StackCount = 0;

        static BabyBusContext() { 
            dataService = new DataService();
        }

        #region Current User

        private static  User _currentUser;

        public static int UserId {
            get {
                if (_currentUser != null)
                    return _currentUser.UserId;
                else
                    return 0;
            }
        }

        public static int KindergartenId {
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

        public static int ClassId {
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

        public static int ChildId {
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

        

        public static User UserAllInfo {
            get {
                if (_currentUser != null)
                    return _currentUser;
                var user = dataService.TableQuery<User>().FirstOrDefault(x => x.HasLogin);
                if (user == null || user.UserId == 0) {
                    return null;
                }
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
                        var users = dataService.GetItems<User>().ToList();
                        foreach (var user in users) {
                            user.HasLogin = false;
                            dataService.Update(user);
                        }
                        value.HasLogin = true;
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

        public static void SaveUser() {
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

        public static List<NoticeModel> BaseNoticeList {
            get {
                var list = dataService.GetItems<NoticeModel>()
                    .Where(x => x.KindergartenId == KindergartenId);
                return list.ToList();
            }
        }

        public static List<QuestionModel> QuestionList {
            get {
                var list = dataService.GetItems<QuestionModel>().OrderByDescending(x => x.QuestionId);
                var answers = dataService.GetItems<AnswerModel>().OrderByDescending(x => x.AnswerId);
                foreach (var question in list) {
                    var temp = answers.Where(x => x.QuestionId == question.QuestionId);
                    if (temp.Any()) {
                        question.Answers = temp.ToList();
                    }
                }
                return list.ToList();
            }
        }

        public static List<ChildModel> ChildList {
            get {
                var list = dataService.GetItems<ChildModel>();
                return list.ToList();
            }
        }

        public static List<NoticeModel> NoticeList {
            get {
                var list = BaseNoticeList.Where(x => x.NoticeType != NoticeType.GrowMemory);
                return list.ToList();
            }
        }

        public static List<NoticeModel> GrowMemoryList {
            get {
                var list = BaseNoticeList.Where(x => x.NoticeType == NoticeType.GrowMemory);
                return list.ToList();
            }
        }

        

        public static void Insert<T>(T t) {
            dataService.InsertOrReplace(t);
        }

        public static void InsertAll<T>(List<T> list) {
				foreach (var item in list) {
					dataService.InsertOrReplace (item);
				}
        }

        public static void InsertQuestions(List<QuestionModel> list) {
            foreach (var question in list) {
                dataService.InsertOrReplace(question);
                if (question.Answers != null) {
                    foreach (var answer in question.Answers) {
                        dataService.InsertOrReplace(answer);
                    }
                }
            }
        }

        public static void DeleteQuestions() {
            dataService.DeleteAll<QuestionModel>();  
        }

        public static void ClearInfo() {
            dataService.DeleteAll<NoticeModel>();
            dataService.DeleteAll<QuestionModel>();            
        }

		public static void InsertAttendance(string title,string content) {
            var top = BaseNoticeList
                .Where(x => x.NoticeType != NoticeType.GrowMemory)
                .OrderByDescending(x=>x.CreateTime)
                .FirstOrDefault();
            var model = new NoticeModel() {
                Content = content,
                Title = title,
                NoticeType = NoticeType.ClassEmergency,
                CreateTime = DateTime.Now,
                KindergartenId = KindergartenId,
                ClassId = ClassId,
            };
            if (top != null) {
                model.NoticeId = top.NoticeId + 1;
                dataService.InsertOrReplace(model);
            }
            else {
                model.NoticeId = 1;
                dataService.InsertOrReplace(model);
            }
        }
    }
}
