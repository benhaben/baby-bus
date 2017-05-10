using System;
using BabyBus.ViewModels.Communication;
using BabyBus.Models.Enums;

namespace ViewModels.Communication {
    public class MemoryIndexViewModel:NoticeIndexViewModel {
        public MemoryIndexViewModel()
            : base(NoticeViewType.GrowMemory) {
        }
    }
}

