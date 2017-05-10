using System.Globalization;
using BabyBus.Helpers;

namespace BabyBus.Models
{
   public class Question
    {
       public int UserID { get; set; }
       public int QuestionID { get; set; }
       public int UserClassRelationID { get; set; }
       public string Content { get; set; }
       public int CreatTime { get; set; }

       public string DisplayDate
       {
           get
           {
               var date = Utils.ConvertIntDateTime(CreatTime);
               return date.ToString("yyyy-M-d dddd HH:mm", new CultureInfo("zh-CN"));
           }
       }
    }
}
