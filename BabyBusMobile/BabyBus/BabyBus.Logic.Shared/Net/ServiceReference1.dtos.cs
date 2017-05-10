/* Options:
Date: 2015-08-27 02:49:38
Version: 4.043
BaseUrl: http://115.28.88.175:8099/api

//GlobalNamespace: 
//MakePartial: True
//MakeVirtual: True
//MakeDataContractsExtensible: False
//AddReturnMarker: True
//AddDescriptionAsComments: True
//AddDataContractAttributes: False
//AddIndexesToDataMembers: False
//AddGeneratedCodeAttributes: False
//AddResponseStatus: False
//AddImplicitVersion: 
//InitializeCollections: True
//IncludeTypes: 
//ExcludeTypes: 
//AddDefaultXmlNamespace: http://schemas.servicestack.net/types
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;
using BabybusSSApi.DatabaseModel;
using BabyBusSSApi.ServiceModel.DTO.Reponse;
using BabyBusSSApi.ServiceModel.Enumeration;
using BabyBusSSApi.ServiceModel.DTO.Query;
using BabyBusSSApi.ServiceModel.DTO.Create;
using BabyBusSSApi.ServiceModel.DTO.Update;
using BabyBusSSApi.ServiceModel.DTO.Create.Report;
using BabyBusSSApi.ServiceModel.DTO.Get;
using BabyBusSSApi.ServiceModel.Test;
using BabyBusSSApi.ServiceModel.DTO.Cache;
using BabyBus.Logic.Shared;
using ServiceStack;


namespace BabybusSSApi.DatabaseModel
{

	public partial class AskForLeave
	{
		public virtual int Id { get; set; }

		public virtual int ChildId { get; set; }

		public virtual DateTimeOffset? CreateTime { get; set; }

		public virtual int? AskForLeaveType { get; set; }
	}

	public partial class Child
	{
		public virtual int Id { get; set; }

		public virtual int KindergartenId { get; set; }

        
		public virtual int ClassId { get; set; }

        
		public virtual int CardPasswordId { get; set; }

        
		public virtual string ChildName { get; set; }

        
		public virtual int Gender { get; set; }

        
		public virtual DateTimeOffset Birthday { get; set; }

        
		public virtual DateTimeOffset CreateTime { get; set; }

		public virtual string ImageName { get; set; }

		public virtual bool Cancel { get; set; }
	}

	public partial class Class
	{
		public virtual int Id { get; set; }

		public virtual int KindergartenId { get; set; }

        
		public virtual string ClassName { get; set; }

		public virtual string Description { get; set; }

		public virtual int ClassCount { get; set; }

        
		public virtual bool Cancel { get; set; }
	}

	public partial class Kindergarten
	{
		public virtual int KindergartenId { get; set; }

		public virtual string KindergartenName { get; set; }

		public virtual string Description { get; set; }

		public virtual string Address { get; set; }

		public virtual string City { get; set; }

        
		public virtual int KindergartenCount { get; set; }

        
		public virtual bool Cancel { get; set; }
	}

	public partial class Notice
	{
		public virtual long Id { get; set; }

		public virtual int KindergartenId { get; set; }

        
		public virtual int ClassId { get; set; }

        
		public virtual int UserId { get; set; }

        
		public virtual int NoticeType { get; set; }

        
		public virtual string Title { get; set; }

		public virtual string Content { get; set; }

		public virtual DateTimeOffset CreateTime { get; set; }

		public virtual string ThumPics { get; set; }

		public virtual string NormalPics { get; set; }

        
		public virtual int ReceiverNumber { get; set; }

        
		public virtual int FavoriteCount { get; set; }

        
		public virtual int ReadedCount { get; set; }

        
		public virtual int ConfirmedCount { get; set; }

		public virtual bool IsHtml{ get; set; }
	}

	public partial class ParentChildRelation
	{
		public virtual int Id { get; set; }

		public virtual int ChildId { get; set; }

        
		public virtual int UserId { get; set; }
	}

	public partial class UV_ECPostInfo
	{

		public int PostInfoId { get; set; }

		public string Abstract { get; set; }

		public string Title { get; set; }

		public int? CategoryId { get; set; }

		public string CategoryName { get; set; }

		public int? ColumnType { get; set; }

		public int? CommentCount { get; set; }

		public DateTime? CreateDate { get; set; }

		public decimal? MarketPrice { get; set; }

		public decimal? CurrentPrice { get; set; }

		public string Description { get; set; }

		public string ImageUrls { get; set; }

		public int? InvolvedCount { get; set; }

		public string LessonTime { get; set; }

		public decimal? Rating { get; set; }

		public int? Status { get; set; }

		public int? TotalCount { get; set; }

		public int UserId { get; set; }
	}

	public partial class UV_Answer
	{
        
		public virtual int AnswerId { get; set; }

        
		public virtual int UserId { get; set; }

        
		public virtual int QuestionId { get; set; }

		public virtual string Content { get; set; }

		public virtual DateTimeOffset? CreateTime { get; set; }

		public virtual string LoginName { get; set; }

		public virtual string RealName { get; set; }

		public virtual int RoleType { get; set; }

		public virtual string ImageName { get; set; }
	}

	public partial class UV_AttendanceDetail
	{
		public virtual int? Year { get; set; }

		public virtual int? Month { get; set; }

		public virtual int ChildId { get; set; }

        
		public virtual int MasterId { get; set; }

        
		public virtual int DetailId { get; set; }

		public virtual int? Status { get; set; }

		public virtual string ChildName { get; set; }

		public virtual string ClassName { get; set; }

		public virtual int? KindergartenId { get; set; }

		public virtual string KindergartenName { get; set; }

		public virtual int Total { get; set; }

        
		public virtual int Attence { get; set; }

        
		public virtual DateTimeOffset CreateDate { get; set; }

		public virtual int? ParentId { get; set; }

		public virtual string PhoneNumber { get; set; }

		public virtual string ImageName { get; set; }

		public virtual int? ClassId { get; set; }

		public virtual string RealName { get; set; }
	}

	public partial class UV_AttendanceMaster
	{
        
		public virtual int MasterId { get; set; }

        
		public virtual DateTimeOffset CreateDate { get; set; }

        
		public virtual int Total { get; set; }

        
		public virtual int Attence { get; set; }

		public virtual int? ClassId { get; set; }

		public virtual string ClassName { get; set; }

		public virtual int? KindergartenId { get; set; }

		public virtual string KindergartenName { get; set; }

		public virtual int? ClassCount { get; set; }
	}

	public partial class UV_Child
	{
		public virtual int? KindergartenId { get; set; }

		public virtual string KindergartenName { get; set; }

		public virtual int? ClassId { get; set; }

		public virtual string ClassName { get; set; }

		public virtual int ChildId { get; set; }

        
		public virtual string ChildName { get; set; }

		public virtual int? ParentId { get; set; }

		public virtual string RealName { get; set; }

		public virtual string LoginName { get; set; }

		public virtual string PhoneNumber { get; set; }

		public virtual int Gender { get; set; }

        
		public virtual DateTimeOffset Birthday { get; set; }

		public virtual string City { get; set; }

		public virtual string Address { get; set; }

		public virtual string ImageName { get; set; }
	}

	public partial class UV_Notice
	{
        
		public virtual long NoticeId { get; set; }

        
		public virtual int KindergartenId { get; set; }

        
		public virtual int ClassId { get; set; }

        
		public virtual int UserId { get; set; }

        
		public virtual int NoticeType { get; set; }

        
		public virtual string Title { get; set; }

		public virtual string Content { get; set; }

		public virtual DateTimeOffset CreateTime { get; set; }

		public virtual string ThumPics { get; set; }

		public virtual string NormalPics { get; set; }

        
		public virtual int ReceiverNumber { get; set; }

        
		public virtual int FavoriteCount { get; set; }

        
		public virtual int ReadedCount { get; set; }

        
		public virtual int ConfirmedCount { get; set; }

        
		public virtual string LoginName { get; set; }

		public virtual string RealName { get; set; }

		public virtual string HeadImage { get; set; }
	}

	public partial class UV_Question
	{
        
		public virtual int QuestionId { get; set; }

        
		public virtual int ChildId { get; set; }

		public virtual string Content { get; set; }

		public virtual DateTimeOffset CreateTime { get; set; }

		public virtual int? QuestionType { get; set; }

		public virtual int? SendUserId { get; set; }

		public virtual DateTimeOffset? BeginDate { get; set; }

		public virtual DateTimeOffset? EndDate { get; set; }

		public virtual int? SenderId { get; set; }

		public virtual string SenderDisplayName { get; set; }

		public virtual string SenderImage { get; set; }

		public virtual string ChildName { get; set; }

		public virtual int? KindergartenId { get; set; }

		public virtual int? ClassId { get; set; }

		public virtual bool? HasAnswer { get; set; }
	}



	public partial class UV_Modality
	{

		public int ChildId { get; set; }

		public int ModalityId { get; set; }

		public bool? IsFinished { get; set; }

		public int? TestRoleType { get; set; }

		public int ClassId { get; set; }
	}

	public partial class UV_MiTestMaster
	{

		public int ChildId { get; set; }

		public int ClassId { get; set; }

		public int ModalityId { get; set; }

		public int? TotalTest { get; set; }

		public int? CompletedTest { get; set; }

		public bool? IsFinished { get; set; }

		public int? TestRoleType { get; set; }

		public string ChildName { get; set; }

		public string ImageName { get; set; }

		public int TestMasterId { get; set; }
	}

	public partial class UV_ECReview
	{
		public string Content { get; set; }

		public DateTime? CreateDate { get; set; }

		public int? PostInfoId { get; set; }

		public int? Rating { get; set; }

		public int ReviewId { get; set; }

		public int? UserId { get; set; }

		public string UserName { get; set; }
	}
}

namespace BabyBusSSApi.ServiceModel.DTO.Cache
{


	[Route("/caches/children", "GET")]
	public partial class CacheQueryChildrenByClassId
        : QueryBase<UV_Child, ChildResponse>, IReturn<QueryResponse<ChildResponse>>
	{
		public virtual long ClassId { get; set; }
	}

	[Route("/caches/notices", "GET")]
	public partial class CacheQueryNoticeById
        : QueryBase<Notice, NoticeResponse>, IReturn<QueryResponse<NoticeResponse>>
	{
		public virtual long Id { get; set; }
	}
}

namespace BabyBusSSApi.ServiceModel.DTO.Create
{

	[Route("/answers/", "POST")]
	public partial class CreateAnswer
        : IReturn<long>
	{
		public virtual int QuestionId { get; set; }

		public virtual string Content { get; set; }
	}

	[Route("/attendance", "POST")]
	public partial class CreateAttendance
        : IReturn<long>
	{
		public CreateAttendance()
		{
			ChildrenPresenceList = new List<long>{ };
			ChildrenAbsenceList = new List<long>{ };
		}

		public virtual long MasterId { get; set; }

		public virtual DateTimeOffset CreateDate { get; set; }

		public virtual List<long> ChildrenPresenceList { get; set; }

		public virtual List<long> ChildrenAbsenceList { get; set; }

		public virtual long Total { get; set; }

		public virtual long Attence { get; set; }
	}

	[Route("/children/", "POST")]
	public partial class CreateChild
        : IReturn<Child>
	{
		public CreateChild()
		{
			ParentIds = new List<long>{ };
		}

		public virtual int KindergartenId { get; set; }

		public virtual int ClassId { get; set; }

		public virtual string ChildName { get; set; }

		public virtual GenderType Gender { get; set; }

		public virtual DateTimeOffset Birthday { get; set; }

		public virtual string ImageName { get; set; }

		public virtual List<long> ParentIds { get; set; }
	}

	[Route("/classes", "POST")]
	public partial class CreateClass
        : IReturn<long>
	{
		public virtual int KindergartenId { get; set; }

		public virtual string ClassName { get; set; }

		public virtual string Description { get; set; }
	}

	[Route("/commets", "POST")]
	public partial class CreateComment
        : IReturn<long>
	{
		public virtual int NoticeId { get; set; }

		public virtual int UserId { get; set; }

		public virtual string Content { get; set; }

		public virtual DateTimeOffset CreateTime { get; set; }

		public virtual int? CommentType { get; set; }

		public virtual int? ChildId { get; set; }
	}

	[Route("/feedbacks", "POST")]
	public partial class CreateFeedback
        : IReturnVoid
	{
		public virtual int Id { get; set; }

		public virtual int UserId { get; set; }

		public virtual string Content { get; set; }

		public virtual DateTimeOffset CreatTime { get; set; }

		public virtual string Type { get; set; }

		public virtual string Name { get; set; }

		public virtual int Status { get; set; }
	}

	[Route("/postinfo/review", "POST")]
	public class CreateReview : IReturn<bool>
	{
		public long PostInfoId { get; set; }

		public string Content { get; set; }

		public int Rating { get; set; }
	}

	[Route("/ec/messages/", "POST")]
	public class CreateEcPostInfo : IReturn<long>
	{
		public int Id { get; set; }

		public ColumnType ColumnType { get; set; }

		public int CategoryId { get; set; }

		public string Title { get; set; }

		public string City { get; set; }

		public string Html { get; set; }

		public decimal? MarketPrice { get; set; }

		public decimal? CurrentPrice { get; set; }

		public string Address { get; set; }

		public decimal? Rating { get; set; }

		public int? CommentCount { get; set; }

		public int? InvolvedCount { get; set; }

		public DateTime? BeginDate { get; set; }

		public DateTime? CreateDate { get; set; }

		public string ImageUrls { get; set; }

		public int? Status { get; set; }

		public int? TotalCount { get; set; }

		public string LessonTime { get; set; }

		public string Abstract { get; set; }

		public long UserId { get; set; }
	}

	public class CreateEcComment : IReturn<long>
	{
		public int PostInfoId { get; set; }

		public string Content { get; set; }

		public DateTime? CreateDate { get; set; }

		public int? UserId { get; set; }

		public int? CommentType { get; set; }
	}

	[Route("/kindergartens/", "POST")]
	public partial class CreateKindergarten
        : IReturn<long>
	{
		public virtual string KindergartenName { get; set; }

		public virtual string Description { get; set; }

		public virtual string Address { get; set; }

		public virtual string City { get; set; }
	}

	[Route("/notices", "POST")]
	public partial class CreateNotice
        : IReturn<long>
	{
		public virtual int KindergartenId { get; set; }

		public virtual int ClassId { get; set; }

		public virtual NoticeType NoticeType { get; set; }

		public virtual string Title { get; set; }

		public virtual string Content { get; set; }

		public virtual string NormalPics { get; set; }
	}

	[Route("/parentchildrelation/", "POST")]
	public partial class CreateParentChildRelation
        : IReturn<ParentChildRelation>
	{
		public virtual int ChildId { get; set; }

		public virtual int UserId { get; set; }
	}

	[Route("/questions/", "POST")]
	public partial class CreateQuestion
        : IReturn<List<long>>
	{
		public CreateQuestion()
		{
			Children = new List<long>{ };
		}

		public virtual List<long> Children { get; set; }

		public virtual string Content { get; set; }

		public virtual long ChildId { get; set; }

		public virtual QuestionType QuestionType { get; set; }

		public virtual DateTimeOffset BeginDate { get; set; }

		public virtual DateTimeOffset EndDate { get; set; }
	}

	[Route("/users", "POST")]
	public partial class CreateUser
        : IReturn<UserResponse>
	{
		public CreateUser()
		{
			Roles = new List<string>{ };
			Permissions = new List<string>{ };
		}

		public virtual string UserName { get; set; }

		public virtual string Password { get; set; }

		public virtual string Email { get; set; }

		public virtual string DisplayName { get; set; }

		public virtual string PhoneNumber { get; set; }

		public virtual List<string> Roles { get; set; }

		public virtual List<string> Permissions { get; set; }

		public virtual RoleType RoleType { get; set; }

		public virtual int KindergartenId { get; set; }

		public virtual int ClassId { get; set; }
	}



	[Route("/mitestdedail/", "POST")]
	public class CreateMiTestDetail : IReturn<long>
	{
		public long UserId { get; set; }

		public int TestRoleType { get; set; }

		public long ChildId { get; set; }

		public long ModalityId { get; set; }

		public long TestMasterId{ get; set; }

		public List<MITestQuestion> TestDetail { get; set; }

	}
}

namespace BabyBusSSApi.ServiceModel.DTO.Create.Report
{

	[Route("/reports/page/", "POST")]
	public partial class CreatePageReport
        : IReturnVoid
	{
		public virtual long UserId { get; set; }

		public virtual PageReportType PageReportType { get; set; }

		public virtual DateTimeOffset? CreateDate { get; set; }
	}

	[Route("/reports/pages/", "POST")]
	public partial class CreatePageReports
        : IReturnVoid
	{
		public CreatePageReports()
		{
			Reports = new List<CreatePageReport>{ };
		}

		public virtual List<CreatePageReport> Reports { get; set; }
	}

	[Route("/reports/", "POST")]
	public partial class CreateUserReport
        : IReturnVoid
	{
		public virtual string Mode { get; set; }

		public virtual int? VerCode { get; set; }

		public virtual string VerName { get; set; }

		public virtual DateTimeOffset? LastLogin { get; set; }

		public virtual string OSType { get; set; }
	}
}

namespace BabyBusSSApi.ServiceModel.DTO.Get
{
	[Route("/mimodalities/summary", "GET")]
	public class GetMIModalitySummary : IReturn<List<MIModalitySummaryResponse>>
	{
		public int ClassId { get; set; }
	}

	[Route("/children/attendance", "GET")]
	public partial class GetChildrenAttendance
        : IReturn<List<ChildAttendanceResponse>>
	{
		public virtual long? ClassId { get; set; }

		public virtual DateTimeOffset? Date { get; set; }
	}

	[Route("/cities", "GET")]
	public partial class GetCities
        : IReturn<List<string>>
	{
	}

	[Route("/readers", "GET")]
	public partial class GetReaders
        : IReturn<List<ReadersResponse>>
	{
		public virtual long NoticeId { get; set; }
	}

	[Route("/readers/summary", "GET")]
	public partial class GetReadersSummary
        : IReturn<ReadersSummaryResponse>
	{
		public virtual int NoticeId { get; set; }
	}

	[Route("/users/{userAuthId}", "GET")]
	public partial class GetUserByUserAuthId
        : IReturn<UserResponse>
	{
		public virtual long UserAuthId { get; set; }
	}

	[Route("/users/{userId}", "GET")]
	public partial class GetUserByUserId
        : IReturn<UserResponse>
	{
		public virtual int UserId { get; set; }
	}

	[Route("/version", "GET")]
	public partial class GetVersion
        : IReturn<VersionResponse>
	{
		public virtual int AppType { get; set; }
	}
}

namespace BabyBusSSApi.ServiceModel.DTO.Query
{



	[Route("/ec/categories", "GET")]
	public class QueryEcCategory : QueryBase<ECCategory, EcCategoryResponese>
	{
		public bool Cancel { get; set; }

		public int ColumnType { get; set; }
	}

	[Route("/ecreview/", "GET")]
	public class QueryECReview : QueryBase<UV_ECReview, ECReviewResponse>
	{
		public int PostInfoId { get; set; }

		public long? ReviewIdLessThan { get; set; }
	}


	[Route("/postinfo", "GET")]
	public class QueryPostInfo : QueryBase<UV_ECPostInfo,PostInfoResponse>
	{
		public ECColumnType[] ColumnTypes { get; set; }

	}

	[Route("/mimodalities/", "GET")]
	public class QueryMiModalities : QueryBase<UV_Modality, MiModalityResponese>
	{
		public long ClassId { get; set; }

		public int? TestRoleType { get; set; }

	}

	[Route("/answers/", "GET")]
	public partial class QueryAnswers
        : QueryBase<UV_Answer, AnswerResponse>, IReturn<QueryResponse<AnswerResponse>>
	{
		public virtual int QuestionId { get; set; }
	}

	[Route("/askforleaves", "GET")]
	public partial class QueryAskForLeaves
        : QueryBase<AskForLeave, AskForLeaveReponse>, IReturn<QueryResponse<AskForLeaveReponse>>
	{
		public virtual long ChildId { get; set; }
	}

	[Route("/attendance/details", "GET")]
	public partial class QueryAttendanceDetails
        : QueryBase<UV_AttendanceDetail, AttendanceDetailReponse>, IReturn<QueryResponse<AttendanceDetailReponse>>
	{
		public virtual int? Year { get; set; }

		public virtual int? Month { get; set; }

		public virtual long? ChildId { get; set; }

		public virtual long? ClassId { get; set; }

		public virtual DateTimeOffset? CreateDate { get; set; }

		public virtual AttendanceType? Status { get; set; }
	}

	[Route("/attendance/", "GET")]
	public partial class QueryAttendanceMaster
        : QueryBase<UV_AttendanceMaster, AttendanceMasterResponse>, IReturn<QueryResponse<AttendanceMasterResponse>>
	{
		public virtual long? ClassId { get; set; }

		public virtual long? KindergartenId { get; set; }

		public virtual DateTimeOffset? CreateDate { get; set; }
	}

	[Route("/children", "GET")]
	public partial class QueryChildren
        : QueryBase<UV_Child, ChildResponse>, IReturn<QueryResponse<ChildResponse>>
	{
		public virtual long? ClassId { get; set; }

		public virtual long? ParentId { get; set; }
	}

	[Route("/classes/", "GET")]
	public partial class QueryClasses
        : QueryBase<Class, ClassResponse>, IReturn<QueryResponse<ClassResponse>>
	{
		public virtual long? Id { get; set; }

		public virtual long? KindergartenId { get; set; }
	}

	[Route("/kindergartens", "GET")]
	public partial class QueryKindergartens
        : QueryBase<Kindergarten, KindergartenResponse>, IReturn<QueryResponse<KindergartenResponse>>
	{
		public virtual string City { get; set; }

		public virtual long KindergartenId { get; set; }
	}

	[Route("/notices", "GET")]
	public partial class QueryNotices
        : QueryBase<UV_Notice, NoticeResponse>, IReturn<QueryResponse<NoticeResponse>>
	{
		public QueryNotices()
		{
			NoticeTypes = new NoticeType[]{ };
		}

		public virtual NoticeType[] NoticeTypes { get; set; }

		public virtual long? ClassId { get; set; }

		public virtual long? NoticeId { get; set; }

		public virtual long? NoticeIdGreaterThan { get; set; }

		public virtual long? NoticeIdLessThan { get; set; }

		public virtual long? KindergartenId { get; set; }
	}

	[Route("/parentchildrelations", "GET")]
	public partial class QueryParentChildRelation
        : QueryBase<ParentChildRelation, ParentChildRelationReponse>, IReturn<QueryResponse<ParentChildRelationReponse>>
	{
	}

	[Route("/questions/", "GET")]
	public partial class QueryQuestions
        : QueryBase<UV_Question, QuestionResponse>, IReturn<QueryResponse<QuestionResponse>>
	{
		public QueryQuestions()
		{
			QuestionTypes = new QuestionType[]{ };
		}

		public virtual long? QuestionIdGreaterThan { get; set; }

		public virtual long? QuestionIdLessThan { get; set; }

		public virtual long? QuestionId { get; set; }

		public virtual long? KindergartenId { get; set; }

		public virtual long? ClassId { get; set; }

		public virtual long? ChildId { get; set; }

		public virtual QuestionType[] QuestionTypes { get; set; }
	}
}

namespace BabyBusSSApi.ServiceModel.DTO.Reponse
{

	public class EcCategoryResponese
	{
		public int Id { get; set; }

		public ECColumnType ColumnType { get; set; }

		public string Name { get; set; }

		public int Level { get; set; }
	}

	public class ECReviewResponse
	{
		public string Content { get; set; }

		public DateTime? CreateDate { get; set; }

		public int? PostInfoId { get; set; }

		public int? Rating { get; set; }

		public int ReviewId { get; set; }

		public int? UserId { get; set; }

		public string RealName { get; set; }

		public string ImageName{ get; set; }
	}

	public partial class AnswerResponse
	{
		public virtual int Id { get; set; }

		public virtual int UserId { get; set; }

		public virtual int QuestionId { get; set; }

		public virtual string Content { get; set; }

		public virtual DateTimeOffset CreateTime { get; set; }

		public virtual string RealName { get; set; }

		public virtual string ImageName { get; set; }

		public virtual RoleType RoleType { get; set; }
	}

	public partial class AskForLeaveReponse
	{
		public virtual int Id { get; set; }

		public virtual int ChildId { get; set; }

		public virtual DateTimeOffset? CreateTime { get; set; }

		public virtual int? AskForLeaveType { get; set; }
	}

	public partial class AttendanceDetailReponse
	{
		public int ChildId { get; set; }

		public int MasterId { get; set; }

		public int DetailId { get; set; }

		public int Status { get; set; }

		public string ChildName { get; set; }

		public string ClassName { get; set; }

		public int KindergartenId { get; set; }

		public string KindergartenName { get; set; }

		public int Total { get; set; }

		public int Attence { get; set; }

		public DateTimeOffset CreateDate { get; set; }

		public int UserId { get; set; }

		public string PhoneNumber { get; set; }

		public string ImageName { get; set; }

		public int ClassId { get; set; }

		public string RealName { get; set; }

		public bool IsAskForLeave { get; set; }
	}

	public partial class AttendanceMasterResponse
	{
		public int MasterId { get; set; }

		public DateTimeOffset CreateDate { get; set; }

		public int Total { get; set; }

		public int Attence { get; set; }

		public int ClassId { get; set; }

		public string ClassName { get; set; }

		public int KindergartenId { get; set; }

		public string KindergartenName { get; set; }

		public int ClassCount { get; set; }
	}

	public partial class ChildAttendanceResponse
	{
		public virtual int ChildId { get; set; }

		public virtual string ChildName { get; set; }

		public virtual bool IsSelect { get; set; }

		public virtual bool IsAskForLeave { get; set; }

		public virtual bool IsRead { get; set; }

		public virtual string PhoneNumber { get; set; }
	}

	public partial class ChildResponse
	{
		public virtual int? KindergartenId { get; set; }

		public virtual string KindergartenName { get; set; }

		public virtual int? ClassId { get; set; }

		public virtual string ClassName { get; set; }

		public virtual int ChildId { get; set; }

		public virtual string ChildName { get; set; }

		public virtual int? ParentId { get; set; }

		public virtual string RealName { get; set; }

		public virtual string LoginName { get; set; }

		public virtual string PhoneNumber { get; set; }

		public virtual string ImageName { get; set; }

		public virtual int Gender { get; set; }

		public virtual DateTimeOffset Birthday { get; set; }

		public virtual string City { get; set; }
	}

	public partial class ClassResponse
	{
		public virtual int Id { get; set; }

		public virtual int KindergartenId { get; set; }

		public virtual string ClassName { get; set; }

		public virtual string Description { get; set; }

		public virtual int ClassCount { get; set; }

		public virtual bool Cancel { get; set; }
	}

	public partial class KindergartenResponse
	{
		public virtual int KindergartenId { get; set; }

		public virtual string KindergartenName { get; set; }

		public virtual string Description { get; set; }

		public virtual string Address { get; set; }

		public virtual string City { get; set; }

		public virtual int KindergartenCount { get; set; }

		public virtual bool Cancel { get; set; }
	}

	public class NoticeResponse
	{
		public long NoticeId { get; set; }

		public int KindergartenId { get; set; }

		public int ClassId { get; set; }

		public int UserId { get; set; }

		public int NoticeType { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }

		public DateTime CreateTime { get; set; }

		public string ThumPics { get; set; }

		public string NormalPics { get; set; }

		public int ReceiverNumber { get; set; }

		public int FavoriteCount { get; set; }

		public int ReadedCount { get; set; }

		public int ConfirmedCount { get; set; }

		public string RealName { get; set; }

		public string HeadImage { get; set; }

		public bool IsHtml{ get; set; }

		public string Abstract{ get; set; }
	}

	public partial class ParentChildRelationReponse
	{
		public virtual int Id { get; set; }

		public virtual int ChildId { get; set; }

		public virtual int UserId { get; set; }
	}

	public partial class ParentResponse
	{
		public virtual long ChildId { get; set; }

		public virtual string ParentId { get; set; }

		public virtual string RealName { get; set; }

		public virtual string PhoneNumber { get; set; }
	}

	public partial class QuestionResponse
	{
		public virtual int QuestionId { get; set; }

		public virtual string Content { get; set; }

		public virtual int ChildId { get; set; }

		public virtual DateTimeOffset CreateTime { get; set; }

		public virtual int QuestionType { get; set; }

		public virtual int SendUserId { get; set; }

		public virtual DateTimeOffset? BeginDate { get; set; }

		public virtual DateTimeOffset? EndDate { get; set; }

		public virtual string ChildName { get; set; }

		public virtual int? SenderId { get; set; }

		public virtual string SenderDisplayName { get; set; }

		public virtual string SenderImage { get; set; }

		public virtual bool HasAnswer { get; set; }
	}

	public class AuthenticateResponse : IMeta, IHasSessionId
	{
		public string UserId { get; set; }

		public string SessionId { get; set; }

		public string UserName { get; set; }

		public string DisplayName { get; set; }

		public string ReferrerUrl { get; set; }

		public ResponseStatus ResponseStatus { get; set; }

		public Dictionary<string, string> Meta { get; set; }

		public AuthenticateResponse()
		{
			this.ResponseStatus = new ResponseStatus();
		}
	}

	public partial class ReadersResponse
	{
		public ReadersResponse()
		{
			Parents = new List<ParentResponse>{ };
		}

		public virtual string ImageName { get; set; }

		public virtual long ChildId { get; set; }

		public virtual string ChildName { get; set; }

		public virtual int CommentType { get; set; }

		public virtual bool IsRead { get; set; }

		public virtual List<ParentResponse> Parents { get; set; }
	}

	public partial class ReadersSummaryResponse
	{
		public virtual long TotalCount { get; set; }

		public virtual long ReadedCount { get; set; }
	}

	public partial class UserResponse
	{
		public UserResponse()
		{
			Roles = new List<string>{ };
		}

		public virtual long Id { get; set; }

		public virtual long UserId { get; set; }

		public virtual string UserName { get; set; }

		public virtual string LoginName { get; set; }

		public virtual string RealName { get; set; }

		public virtual string PhoneNumber { get; set; }

		public virtual List<string> Roles { get; set; }

		public virtual int RoleType { get; set; }

		public virtual int KindergartenId { get; set; }

		public virtual int ClassId { get; set; }

		public virtual int ChildId { get; set; }

		public virtual string ImageName { get; set; }

		public virtual bool Cancel { get; set; }
	}

	public partial class VersionResponse
	{
		public virtual int AppType { get; set; }

		public virtual string AppName { get; set; }

		public virtual string ApkName { get; set; }

		public virtual string Description { get; set; }

		public virtual string Link { get; set; }

		public virtual string VerName { get; set; }

		public virtual int VerCode { get; set; }
	}

	public class MiModalityResponese
	{
		public int ModalityId { get; set; }

		public int IsFinished { get; set; }

		public int ChildId { get; set; }

		public long TestRoleType { get; set; }

	}

	public class MiTestMasterResponese
	{
		public int UserId { get; set; }

		public int TestRoleType { get; set; }

		public int ChildId { get; set; }

		public int ModalityId { get; set; }

		public int TestMasterId { get; set; }

		public int TotalTest { get; set; }

		public int CompletedTest { get; set; }

		public bool IsFinished { get; set; }

		public string ChildName { get; set; }

		public string ImageName { get; set; }

	}

	public class MiTestQuestionsResponese
	{

		public int AssessIndexId { get; set; }

		public string AssessName { get; set; }

		public int ModalityId { get; set; }

		public string TestQuestionName { get; set; }

		public int TestQuestionId { get; set; }

		public int Score{ get; set; }

		public int TestDetailId{ get; set; }
	}

	public class MiTestScoreResponese
	{
		public int TestQuestionId { get; set; }

		public int Score { get; set; }

		public int TestDetailId { get; set; }
	}

	public class MIModalitySummaryResponse
	{
		public int ModalityId { get; set; }

		public int Total { get; set; }

		public int Completed { get; set; }
	}

	public class PostInfoResponse
	{
		public string Address { get; set; }

		public int PostInfoId { get; set; }

		public string Abstract { get; set; }

		public string Title { get; set; }

		public int? CategoryId { get; set; }

		public string CategoryName { get; set; }

		public int? ColumnType { get; set; }

		public int? CommentCount { get; set; }

		public DateTime? CreateDate { get; set; }

		public decimal? MarketPrice { get; set; }

		public decimal? CurrentPrice { get; set; }

		public string Description { get; set; }

		public string ImageUrls { get; set; }

		public int? InvolvedCount { get; set; }

		public string LessonTime { get; set; }

		public decimal? Rating { get; set; }

		public int? Status { get; set; }

		public int? TotalCount { get; set; }

		public int UserId { get; set; }

	}

	public class PhysicalExaminationResponese
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

		public DateTimeOffset Birthday { get; set; }

		public DateTimeOffset PlanTime { get; set; }

		public DateTimeOffset CreateTime { get; set; }

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

namespace BabyBusSSApi.ServiceModel.DTO.Update
{

	[Route("/physicalExamination", "PUT")]
	public class UpdatePhysicalExamination : IReturnVoid
	{
		public int Id { get; set; }

		public string ChildName{ get; set; }

		public double? AgeGroup { get; set; }

		public DateTimeOffset? Birthday { get; set; }

		public int? Gender { get; set; }

		public int? UserId { get; set; }

		public double? Height { get; set; }

		public double? Weight { get; set; }

		//十米折返跑
		public double? TenMetersShuttleRun { get; set; }

		//坐位体前屈
		public double? SitAndReach { get; set; }

		//立定跳远
		public double? StandingLongJump { get; set; }

		//双脚连续跳
		public double? JumpWithBothFeet { get; set; }

		//网球掷远
		public double? TennisThrowFar { get; set; }

		public double? WalkOnTheBalanceBeam { get; set; }
	}

	[Route("/attendance", "PUT")]
	public partial class UpdateAttendance
        : IReturn<long>
	{
		public UpdateAttendance()
		{
			ChildrenPresenceList = new List<long>{ };
			ChildrenAbsenceList = new List<long>{ };
		}

		public virtual long Id { get; set; }

		public virtual List<long> ChildrenPresenceList { get; set; }

		public virtual List<long> ChildrenAbsenceList { get; set; }

		public virtual long Total { get; set; }

		public virtual long Attence { get; set; }
	}

	[Route("/children", "PUT")]
	public partial class UpdateChild
        : IReturn<bool>
	{
		public virtual long Id { get; set; }

		public virtual long KindergartenId { get; set; }

		public virtual long ClassId { get; set; }

		public virtual long CardPasswordId { get; set; }

		public virtual string ChildName { get; set; }

		public virtual int Gender { get; set; }

		public virtual DateTimeOffset Birthday { get; set; }

		public virtual DateTimeOffset CreateTime { get; set; }

		public virtual string ImageName { get; set; }

		public virtual bool Cancel { get; set; }
	}

	[Route("/password/{userAuthId}", "PUT")]
	public partial class UpdateOtherPersionPassword
        : IReturn<bool>
	{
		public virtual long UserAuthId { get; set; }

		public virtual string OldPassword { get; set; }

		public virtual string NewPassword { get; set; }
	}

	[Route("/password", "PUT")]
	public partial class UpdatePassword
        : IReturn<bool>
	{
		public virtual string OldPassword { get; set; }

		public virtual string NewPassword { get; set; }
	}

	[Route("/users", "PUT")]
	public partial class UpdateUser
        : IReturn<bool>
	{
		public virtual long Id { get; set; }

		public virtual int RoleType { get; set; }

		public virtual string RealName { get; set; }

		public virtual string Password { get; set; }

		public virtual string LoginName { get; set; }

		public virtual long KindergartenId { get; set; }

		public virtual long ClassId { get; set; }

		public virtual string ImageName { get; set; }

		public virtual bool Cancel { get; set; }

		public virtual DateTimeOffset CreateTime { get; set; }

		public virtual string OpenId { get; set; }
	}
}

namespace BabyBusSSApi.ServiceModel.Enumeration
{

	public enum AttendanceType
	{
		Absence,
		Presence,
	}

	public enum GenderType
	{
		None,
		Male,
		Female,
	}

	public enum NoticeType
	{
		ClassHomework = 1,
		ClassCommon = 2,
		ClassEmergency = 3,
		KindergartenAll = 4,
		KindergartenStaff = 5,
		GrowMemory = 6,
		KindergartenRecipe = 7,
		BabyBusNotice = 8,
		BabyBusNoticeHtml = 9,
	}

	public enum PageReportType
	{
		GrowMomeryIndex,
		GrowMomeryDetail,
		NoticeIndex,
		NoticeDetail,
		Attendance,
	}

	public enum QuestionType
	{
		AskforLeave,
		NormalMessage,
		MasterMessage,
		PersonalMessage,
	}

	public enum RoleType
	{
		Admin,
		Parent,
		Teacher,
		HeadMaster,
	}
}

namespace BabyBusSSApi.ServiceModel.Test
{

	[Route("/testDto/{Name}")]
	public partial class TestDto
        : IReturn<TestResponse>
	{
		public virtual string Name { get; set; }
	}

	public partial class TestResponse
	{
		public virtual string Result { get; set; }
	}
}

