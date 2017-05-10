using BabyBusSSApi.ServiceModel.DTO.Reponse;
using AutoMapper;
using System;
using BabyBusSSApi.ServiceModel.DTO.Update;

namespace BabyBus.Logic.Shared
{
	public static class ConvertExtensions
	{
		public class DateTimeToDateTimeOffsetConverter : ITypeConverter<DateTime, DateTimeOffset>
		{
			#region ITypeConverter implementation

			public DateTimeOffset Convert(ResolutionContext context)
			{
				try {
					var objDateTime = (DateTime)context.SourceValue;
					return new DateTimeOffset(objDateTime);
				} catch (Exception e) {
				}
				return DateTimeOffset.Now;
			}

			#endregion
		}

		public class DateTimeOffsetToDateTimeConverter : ITypeConverter<DateTimeOffset, DateTime>
		{
			#region ITypeConverter implementation

			public DateTime Convert(ResolutionContext context)
			{
				try {
					var objDateTimeOffset = (DateTimeOffset)context.SourceValue;
					return objDateTimeOffset.DateTime;
				} catch (Exception e) {
				}
				return DateTime.Now;
			}

			#endregion
		}

		public static void Initiate()
		{
			Mapper.Configuration.ShouldMapField = arg => false;
			Mapper.Configuration.AllowNullDestinationValues = true;
			Mapper.Configuration.AllowNullCollections = true;
			Mapper.Initialize(cfg => {
				cfg.CreateMap<QuestionResponse, QuestionModel>();
				cfg.CreateMap<NoticeResponse, NoticeModel>();
				cfg.CreateMap<ClassResponse, KindergartenClassModel>();
				cfg.CreateMap<UserResponse, UserModel>();
				cfg.CreateMap<AnswerResponse, AnswerModel>();
				cfg.CreateMap<ReadersResponse, ChildModel>();
				cfg.CreateMap<ChildResponse, ChildModel>();
				cfg.CreateMap<ChildAttendanceResponse, ChildModel>();
				cfg.CreateMap<KindergartenResponse, KindergartenModel>();
				cfg.CreateMap<AttendanceMasterResponse, AttendanceMasterModel>();
				cfg.CreateMap<VersionResponse, VersionModel>();

				cfg.CreateMap<UserModel, UpdateUser>();
				cfg.CreateMap<MiTestMasterResponese, MITestMaster>();
				cfg.CreateMap<MIModalitySummaryResponse, MIModality>();
				cfg.CreateMap<ECReviewResponse,ECReview>();
				cfg.CreateMap<EcCategoryResponese,ECCategory>();
			});

			Mapper.CreateMap<DateTime, DateTimeOffset>().ConvertUsing<DateTimeToDateTimeOffsetConverter>();
			Mapper.CreateMap<DateTimeOffset, DateTime>().ConvertUsing<DateTimeOffsetToDateTimeConverter>();
		}

		public static VersionModel ToVersionModel(this VersionResponse from)
		{
			VersionModel to = new VersionModel();
			to = Mapper.Map<VersionResponse, VersionModel>(from, to); 
			return to;
		}

       
		public static AttendanceMasterModel ToAttendanceMasterModel(this AttendanceMasterResponse from)
		{
			AttendanceMasterModel to = new AttendanceMasterModel();
			to = Mapper.Map<AttendanceMasterResponse, AttendanceMasterModel>(from, to); 
			return to;
		}

		public static ChildModel ToChildModel(this ChildResponse from)
		{
			ChildModel to = new ChildModel();
			to = Mapper.Map<ChildResponse, ChildModel>(from, to); 
			return to;
		}

		public static KindergartenModel ToKindergartenModel(this KindergartenResponse from)
		{
			//Note: Mapper throw exception somtimes, this bitch is not as good as we expected, use this method to woraround
			KindergartenModel to = new KindergartenModel();
			to = Mapper.Map<KindergartenResponse, KindergartenModel>(from, to); 
			return to;
		}

		public static QuestionModel ToQuestionModel(this QuestionResponse from)
		{
			QuestionModel to = new QuestionModel();
			to = Mapper.Map<QuestionResponse, QuestionModel>(from, to); 
			to.SendUserName = from.SenderDisplayName;
			return to;
		}

		public static ECPostInfo ToECPostInfoModel(this PostInfoResponse from)
		{
			ECPostInfo to = new ECPostInfo();
			to = Mapper.Map<PostInfoResponse, ECPostInfo>(from, to); 
			return to;
		}

		public static NoticeModel ToNoticeModel(this NoticeResponse from)
		{
			NoticeModel to = new NoticeModel();
			to = Mapper.Map<NoticeResponse, NoticeModel>(from, to); 
			return to;
		}

		public static KindergartenClassModel ToKindergartenClassModel(this ClassResponse from)
		{
			KindergartenClassModel to = new KindergartenClassModel();
			to = Mapper.Map<ClassResponse,KindergartenClassModel>(from, to); 
			to.ClassId = from.Id;
			return to;
		}

		public static UserModel ToUserModel(this UserResponse from)
		{
			UserModel to = new UserModel();
			to = Mapper.Map<UserResponse, UserModel>(from, to); 
			to.LoginName = from.UserName;
			return to;
		}

		public static AnswerModel ToAnswerModel(this AnswerResponse from)
		{
			AnswerModel to = new AnswerModel();
			to = Mapper.Map<AnswerResponse,AnswerModel>(from, to); 
			to.CreateTime = from.CreateTime.DateTime;
			return to;
		}

		public static ChildModel ToChildModel(this ReadersResponse from)
		{
			ChildModel to = new ChildModel();

			to = Mapper.Map<ReadersResponse, ChildModel>(from, to);
			if (from.Parents.Count > 0) {
				to.ParentName = from.Parents[0].RealName;
				to.PhoneNumber = from.Parents[0].PhoneNumber;
			}
			return to;
		}


		public static ChildModel ToChildModel(this ChildAttendanceResponse from)
		{
			ChildModel to = new ChildModel();
			to = Mapper.Map<ChildAttendanceResponse, ChildModel>(from, to);
			return to;
		}

		public static MIModality ToMIModality(this MIModalitySummaryResponse from)
		{
			var to = new MIModality();
			to = Mapper.Map<MIModalitySummaryResponse, MIModality>(from, to);
			return to;
		}

		public static MITestMaster ToMITestMaster(this MiTestMasterResponese from)
		{
			var to = new MITestMaster();
			to = Mapper.Map<MiTestMasterResponese, MITestMaster>(from, to);
			return to;
		}

		public static ECReview ToECReviewModel(this ECReviewResponse from)
		{
			var to = new ECReview();
			to = Mapper.Map<ECReviewResponse,ECReview>(from, to);
			return to;
		}

		public static ECCategory ToECCategoryModel(this EcCategoryResponese from)
		{
			var to = new ECCategory();
			to = Mapper.Map<EcCategoryResponese,ECCategory>(from, to);
			return to;
		}
	}
}

