using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BabyBus.Models.ChildEx;

namespace BabyBus.Services.ChildEx
{
    public class ChildExService : IChildExService
    {
        private static readonly List<Models.ChildEx.ChildEx> _list = new List<Models.ChildEx.ChildEx>
        {
            new Models.ChildEx.ChildEx
            {
                Id = 1,
                Title = "熊孩子",
				SlideDisplay = "* _ _",
                Content = "熊孩子太他妈讨厌了",
                InDate = new DateTime(2014, 7, 15),
                ChildExType = ChildExType.幼教经验
            },
            new Models.ChildEx.ChildEx
            {
                Id = 2,
				SlideDisplay = "* _ _",
                Title = "C# 遍历枚举类型的所有元素",
                Content = "C# 遍历枚举类型的所有元素",
                InDate = new DateTime(2014, 7, 14),
                ChildExType = ChildExType.幼教经验
            },
            new Models.ChildEx.ChildEx
            {
                Id = 3,
                Title = "羞羞的事情",
				SlideDisplay = "_ * _",
                Content = "熊孩子太他妈讨厌了",
                InDate = new DateTime(2014, 7, 14),
                ChildExType = ChildExType.健康经验
            },
            new Models.ChildEx.ChildEx
            {
                Id = 4,
                Title = "我擦",
				SlideDisplay = "_ _ *",
                Content = "熊孩子太他妈讨厌了",
                InDate = new DateTime(2014, 7, 13),
                ChildExType = ChildExType.幼教服务
            }
        };

        public async Task<List<Models.ChildEx.ChildEx>> SearchChildEx()
        {
			await Task.Delay (1);
            return _list;
        }

        public async Task<Models.ChildEx.ChildEx> GetById(int id)
        {
			await Task.Delay (1);
            Models.ChildEx.ChildEx item = _list.FirstOrDefault(g => g.Id == id);
            return item;
        }
    }
}