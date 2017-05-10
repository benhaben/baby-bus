using System;
using System.Collections.Generic;

namespace BabyBus.Logic.Shared
{
	public class MI_TestAssessModel
	{
		public MI_TestAssessModel ()
		{
			MI_Question = new List<MI_QuestionModel>();
		}
		public int MI_TestAssessId{ get; set;}
		public string MI_TestAssessName{ get; set;}
		public int ModalityId { get; set; }
		public List<MI_QuestionModel> MI_Question{ get; set;}


	}
}

