using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyBus.Net.Attendance;
using BabyBus.Utilities;

namespace BabyBus.ViewModels.Attendance {
    public class ChildAttendanceViewModel : BaseViewModel {

        private readonly IAttendanceService _service;
        public List<DateTime> AttenceDates = new List<DateTime>();

        public int AttenceTotal {
            get { return AttenceDates.Count; }
        }

        public ChildAttendanceViewModel(IAttendanceService service) {
            _service = service;

            //Test Init
            //TestInit();
        }

        public override void Start() {
            base.Start();
            FirstLoad();
        }

        public override void FirstLoad() {
            IsLoading = true;
            ViewModelStatus = new ViewModelStatus("正在加载...", true, MessageType.Information, TipsType.DialogProgress);
            Task.Run(async () => {
                try {
                    var result = await _service.GetChildMonthAttendance(
                                     BabyBusContext.ChildId, DateTime.Now);
                    if (!result.Status) {
                        ViewModelStatus = new ViewModelStatus(result.Message);
                    } else {
                        AttenceDates = result.Items;
                        RaisePropertyChanged(() => AttenceTotal);
                    }
                } catch (Exception ex) {
                    Debug.WriteLine(ex.Message);
                }
                ViewModelStatus = new ViewModelStatus("加载成功", false, MessageType.Success, TipsType.DialogDisappearAuto);
                //Notify UI
                base.FirstLoad();
                
            });

        }

        private void TestInit() {
            AttenceDates.Add(new DateTime(2015, 3, 1));
            AttenceDates.Add(new DateTime(2015, 3, 2));
            AttenceDates.Add(new DateTime(2015, 3, 3));
            AttenceDates.Add(new DateTime(2015, 3, 4)); 
            AttenceDates.Add(new DateTime(2015, 3, 5));

            AttenceDates.Add(new DateTime(2015, 3, 11));
            AttenceDates.Add(new DateTime(2015, 3, 12));
            AttenceDates.Add(new DateTime(2015, 3, 13));
            AttenceDates.Add(new DateTime(2015, 3, 14));
            AttenceDates.Add(new DateTime(2015, 3, 15));

            AttenceDates.Add(new DateTime(2015, 3, 23));
            AttenceDates.Add(new DateTime(2015, 3, 24));

            RaisePropertyChanged(() => AttenceTotal);
        }
    }
}
