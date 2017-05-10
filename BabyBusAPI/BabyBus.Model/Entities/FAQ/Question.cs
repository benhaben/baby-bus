using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BabyBus.Model.Entities.Main;
using Newtonsoft.Json;

namespace BabyBus.Model.Entities.FAQ
{
    [Table("Question")]
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        [Index]
        public int ChildId { get; set; }
        [MaxLength(200)]
        public string Content { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        [Index]
        public QuestionType QuestionType { get; set; }
    }

    public enum QuestionType
    {
        AskforLeave = 0,
        NormalMessage = 1
    }
}
