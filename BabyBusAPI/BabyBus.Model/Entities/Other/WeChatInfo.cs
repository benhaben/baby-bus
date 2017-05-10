using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyBus.Model.Entities.Other
{
    [Table("WeChatInfo")]
    public class WeChatInfo
    {
        [Key]
        public int WeChatInfoId { get; set; }
        [Index(IsUnique = true)]
        [StringLength(35)] 
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public string Sex { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string Headimgurl { get; set; }
        public string Language { get; set; }
        public string SubscribeTime { get; set; }
    }
}
