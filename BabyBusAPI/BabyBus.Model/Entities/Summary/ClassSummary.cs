using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Model.Entities.Summary
{
    public class ClassSummary
    {
        [Key]
        public long Id { get; set; }
        [Index]
        [Required]
        public DateTimeOffset Create { get; set; }
        [Index]
        [Required]
        public int ClassId { get; set; }
        public int NoticeCount { get; set; }
        public int GrowMemoryCount { get; set; }
        public int QuestionCount { get; set; }
        public int AnswerCount { get; set; }
    }
}
