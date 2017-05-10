using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Models;
using BabyBus.Models.Enums;

namespace BabyBus.ViewModels.Communication
{
    public class CommIndexViewModel : BaseViewModel
    {
        public NoticeIndexViewModel NoticeIndexViewModel { get; private set; }
        public QuestionIndexViewModel QuestionIndexViewModel { get; private set; }

        public CommIndexViewModel() {
            NoticeIndexViewModel = new NoticeIndexViewModel(NoticeViewType.Notice);
            QuestionIndexViewModel = new QuestionIndexViewModel();
        }
    }
}
