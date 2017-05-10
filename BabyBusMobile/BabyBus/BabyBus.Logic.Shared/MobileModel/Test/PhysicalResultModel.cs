using System;

namespace BabyBus.Logic.Shared
{
	public class PhysicalExaminationResult
	{
		public int Id { get; set; }

		public int ChildId { get; set; }

		public string ChildName { get; set; }

		public int Gender { get; set; }

		public string ClassName { get; set; }

		public int ClassId { get; set; }

		public string KindergartenName { get; set; }

		public int KindergartenId { get; set; }

		public double AgeGroup { get; set; }

		public DateTime Birthday { get; set; }

		public DateTime PlanTime { get; set; }

		public DateTime CreateTime { get; set; }

		public int UserId { get; set; }

		public double Height { get; set; }

		public double Weight { get; set; }

		public double StandingLongJump { get; set; }

		public double JumpWithBothFeet { get; set; }

		public double TennisThrowFar { get; set; }

		public double WalkOnTheBalanceBeam { get; set; }

		public double SitAndReach { get; set; }

		public double TenMetersShuttleRun { get; set; }

		public int HeightScore { get; set; }

		public int WeightScore { get; set; }

		public int StandingLongJumpScore { get; set; }

		public int JumpWithBothFeetScore { get; set; }

		public int TennisThrowFarScore { get; set; }

		public int WalkOnTheBalanceBeamScore { get; set; }

		public int SitAndReachScore { get; set; }

		public int TenMetersShuttleRunScore { get; set; }

		public int TotalScore { get; set; }

		public int Grade { get; set; }

		public int PhysicalExaminationPlanId { get; set; }

		public string GradeResult { get; set; }

		public string WeightResult { get; set; }

		public string HeightResult { get; set; }

		public string SitAndReachResult { get; set; }

		public string PowerResult { get; set; }

		public string SpeedResult { get; set; }

		public string BalanceResult { get; set; }
	}
}

