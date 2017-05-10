using System;
using System.Collections.Generic;
using BabyBus.Models.Enums;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.Models.Communication {
    public class NoticeModel {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public long NoticeId { get; set; }

        public int KindergartenId { get; set; }

        public int ClassId { get; set; }

        public NoticeType NoticeType { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public string ThumPics { get; set; }

        public string NormalPics { get; set; }

        public int ReceiverNumber { get; set; }

        public int FavoriteCount { get; set; }

        public int ReadedCount { get; set; }

        public int ConfirmedCount { get; set; }

        public int UserId { get; set; }

        public string RealName { get; set; }

        [Ignore]
        public List<int> ChildID{ get; set; }

        //show in the UI
        public string ChildrenName{ get; set; }

        [Ignore]
        public int ImageCount { get; set; }

        [Ignore]
        public string FirstImage {
            get { 
                string name;
                string pathAndName;
                if (!string.IsNullOrEmpty(NormalPics)) {
                    name = NormalPics.Split(',')[0];
                    if (!string.IsNullOrEmpty(name)) {
//                        pathAndName = Constants.ThumbServerPath + name + Constants.ThumbRule;
                        pathAndName = name;

                    } else {
                        pathAndName = "";
                    }
                } else {
                    pathAndName = "";
                }
                return pathAndName;
            }
        }

        [Ignore]
        public string Abstract {
            get {
                if (!string.IsNullOrEmpty(Content)) {
                    if (Content.Length >= 18)
                        return Content.Substring(0, 18) + "...";
                    else
                        return Content;
                } else {
                    return string.Empty;
                }
            }
        }

        [Ignore]
        public List<string> ImageList {
            get {
                if (!string.IsNullOrEmpty(NormalPics)) {
                    return new List<string>(NormalPics.Split(','));
                } else {
                    return null;
                }
            }
        }
    }
}
