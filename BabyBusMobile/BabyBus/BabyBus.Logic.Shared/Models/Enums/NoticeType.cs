using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Models.Enums
{
	public enum NoticeType
	{
		ClassHomework = 1,
		ClassCommon,
		
		//班级紧急通知，暂时用作本地孩子考勤
		ClassEmergency,

		//园区通知
		KindergartenAll,

		//园务通知
		KindergartenStaff,
       
		GrowMemory,
       
		//食谱
		KindergartenRecipe,

		//优贝小报
		BabyBusNotice,

	}
}
