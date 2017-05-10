using System;
using System.Collections.Generic;
using BabyBus.Model.Entities.Article.Enum;
using cn.jpush.api;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;

namespace BabyBus.API.Utils {
    public class JPushUtils {
        private static readonly string Parent_AppKey = Config.Parent_AppKey;
        private static readonly string Parent_ApiMaster = Config.Parent_ApiMaster;
        private static readonly string Teacher_AppKey = Config.Teacher_AppKey;
        private static readonly string Teacher_ApiMaster = Config.Teacher_ApiMaster;
        private static readonly string Master_AppKey = Config.Master_AppKey;
        private static readonly string Master_ApiMaster = Config.Master_ApiMaster;
        private static readonly Dictionary<NoticeType, string> MessageTypeMap = new Dictionary<NoticeType, string>();

        private const string Tag_Notice = "Notice";
        private const string Tag_GrowMemory = "GrowMemory";
        private const string Tag_Question = "Question";
        private const string Tag_Attendance = "Attendance";
        private const string Msg_Attendance = "您家宝宝已到幼儿园";
        private const string Msg_UnAttendance = "您家宝宝未到幼儿园";
        

        static JPushUtils() {
            MessageTypeMap.Add(NoticeType.ClassEmergency, "用户登录");
            MessageTypeMap.Add(NoticeType.ClassHomework, "家庭作业");
            MessageTypeMap.Add(NoticeType.ClassCommon, "班级通知");
            MessageTypeMap.Add(NoticeType.KindergartenAll, "园区通知");
            MessageTypeMap.Add(NoticeType.KindergartenStaff, "园务通知");
            MessageTypeMap.Add(NoticeType.GrowMemory, "成长记忆");
        }


        public static void PushMessage(NoticeType noticeType, string content, int kgId, int classId, long noticeId) {
            var payload = new PushPayload();
            //Platform
            payload.platform = Platform.all();
            //Audience
            if (noticeType == NoticeType.ClassCommon || noticeType == NoticeType.ClassEmergency
                || noticeType == NoticeType.ClassHomework || noticeType == NoticeType.GrowMemory) {
                payload.audience = Audience.s_tag_and(
                    string.Format("Kindergarten_{0}", kgId), string.Format("Class_{0}", classId));
            }
            else if (noticeType == NoticeType.KindergartenAll || noticeType == NoticeType.KindergartenStaff) {
                payload.audience = Audience.s_tag(
                    string.Format("Kindergarten_{0}", kgId));
            }
            //Notification
            string typeValue = noticeType == NoticeType.GrowMemory ? Tag_GrowMemory : Tag_Notice;

            Notification notification = new Notification().setAlert(content);
            notification.AndroidNotification = new AndroidNotification().setTitle(MessageTypeMap[noticeType]);
            notification.AndroidNotification.AddExtra("Tag", typeValue);
            notification.AndroidNotification.AddExtra("NoticeId", noticeId.ToString());
            notification.IosNotification = new IosNotification();
            notification.IosNotification.incrBadge(1);
            notification.IosNotification.AddExtra("Tag", typeValue);
            notification.IosNotification.AddExtra("NoticeId", noticeId.ToString());
            payload.options.apns_production = true;

            payload.notification = notification.Check();
            if (noticeType != NoticeType.KindergartenStaff) {
                try {
                    //Parent
                    var client1 = new JPushClient(Parent_AppKey, Parent_ApiMaster);
                    client1.SendPush(payload);
                }
                catch {
                }
            }
            if (noticeType == NoticeType.KindergartenAll || noticeType == NoticeType.KindergartenStaff) {
                try {
                    //Master
                    var client3 = new JPushClient(Master_AppKey, Master_ApiMaster);
                    client3.SendPush(payload);
                }
                catch {

                }
            }
            //Teacher
            try {
                var client2 = new JPushClient(Teacher_AppKey, Teacher_ApiMaster);
                client2.SendPush(payload);
            }
            catch {
            }
        }

        public static void PushQuestion(string content, string[] users, int questionId) {
            var payload = new PushPayload();
            //Platform
            payload.platform = Platform.all();
            var hashSet = new HashSet<string>();
            foreach (string user in users) {
                hashSet.Add("User_" + user);
            }
            payload.audience = Audience.s_alias(hashSet);

            Notification notification = new Notification().setAlert(content);
            notification.AndroidNotification = new AndroidNotification().setTitle("提问");
            notification.AndroidNotification.AddExtra("Tag", Tag_Question);
            notification.AndroidNotification.AddExtra("QuestionId", questionId.ToString());
            notification.IosNotification = new IosNotification();
            notification.IosNotification.incrBadge(1);
            notification.IosNotification.AddExtra("Tag", Tag_Question);
            notification.IosNotification.AddExtra("QuestionId", questionId.ToString());
            payload.options.apns_production = true;

            payload.notification = notification.Check();
            try {
                //Teacher
                var client2 = new JPushClient(Teacher_AppKey, Teacher_ApiMaster);
                client2.SendPush(payload);
            }
            catch (Exception ex) {
            }
        }

        public static void PushAnswer(string content, string[] users, int questionId) {
            var payload = new PushPayload();
            //Platform
            payload.platform = Platform.all();
            var hashSet = new HashSet<string>();
            foreach (string user in users) {
                hashSet.Add("User_" + user);
            }
            payload.audience = Audience.s_alias(hashSet);

            Notification notification = new Notification().setAlert(content);
            notification.AndroidNotification = new AndroidNotification().setTitle("回答");
            notification.AndroidNotification.AddExtra("Tag", Tag_Question);
            notification.AndroidNotification.AddExtra("QuestionId", questionId.ToString());
            notification.IosNotification = new IosNotification();
            notification.IosNotification.incrBadge(1);
            notification.IosNotification.AddExtra("Tag", Tag_Question);
            notification.IosNotification.AddExtra("QuestionId", questionId.ToString());
            payload.options.apns_production = true;

            payload.notification = notification.Check();
            try {
                //Parent
                var client1 = new JPushClient(Parent_AppKey, Parent_ApiMaster);
                client1.SendPush(payload);
            }
            catch (Exception ex) {
            }
        }

        public static void PushAttendance(string[] attUserList, string[] unattUserList) {
            //Attendance User
            var attPayload = new PushPayload();
            //Platform
            attPayload.platform = Platform.all();
            var hashSet = new HashSet<string>();
            foreach (string user in attUserList) {
                hashSet.Add("User_" + user);
            }
            var date = DateTime.Now.ToString("D") + "的考勤";
            attPayload.audience = Audience.s_alias(hashSet);
            Notification notification = new Notification().setAlert(Msg_Attendance);
            notification.AndroidNotification = new AndroidNotification().setTitle(date);
            notification.AndroidNotification.AddExtra("Tag", Tag_Attendance);
            notification.AndroidNotification.AddExtra("Title", date);
            notification.AndroidNotification.AddExtra("Content", Msg_Attendance);
            notification.IosNotification = new IosNotification();
            notification.IosNotification.incrBadge(1);
            notification.IosNotification.AddExtra("Tag", Tag_Attendance);
            attPayload.options.apns_production = true;
            attPayload.notification = notification.Check();

            //UnAttendance User
            var unAttPayload = new PushPayload();
            //Platform
            attPayload.platform = Platform.all();
            var hashSet1 = new HashSet<string>();
            foreach (string user in attUserList) {
                hashSet1.Add("User_" + user);
            }
            attPayload.audience = Audience.s_alias(hashSet1);
            Notification notification2 = new Notification().setAlert(Msg_UnAttendance);
            notification2.AndroidNotification = new AndroidNotification().setTitle(date);
            notification2.AndroidNotification.AddExtra("Tag", Tag_Attendance);
            notification2.AndroidNotification.AddExtra("Title", date);
            notification2.AndroidNotification.AddExtra("Content", Msg_UnAttendance);
            notification2.IosNotification = new IosNotification();
            notification2.IosNotification.incrBadge(1);
            notification2.IosNotification.AddExtra("Tag", Tag_Attendance);
            unAttPayload.options.apns_production = true;
            unAttPayload.notification = notification.Check();
            //Parent
            var client1 = new JPushClient(Parent_AppKey, Parent_ApiMaster);
            try {
                client1.SendPush(attPayload);
            }
            catch (Exception ex) {
            }
            try {
                client1.SendPush(unAttPayload);
            }
            catch (Exception ex) {
            }
        }
    }
}