using System;

namespace BabyBus.Logic.Shared
{
    public class TextMessage
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

       
        public string Body { get; set; }

        public DateTime Timestamp { get; set; }


        public int? ImageId { get; set; }

        //add by yin
        bool IsTeacher { get; set; }

        bool IsParent { get; set; }

        byte[] ImageBytes { get; set; }
    }
}
