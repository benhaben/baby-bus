using System.Threading.Tasks;

using BabyBusSSApi.ServiceModel.Enumeration;
using BabyBusSSApi.ServiceModel.DTO.Update;
using System.Collections.Generic;
using BabyBusSSApi.ServiceModel.DTO.Create;
using BabyBusSSApi.ServiceModel.DTO.Reponse;
using BabyBusSSApi.ServiceModel.DTO.Create.Report;
using System;
using System.Net;
using BabybusSSApi.DatabaseModel;

namespace BabyBus.Logic.Shared
{
	public interface IRemoteService
	{
		string CookiesJsonString {
			get;
		}

		List<Cookie> Cookie{ get; set; }

		Task<List<ChildModel>> GetChildrenAttendance(DateTimeOffset date);

		Task<long> WorkAttendance(CreateAttendance model);

		#region MI

		Task<List<MIModality>> GetTeacherModality();

		Task<List<MITestMaster>> GetModalityChild(int modalityId, int classId);

		Task<long> SendTestQuestions(CreateMiTestDetail model);

		Task<List<MIAssessIndex>> GetTestDetail(RoleType roleType, long _modalityId, long _childId);

		Task<List<MITestMaster>> GetParentModality();

		#endregion

		Task<List<KindergartenModel>>GetKindergartenByCity(string city);

		Task<List<KindergartenClassModel>>GetClassByKgId(long kindergartenId);

		Task<List<string>>GetAllCities();

		Task<bool> RegisterDetial(RegisterDetialModel registerDetial);

		Task<User> Register(User user);

		Task GetIdentifyCode(string phoneNumber, bool check);

		Task<bool> CheckIdentifyCode(string phoneNumber, string code);

		Task<bool> ResetPassword(string phoneNumber, string code, string password);

		Task<string> GenarateNewOrderNumber(int postinfoId, PaymentType type);

		Task<PayOrder> GetPayOrder(string orderNumber);

		Task<AddedValue> GetAddedValue(long kindergartenId);

		Task<bool> GetPaymentStatusByPostInfoId(int postInfoId);

		Task<bool> GetPaymentStatus(PaymentType type, int postInfoId = 0);

		Task<int> GetPaymentFee(PaymentType valueAdded);

		Task<List<AttendanceDetailReponse>> GetChildMonthAttendance(DateTime date);

		Task ChangePassword(UpdatePassword pwd);

		Task SendFeedback(CreateFeedback feedBack);

		Task<int> SendReview(CreateReview review);

		Task<int> SendComment(CreateEcComment commnet);

		Task SendForum(CreateEcPostInfo postinfo);

		Task SendQuestion(CreateQuestion question);

		Task<List<AttendanceMasterModel>> GetAttendanceMasterList(DateTimeOffset date);

		Task SendAnswer(CreateAnswer answer);

		Task<List<AnswerModel>> GetAnswersByQuestionId(int questionId);

		Task<QuestionModel> GetQuestionById(int questionId);

		Task SendNotice(NoticeModel notice);

		Task<List<ChildModel>> GetReadersListByNoticeId(long noticeId);

		/// <summary>
		/// Creates the report. Teacher, Parent, Master
		/// </summary>
		/// <returns>The report.</returns>
		/// <param name="report">Report.</param>
		Task CreateReport(CreateUserReport report);

		Task UploadPageReport(CreatePageReport list);

		/// <summary>
		/// Gets the old question list.
		/// </summary>
		/// <returns>The old question list.</returns>
		/// <param name="minId">Minimum identifier.</param>
		/// <param name="roleType">Role type.</param>
		Task<List<QuestionModel>> GetOldQuestionList(long minId, RoleType roleType);

		/// <summary>
		/// Gets the new question list.
		/// </summary>
		/// <returns>The new question list.</returns>
		/// <param name="minId">Minimum identifier.</param>
		/// <param name = "roleType"></param>
		Task<List<QuestionModel>> GetNewQuestionList(long minId, RoleType roleType);

		#region EC

		Task<List<ECPostInfo>> GetRecommendPosts();

		Task<List<ECPostInfo>> GetNewECPostInfo(ECColumnType[] columnTypes, long maxId, int[] cetagoryIds);

		Task<List<ECPostInfo>> GetOldECPostInfo(ECColumnType[] columnTypes, long minId, int[] cetagoryIds);

		Task<ECPostInfo> GetECPostInfoById(int postInfoId);

		Task<ECPostInfo> GetAudioService();

		Task<List<ECReview>> GetECReview(int postInfoId, int pageSize = Constants.PAGESIZE);


		Task<List<ECComment>> GetECComment(int postInfoId, int pageSize = Constants.PAGESIZE);

		Task<bool> GetIsPraised(int postInfoId);

		Task<List<ECComment>> GetOldECCommentList(int postInfoId, long minId);

		Task<List<ECReview>> GetOldECReviewList(int postInfoId, long minId);

		Task<List<ECCategory>> GetECCategoryList(ECColumnType columnType);

		//		Task<List<ECPostComment>> GetNewPostCommentList();
		//
		//		Task<List<ECPostComment>> GetOldPostCommentList(long minId);

		Task<List<ECPostInfo>> GetNewPostCommentList();

		Task<List<ECPostInfo>> GetOldPostCommentList(long minId);

		Task<List<ECPayOrder>> GetNewECPayOrdertList();

		Task<List<ECPayOrder>> GetOldECPayOrderList(long minId);

		Task<List<AdvertisementModel>> GetAdvertisement();

		#endregion

		Task CreateComment(CreateComment comment);

		/// <summary>
		/// Gets the notice readers by notice identifier.
		/// </summary>
		/// <returns>The notice readers by notice identifier.</returns>
		/// <param name="_noticeId">Notice identifier.</param>
		//        Task<List<NoticeReadModeleadModel>>  GetNoti/// ceReadersByNoticeId(int _noticeId);

		/// <summary>
		/// Gets the notice readers satistic by notice identifier.
		/// </summary>
		/// <returns>The notice readers summary by notice identifier.</returns>
		/// <param name="_noticeId">Notice identifier.</param>
		Task<ReadersSummaryResponse>  GetNoticeReadersSummaryByNoticeId(int _noticeId);


		/// <summary>
		/// Gets the notice by identifier.
		/// </summary>
		/// <returns>The notice by identifier.</returns>
		/// <param name="noticeId">Notice identifier.</param>
		Task<NoticeModel> GetNoticeById(int noticeId);

		/// <summary>
		/// Gets the old notice list. Get notice by id which smaller than minId
		/// </summary>
		/// <returns>The old notice list.</returns>
		/// <param name = "noticeTypes"></param>
		/// <param name="minId">Minimum identifier.</param>
		Task<List<NoticeModel>> GetOldNoticeList(NoticeViewType viewtype, long minId);

		/// <summary>
		/// Gets the new notice list. Get notice by id which bigger than maxId 
		/// </summary>
		/// <returns>The new notice list.</returns>
		Task<List<NoticeModel>> GetNewNoticeList(NoticeViewType viewtype, long maxId);

		/// <summary>
		/// Gets the latest memory. Parent
		/// </summary>
		/// <returns>The latest memory.</returns>
		Task<NoticeModel> GetLatestMemory();

		/// <summary>
		/// Gets the latest class notice. Parent
		/// </summary>
		/// <returns>The latest class notice.</returns>
		Task<NoticeModel> GetLatestClassNotice();

		/// <summary>
		/// Gets the latest kindergartens notice. Parent
		/// </summary>
		/// <returns>The latest kindergartens notice.</returns>
		Task<NoticeModel> GetLatestKindergartensNotice();

		/// <summary>
		/// Gets the name of the version by. Android
		/// </summary>
		/// <returns>The version by name.</returns>
		/// <param name="appType">App type.</param>
		Task<VersionModel> GetVersionByAppType(int appType);

		/// <summary>
		/// Login the specified name, password, roleType and rememberMe. Teacher, Parent, Master
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="password">Password.</param>
		/// <param name="roleType">Role type.</param>
		/// <param name="rememberMe">If set to <c>true</c> remember me.</param>
		Task<AuthenticateResponse> Login(string name, string password, RoleType roleType, bool rememberMe = true);

		Task<AuthenticateResponse> LoginWithWechat(string code);

		/// <summary>
		/// Logout this instance. Teacher, Parent, Master
		/// </summary>
		Task<bool> Logout();

		/// <summary>
		/// Gets the kindergarten. Teacher, Parent, Master
		/// </summary>
		/// <returns>The kindergarten.</returns>
		Task<KindergartenModel> GetKindergarten();

		/// <summary>
		/// Gets the child by parent identifier. Parent
		/// </summary>
		/// <returns>The child by parent identifier.</returns>
		/// <param name="id">Identifier.</param>
		Task<ChildModel> GetChildByParentId(long id);

		/// <summary>
		/// Gets the children. Teacher
		/// </summary>
		/// <returns>The children.</returns>
		Task<List<ChildModel>> GetChildren();

		/// <summary>
		/// Gets the class. Teacher, Parent
		/// </summary>
		/// <returns>The class.</returns>
		Task<KindergartenClassModel> GetClass(long classId);

		/// <summary>
		/// Updates the child. Teacher, Parent, Master
		/// </summary>
		/// <returns>The child.</returns>
		/// <param name="child">Child.</param>
		Task<bool> UpdateChild(ChildModel child);

		/// <summary>
		/// Updates the user. Teacher, Parent, Master
		/// </summary>
		/// <returns>The user.</returns>
		/// <param name="updateUser">Update user.</param>
		Task<bool> UpdateUser(UserModel updateUser);

		#region Test

		Task<PhysicalExaminationResult> GetPhysicalExaminationResult(long childId);

		Task<int> UpdatePhysicalExaminationResult(UpdatePhysicalExamination model);



		#endregion

		#region Albums

		Task<List<AlbumModel>> GetAlbums();

		#endregion

	}
}
