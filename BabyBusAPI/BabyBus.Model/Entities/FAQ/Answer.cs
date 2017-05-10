using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BabyBus.Model.Entities.Login;
using Newtonsoft.Json;

namespace BabyBus.Model.Entities.FAQ
{
    [Table("Answer")]
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }
        [Index]
        public int UserId { get; set; }
        [Index] 
        public int QuestionId { get; set; }
        [MaxLength(200)]
        public string Content { get; set; }
        public DateTimeOffset CreateTime { get; set; }
    }
}