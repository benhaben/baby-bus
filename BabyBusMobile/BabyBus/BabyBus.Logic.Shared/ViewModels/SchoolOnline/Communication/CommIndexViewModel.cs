using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace BabyBus.Logic.Shared
{
    public class CommIndexViewModel : BaseViewModel
    {
        public NoticeIndexViewModel NoticeIndexViewModel { get; private set; }

        public QuestionIndexViewModel QuestionIndexViewModel { get; private set; }

        public CommIndexViewModel()
        {
            NoticeIndexViewModel = new NoticeIndexViewModel(NoticeViewType.Notice);
            QuestionIndexViewModel = new QuestionIndexViewModel();
        }
    }
}
