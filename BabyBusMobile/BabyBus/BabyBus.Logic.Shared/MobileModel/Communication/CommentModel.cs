using System;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.Logic.Shared
{
    public class CommentModel
    {
        [PrimaryKey]
        public long CommentId { get; set; }

        public int UserId { get; set; }

        public long NoticeId { get; set; }

        public string Content { get; set; }

        public DateTimeOffset CreateTime { get; set; }

        public  CommentType CommentType{ get; set; }

        public int ChildId { get; set; }

        public int ParentId { get; set; }
    }
}

