using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using BabyBusSSApi.ServiceModel.DTO.Create;
using BabyBusSSApi.ServiceModel.DTO.Create.Report;
using BabyBusSSApi.ServiceModel.DTO.Reponse;
using BabyBusSSApi.ServiceModel.DTO.Update;
using BabyBusSSApi.ServiceModel.Enumeration;
using RestSharp;
using Newtonsoft.Json;
using ServiceStack;
using AutoMapper;
using System.Linq;

namespace BabyBus.Logic.Shared
{
	public class RemoteService : IRemoteService
	{
		//        public IServiceClient iServiceClient{ get; set; }

		readonly RestClient _iServiceClient;
		List<Cookie> _cookies = new List<Cookie>();

		public string CookiesJsonString {
			get {
				return JsonConvert.SerializeObject(_cookies);
			}
		}

		public List<Cookie> Cookie {
			get {
				return _cookies;
			}
			set {
				_cookies = value;
				if (_iServiceClient.CookieContainer == null) {
					_iServiceClient.CookieContainer = new CookieContainer(_cookies.Count);
				}
				foreach (var sessionCookie in _cookies) {
					_iServiceClient.CookieContainer.Add(new Cookie(sessionCookie.Name, sessionCookie.Value, sessionCookie.Path, sessionCookie.Domain));
				}
			}
		}

		public RemoteService()
		{
			_iServiceClient = new RestClient(Constants.BaseApiUrl);
		}

      
      

		#region MI

       

		public async Task<List<MIModality>> GetTeacherModality()
		{
			var request = new RestRequest("/mimodalities/summary", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("ClassId", BabyBusContext.ClassId);
			var response = await _iServiceClient.ExecuteTaskAsync<List<MIModalitySummaryResponse>>(request);
			TryThrowResponseException(response);

			return response.Data.ConvertAll(x => {
				return x.ToMIModality();
			});
		}

		public async Task<List<MITestMaster>> GetModalityChild(int modalityId, int classId)
		{
			var request = new RestRequest("/mitestmaster/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("ClassId", classId);
			request.AddParameter("TestRoleType", (int)BabyBusContext.RoleType);


			var response = await _iServiceClient.ExecuteTaskAsync
				<QueryResponse<MiTestMasterResponese>>(request);
			TryThrowResponseException(response);
			return response.Data.Results.ConvertAll(x => {
				return x.ToMITestMaster();
			});
		}

		public async Task<long> SendTestQuestions(CreateMiTestDetail model)
		{

			var request = new RestRequest("/mitestdedail", Method.POST);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			request.AddBody(model);

			var response = await _iServiceClient.ExecuteTaskAsync<long>(request);
			TryThrowResponseException(response);
			return response.Data;
		}

		public async Task<List<MIAssessIndex>> GetTestDetail(RoleType roleType, long modalityId, long childId)
		{
			var miAIList = new List<MIAssessIndex>();

			//1. Get Test Question
			var request = new RestRequest("/mitestquestions/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("ModalityId", modalityId);
			var mitestquestionResponse = await _iServiceClient.ExecuteTaskAsync
				<QueryResponse<MiTestQuestionsResponese>>(request);
			TryThrowResponseException(mitestquestionResponse);

			//2. Get Test Score
			request = new RestRequest("/mitestcore/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("ModalityId", modalityId);
			request.AddParameter("ChildId", childId);
			request.AddParameter("TestRoleType", (int)BabyBusContext.RoleType);
			var mitestscoreResponse = await _iServiceClient.ExecuteTaskAsync
				<QueryResponse<MiTestScoreResponese>>(request);
			TryThrowResponseException(mitestscoreResponse);
			foreach (var question in mitestquestionResponse.Data.Results) {
				var score = mitestscoreResponse.Data.Results.Find(x => x.TestQuestionId == question.TestQuestionId);
				if (score != null) {
					question.TestDetailId = score.TestDetailId;
					question.Score = score.Score;
				}
			}

			//3. Data Mapping
			foreach (var question in mitestquestionResponse.Data.Results) {
				var curitem = miAIList.Find(x => x.Name == question.AssessName);
				if (curitem != null) {
					curitem.MITestList.Add(new MITestQuestion {
						TestQuestionId = question.TestQuestionId,
						Name = question.TestQuestionName,
						Score = question.Score,
						Id = question.TestDetailId
					});
				} else {
					var item = new MIAssessIndex();
					item.Name = question.AssessName;
					item.ModalityId = question.ModalityId;
					item.MITestList = new List<MITestQuestion>();
					item.MITestList.Add(new MITestQuestion {
						TestQuestionId = question.TestQuestionId,
						Name = question.TestQuestionName,
						Score = question.Score,
						Id = question.TestDetailId
					});
					miAIList.Add(item);
				}
			}

			return miAIList;
		}

		public async Task<List<MITestMaster>> GetParentModality()
		{
			var request = new RestRequest("/mitestmaster/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("ChildId", BabyBusContext.ChildId);
			request.AddParameter("TestRoleType", (int)BabyBusContext.RoleType);
			request.AddParameter("OrderByDesc", "IsMember", ParameterType.QueryString);

			var response = await _iServiceClient.ExecuteTaskAsync
				<QueryResponse<MiTestMasterResponese>>(request);
			TryThrowResponseException(response);
			return response.Data.Results.ConvertAll(x => {
				return x.ToMITestMaster();
			});
		}

		#endregion

		public async Task GetIdentifyCode(string phoneNumber, bool checkExist)
		{
			var request = new RestRequest("/IdentifyingCode", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("PhoneNumber", phoneNumber);
			request.AddParameter("CheckExist", checkExist);

			var response = await _iServiceClient.ExecuteTaskAsync(request);
			TryThrowResponseException(response);
		}

		public async Task<bool> CheckIdentifyCode(string phoneNumber, string code)
		{
			var request = new RestRequest("/IdentifyingCode/check", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("PhoneNumber", phoneNumber);
			request.AddParameter("Code", code);

			var response = await _iServiceClient.ExecuteTaskAsync<bool>(request);
			TryThrowResponseException(response);
			return response.Data;
		}

		public async  Task<bool> ResetPassword(string phoneNumber, string code, string password)
		{
			var request = new RestRequest("/password/reset", Method.PUT);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			request.AddBody(new {
                PhoneNumber = phoneNumber,
                Code = code,
                NewPassword = password
            });     
			var reponse = await _iServiceClient.ExecuteTaskAsync<bool>(request);
			TryThrowResponseException(reponse);
			return reponse.Data;
		}


		public async Task<List<KindergartenModel>>GetKindergartenByCity(string city)
		{
			var request = new RestRequest("/registers/kindergartens", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("City", city);

			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<KindergartenModel>>(request);
			TryThrowResponseException(response);
			return response.Data.Results;
		}

		public async Task<List<KindergartenClassModel>>GetClassByKgId(long kindergartenId)
		{
			var request = new RestRequest("/registers/classes/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("KindergartenId", kindergartenId);

			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<KindergartenClassModel>>(request);
			TryThrowResponseException(response);
			return response.Data.Results;
		}

		public async Task<List<string>>GetAllCities()
		{
			var request = new RestRequest("/cities/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			var response = await _iServiceClient.ExecuteTaskAsync<List<string>>(request);
			TryThrowResponseException(response);
			return response.Data;
		}

		public async Task<bool>RegisterDetial(RegisterDetialModel registerDetial)
		{
			var request = new RestRequest("/registerdetial/", Method.POST);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			var response = await _iServiceClient.ExecuteTaskAsync<bool>(request);
			TryThrowResponseException(response);
			return response.Data;
		}

		public async Task<User>Register(User user)
		{
			var request = new RestRequest("/users", Method.POST);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("PhoneNumber", user.PhoneNumber);
			request.AddParameter("LoginName", user.LoginName);
			request.AddParameter("Password", user.Password);
			request.AddParameter("RoleType", user.RoleType);
			var response = await _iServiceClient.ExecuteTaskAsync<User>(request);
			TryThrowResponseException(response);
			return response.Data;
		}

		#region Payment

		public async Task<string> GenarateNewOrderNumber(int postinfoId, PaymentType type)
		{
			var request = new RestRequest("/payorders/", Method.POST);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("PostInfoId", postinfoId);
			request.AddParameter("UserId", BabyBusContext.UserId);
			request.AddParameter("PaymentType", type);
			var response = await _iServiceClient.ExecuteTaskAsync<string>(request);
			TryThrowResponseException(response);
			return response.Data;
		}

		public async Task<bool> GetPaymentStatusByPostInfoId(int postInfoId)
		{
			var request = new RestRequest("/ec/paystatusbyid", Method.GET);
			request.AddParameter("UserId", BabyBusContext.UserId);
			request.AddParameter("PostInfoId", postInfoId);
			var response = await _iServiceClient.ExecuteTaskAsync<bool>(request);
			TryThrowResponseException(response);
			return response.Data;
		}

		public async Task<bool> GetPaymentStatus(PaymentType type, int postInfoId = 0)
		{
			var request = new RestRequest("/ec/paystatus", Method.GET);
			request.AddParameter("UserId", BabyBusContext.UserId);
			request.AddParameter("PostInfoId", postInfoId);
			request.AddParameter("PaymentType", type);
			var response = await _iServiceClient.ExecuteTaskAsync<bool>(request);
			TryThrowResponseException(response);
			return response.Data;
		}

		public async Task<int> GetPaymentFee(PaymentType type)
		{

			var request = new RestRequest("/paymentfee/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("Type", (int)type, ParameterType.QueryString);
			var response = await _iServiceClient.ExecuteTaskAsync<int>(request);
			TryThrowResponseException(response);
			return response.Data;
		}

		public async Task<PayOrder> GetPayOrder(string orderNo)
		{
			var request = new RestRequest("/ec/payorderByOrderNumber", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("OrderNumber", orderNo);
			var reponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<PayOrder>>(request);
			TryThrowResponseException(reponse);
			if (reponse.Data != null && reponse.Data.Results.Count > 0) {
				return reponse.Data.Results[0];	
			} else {
				return new PayOrder();
			}

		}

		#endregion

		public async Task<long> WorkAttendance(CreateAttendance createAttendance)
		{
			var request = new RestRequest("/attendance", Method.POST);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			request.AddBody(createAttendance);     
			var reponse = await _iServiceClient.ExecuteTaskAsync<long>(request);
			TryThrowResponseException(reponse);
			return reponse.Data;
		}

		public async Task<List<AttendanceDetailReponse>> GetChildMonthAttendance(DateTime date)
		{
			var request = new RestRequest("/attendance/details", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("Year", date.Year, ParameterType.QueryString);
			request.AddParameter("Month", date.Month, ParameterType.QueryString);

			var attendanceResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<AttendanceDetailReponse>>(request);
			TryThrowResponseException(attendanceResponse);
			return attendanceResponse.Data.Results;
		}

		public async Task ChangePassword(UpdatePassword pwd)
		{
			var request = new RestRequest("password", Method.PUT);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			request.AddBody(pwd);     
			var reponse = await _iServiceClient.ExecuteTaskAsync(request);
			TryThrowResponseException(reponse);

		}

		public async Task SendFeedback(CreateFeedback feedBack)
		{
			var request = new RestRequest("/feedbacks", Method.POST);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			request.AddBody(feedBack);     
			var reponse = await _iServiceClient.ExecuteTaskAsync(request);
			TryThrowResponseException(reponse);

		}

		public async Task<int> SendReview(CreateReview review)
		{
			var request = new RestRequest("/ec/review", Method.POST);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			request.AddBody(review);     
			var response = await _iServiceClient.ExecuteTaskAsync<int>(request);
			TryThrowResponseException(response);
			return response.Data;
		}

		public async Task<int> SendComment(CreateEcComment commnet)
		{
			
			var request = new RestRequest("/ec/comments", Method.POST);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			request.AddBody(commnet);     
			var response = await _iServiceClient.ExecuteTaskAsync<int>(request);
			TryThrowResponseException(response);
			return response.Data;
		}

		public async Task SendForum(CreateEcPostInfo postinfo)
		{

			var request = new RestRequest("/ec/messages/", Method.POST);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			request.AddBody(postinfo);     
			var reponse = await _iServiceClient.ExecuteTaskAsync(request);
			TryThrowResponseException(reponse);
		}

		public async Task SendQuestion(CreateQuestion createQuestion)
		{
			var request = new RestRequest("questions", Method.POST);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			request.AddBody(createQuestion);     
			var reponse = await _iServiceClient.ExecuteTaskAsync(request);
			TryThrowResponseException(reponse);

		}

		public async Task<List<AttendanceMasterModel>> GetAttendanceMasterList(DateTimeOffset date)
		{
			var request = new RestRequest("/attendance/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("CreateDate", date);
			request.AddParameter("ClassId", BabyBusContext.UserAllInfo.ClassId);
			request.AddParameter("KindergartenId", BabyBusContext.UserAllInfo.KindergartenId);
			var attendanceResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<AttendanceMasterModel>>(request);                                      
			TryThrowResponseException(attendanceResponse);
//			var ret = attendanceResponse.Data.Results.ConvertAll(x => {
//				return x.ToAttendanceMasterModel(); 
//			});
//			return ret;
			return attendanceResponse.Data.Results;
        
		}

		public async Task SendAnswer(CreateAnswer answer)
		{
			var request = new RestRequest("answers", Method.POST);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			request.AddBody(answer);     
			var reponse = await _iServiceClient.ExecuteTaskAsync(request);
			TryThrowResponseException(reponse);

		}

		public async Task<List<AnswerModel>> GetAnswersByQuestionId(int questionId)
		{
			var request = new RestRequest("answers", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("QuestionId", questionId, ParameterType.QueryString);
			request.AddParameter("OrderBy", "AnswerId", ParameterType.QueryString);
			var answersResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<AnswerResponse>>(request);
			TryThrowResponseException(answersResponse);

			var answers = answersResponse.Data.Results.ConvertAll(x => {
				return  x.ToAnswerModel();
			});
			return answers;
		}

		public async Task<QuestionModel> GetQuestionById(int questionId)
		{
			var request = new RestRequest("questions", Method.GET);
			request.AddParameter("QuestionId", questionId, ParameterType.QueryString);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			var questionResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<QuestionResponse>>(request);
			TryThrowResponseException(questionResponse);
        
			if (questionResponse.Data.Results.Count == 1) {
				return questionResponse.Data.Results[0].ToQuestionModel();
			} else {
				throw new UnexpectedResponseDataException();
			}
		}

		public async Task SendNotice(NoticeModel notice)
		{
			var createNotice = Mapper.DynamicMap<CreateNotice>(notice);
			var request = new RestRequest("notices", Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddBody(createNotice);     
			var reponse = await _iServiceClient.ExecuteTaskAsync(request);
			TryThrowResponseException(reponse);

		}

		public async Task<List<ChildModel>> GetReadersListByNoticeId(long noticeId)
		{
			var request = new RestRequest("/readers", Method.GET);
			request.AddParameter("NoticeId", noticeId, ParameterType.QueryString);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			var readersSummaryResponse = await _iServiceClient.ExecuteTaskAsync<List<ReadersResponse>>(request);
			TryThrowResponseException(readersSummaryResponse);

			var children = readersSummaryResponse.Data.ConvertAll(x => {
				return  x.ToChildModel();
			});
			return children;
		}

		public async Task CreateComment(CreateComment comment)
		{
			var request = new RestRequest("commets", Method.POST);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddBody(comment);     
			var reponse = await _iServiceClient.ExecuteTaskAsync(request);
			TryThrowResponseException(reponse);

		}

		public async Task<ReadersSummaryResponse> GetNoticeReadersSummaryByNoticeId(int _noticeId)
		{
			var request = new RestRequest("/readers/summary", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("NoticeId", _noticeId, ParameterType.QueryString);
			var readersSummaryResponse = await _iServiceClient.ExecuteTaskAsync<ReadersSummaryResponse>(request);
			TryThrowResponseException(readersSummaryResponse);

			return readersSummaryResponse.Data;
		}

		public async Task UploadPageReport(CreatePageReport createPageReport)
		{
			var request = new RestRequest("reports/page/", Method.POST);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddBody(createPageReport);     
			var reponse = await _iServiceClient.ExecuteTaskAsync(request);
			TryThrowResponseException(reponse);

		}

		public async Task CreateReport(CreateUserReport report)
		{
			var request = new RestRequest("reports", Method.POST);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddBody(report);     
			var reponse = await _iServiceClient.ExecuteTaskAsync(request);
			//TryThrowResponseException(reponse);

		}

		#region EC

		public async Task<List<AdvertisementModel>> GetAdvertisement()
		{
			var request = new RestRequest("/ec/advertisement", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("IsUsed", true);
			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<AdvertisementModel>>(request);
			TryThrowResponseException(response);
			return response.Data.Results;
		}


		public async Task<List<ECCategory>> GetECCategoryList(ECColumnType columnType)
		{
			ECColumnType[] columnTypes = new ECColumnType[]{ columnType };
			var request = new RestRequest("/ec/categories/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("ColumnTypes", JsonConvert.SerializeObject(columnTypes), ParameterType.QueryString);
//			request.AddParameter("ColumnType", JsonConvert.SerializeObject(columnType), ParameterType.QueryString);
			request.AddParameter("Cancel", "false");
			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<EcCategoryResponese>>(request);
			TryThrowResponseException(response);
			var list = response.Data.Results.ConvertAll(x => {
				return x.ToECCategoryModel();
			});
			return list;

		}

		public async Task<List<ECPostInfo>> GetRecommendPosts()
		{
			int pagesize = 3;
			ECColumnType[] columnTypes = { ECColumnType.Activity, ECColumnType.Course };

			var request = new RestRequest("/ec/messages/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("ColumnTypes", JsonConvert.SerializeObject(columnTypes), ParameterType.QueryString);
			request.AddParameter("OrderByDesc", "CreateDate", ParameterType.QueryString);
			request.AddParameter("Take", pagesize, ParameterType.QueryString);
			request.AddParameter("Cancel", "false");
			var eCPostInfoResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECPostInfo>>(request);
			TryThrowResponseException(eCPostInfoResponse);

			return eCPostInfoResponse.Data.Results;
		}

		public async Task<List<ECPostInfo>> GetNewECPostInfo(ECColumnType[] columnTypes, long maxId, int[] categoryIds)
		{
			var request = new RestRequest("/ec/messages/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("ColumnTypes", JsonConvert.SerializeObject(columnTypes), ParameterType.QueryString);
			if (categoryIds != null && categoryIds.Length > 0) {
				request.AddParameter("CategoryIds", JsonConvert.SerializeObject(categoryIds), ParameterType.QueryString);
			}
			request.AddParameter("OrderByDesc", "CreateDate", ParameterType.QueryString);
			request.AddParameter("Take", Constants.PAGESIZE, ParameterType.QueryString);
			request.AddParameter("PostInfoIdGreaterThan", maxId);
			request.AddParameter("Cancel", "false");
			var eCPostInfoResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECPostInfo>>(request);
			TryThrowResponseException(eCPostInfoResponse);

			return eCPostInfoResponse.Data.Results;
		}

		public async Task<List<ECPostInfo>> GetOldECPostInfo(ECColumnType[] columnTypes, long minId, int[] categoryIds)
		{
			var request = new RestRequest("/ec/messages/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("ColumnTypes", JsonConvert.SerializeObject(columnTypes), ParameterType.QueryString);
			if (categoryIds != null && categoryIds.Length > 0) {
				request.AddParameter("CategoryIds", JsonConvert.SerializeObject(categoryIds), ParameterType.QueryString);
			}
			request.AddParameter("PostInfoIdLessThan", minId);
			request.AddParameter("OrderByDesc", "CreateDate", ParameterType.QueryString);
			request.AddParameter("Take", Constants.PAGESIZE, ParameterType.QueryString);
			request.AddParameter("Cancel", "false");
			var eCPostInfoResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECPostInfo>>(request);
			TryThrowResponseException(eCPostInfoResponse);
			return eCPostInfoResponse.Data.Results;
		}

		public async Task<ECPostInfo> GetECPostInfoById(int postInfoId)
		{	
			var request = new RestRequest("/ec/messages/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("PostInfoId", postInfoId, ParameterType.QueryString);
			var eCPostInfoResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECPostInfo>>(request);
			TryThrowResponseException(eCPostInfoResponse);
			return eCPostInfoResponse.Data.Results[0];
		}

		public async Task<ECPostInfo> GetAudioService()
		{
			var request = new RestRequest("/ec/media", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			var response = await _iServiceClient.ExecuteTaskAsync<ECPostInfo>(request);
			TryThrowResponseException(response);
			return response.Data;
		}

		public async Task<List<ECReview>> GetECReview(int postInfoId, int pageSize = Constants.PAGESIZE)
		{
			var request = new RestRequest("/ec/review", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("PostInfoId", postInfoId);
			request.AddParameter("Take", pageSize);
			request.AddParameter("OrderByDesc", "CreateDate", ParameterType.QueryString);
			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECReviewResponse>>(request);
			TryThrowResponseException(response);
			var reviews = response.Data.Results.ConvertAll(x => {
				return x.ToECReviewModel();
			});
			return reviews;
		}

		public async Task<List<ECComment>> GetECComment(int postInfoId, int pageSize = Constants.PAGESIZE)
		{
			var request = new RestRequest("/ec/comments", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("PostInfoId", postInfoId);
			request.AddParameter("OrderByDesc", "CreateDate", ParameterType.QueryString);
			request.AddParameter("CommentType", 1);
			request.AddParameter("Take", pageSize);
			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECComment>>(request);
			TryThrowResponseException(response);
			return response.Data.Results;
		}

		public async Task<bool> GetIsPraised(int postInfoId)
		{
			var request = new RestRequest("/ec/comments", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("PostInfoId", postInfoId);
			request.AddParameter("CommentType", 2);
			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECComment>>(request);
			TryThrowResponseException(response);
			var eccomment = response.Data.Results;
			return response.Data.Results.Count != 0;
		}

		public async Task<List<ECComment>> GetOldECCommentList(int postInfoId, long minId)
		{
			var request = new RestRequest("/ec/comments", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("PostInfoId", postInfoId);
			request.AddParameter("CommentIdLessThan", minId);
			request.AddParameter("OrderByDesc", "CreateDate", ParameterType.QueryString);
			request.AddParameter("CommentType", 1);
			request.AddParameter("Cancel", "false");
			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECComment>>(request);
			return response.Data.Results;
		}

		public async Task<List<ECReview>> GetOldECReviewList(int postInfoId, long minId)
		{
			var request = new RestRequest("/ec/review", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("PostInfoId", postInfoId);
			request.AddParameter("ReviewIdLessThan", minId);
			request.AddParameter("OrderByDesc", "CreateDate", ParameterType.QueryString);
			request.AddParameter("Take", Constants.PAGESIZE);
			request.AddParameter("Cancel", "false");
			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECReviewResponse>>(request);
			TryThrowResponseException(response);
			var reviews = response.Data.Results.ConvertAll(x => {
				return x.ToECReviewModel();
			});
			return reviews;
		}

		public async Task<List<ECPostInfo>> GetNewPostCommentList()
		{
			var request = new RestRequest("/ec/messages/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("Take", Constants.PAGESIZE);
			request.AddParameter("UserId", BabyBusContext.UserAllInfo.UserId);
			request.AddParameter("OrderByDesc", "PostInfoId", ParameterType.QueryString);
			request.AddParameter("Cancel", "false");
			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECPostInfo>>(request);
			TryThrowResponseException(response);
			return response.Data.Results;
		}

		public async Task<List<ECPostInfo>> GetOldPostCommentList(long minId)
		{
			var request = new RestRequest("/ec/messages/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("UserId", BabyBusContext.UserAllInfo.UserId);
			request.AddParameter("OrderByDesc", "PostInfoId", ParameterType.QueryString);
			request.AddParameter("PostInfoIdLessThan", minId);
			request.AddParameter("Take", Constants.PAGESIZE);
			request.AddParameter("Cancel", "false");
			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECPostInfo>>(request);
			TryThrowResponseException(response);
			return response.Data.Results;
		}
		//		public async Task<List<ECPostComment>> GetNewPostCommentList()
		//		{
		//			var request = new RestRequest("/ec/postcomment", Method.GET);
		//			request.AddHeader("Content-Type", "application/json; charset=utf-8");
		//			request.AddParameter("Take", Constants.PAGESIZE);
		//			request.AddParameter("UserId", BabyBusContext.UserAllInfo.UserId);
		//			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECPostComment>>(request);
		//			TryThrowResponseException(response);
		//			return response.Data.Results;
		//		}
		//
		//		public async Task<List<ECPostComment>> GetOldPostCommentList(long minId)
		//		{
		//			var request = new RestRequest("/ec/postcomment", Method.GET);
		//			request.AddHeader("Content-Type", "application/json; charset=utf-8");
		//			request.AddParameter("UserId", BabyBusContext.UserAllInfo.UserId);
		//			request.AddParameter("ReviewIdLessThan", minId);
		//			request.AddParameter("Take", Constants.PAGESIZE);
		//			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECPostComment>>(request);
		//			TryThrowResponseException(response);
		//			return response.Data.Results;
		//		}
		public async Task<List<ECPayOrder>> GetNewECPayOrdertList()
		{
			var request = new RestRequest("/ec/payorders", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("Take", Constants.PAGESIZE, ParameterType.QueryString);
			request.AddParameter("OrderByDesc", "OrderNumber", ParameterType.QueryString);
			request.AddParameter("UserId", BabyBusContext.UserAllInfo.UserId);
			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECPayOrder>>(request);
			TryThrowResponseException(response);
			return response.Data.Results;
		}

		public async Task<List<ECPayOrder>> GetOldECPayOrderList(long minId)
		{
			var request = new RestRequest("/ec/payorders", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("UserId", BabyBusContext.UserAllInfo.UserId);
			request.AddParameter("OrderNumberLessThan", minId);
			request.AddParameter("Take", Constants.PAGESIZE, ParameterType.QueryString);
			request.AddParameter("OrderByDesc", "OrderNumber", ParameterType.QueryString);
			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ECPayOrder>>(request);
			TryThrowResponseException(response);
			return response.Data.Results;
		}

		#endregion

		public async Task<List<QuestionModel>> GetOldQuestionList(long minId, RoleType roleType)
		{
			QuestionType[] qt;
			if (roleType == RoleType.HeadMaster) {
				qt = new  []{ QuestionType.MasterMessage };
			} else if (roleType == RoleType.Parent) {
				qt = new  []{ QuestionType.AskforLeave, QuestionType.NormalMessage, QuestionType.PersonalMessage };
			} else {
				qt = new  []{ QuestionType.AskforLeave, QuestionType.NormalMessage, QuestionType.PersonalMessage };
			}
			var request = new RestRequest("questions", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("QuestionIdLessThan", minId, ParameterType.QueryString);
			request.AddParameter("QuestionTypes", JsonConvert.SerializeObject(qt), ParameterType.QueryString);
			request.AddParameter("OrderByDesc", "QuestionId", ParameterType.QueryString);
			request.AddParameter("Take", Constants.PAGESIZE, ParameterType.QueryString);
			var questionResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<QuestionResponse>>(request);
			TryThrowResponseException(questionResponse);

			var questions = questionResponse.Data.Results.ConvertAll(x => {
				return  x.ToQuestionModel();
			});
			return questions;
		}

		public async Task<List<QuestionModel>> GetNewQuestionList(long maxId, RoleType roleType)
		{
			QuestionType[] qt;
			if (roleType == RoleType.HeadMaster) {
				qt = new  []{ QuestionType.MasterMessage };
			} else if (roleType == RoleType.Parent) {
				qt = new  [] {
					QuestionType.AskforLeave,
					QuestionType.NormalMessage,
					QuestionType.PersonalMessage,
					QuestionType.MasterMessage
				};
			} else {
				qt = new  []{ QuestionType.AskforLeave, QuestionType.NormalMessage, QuestionType.PersonalMessage };
			}
			var request = new RestRequest("questions", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("QuestionIdGreaterThan", maxId, ParameterType.QueryString);
			request.AddParameter("QuestionTypes", JsonConvert.SerializeObject(qt), ParameterType.QueryString);
			request.AddParameter("OrderByDesc", "QuestionId", ParameterType.QueryString);
			if (maxId == 0) {
				request.AddParameter("Take", Constants.PAGESIZE, ParameterType.QueryString);
			}
			var questionResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<QuestionResponse>>(request);
			TryThrowResponseException(questionResponse);
        
			var questions = questionResponse.Data.Results.ConvertAll(x => {
				return x.ToQuestionModel();
			});
			return questions;
		}

		public async Task<NoticeModel> GetNoticeById(int noticeId)
		{
			var request = new RestRequest("notices", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("NoticeId", noticeId, ParameterType.QueryString);
			var noticesResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<NoticeResponse>>(request);
			TryThrowResponseException(noticesResponse);
        
			if (noticesResponse.Data.Results.Count == 1) {
				return noticesResponse.Data.Results[0].ToNoticeModel();
			} else {
				throw new UnexpectedResponseDataException();
			}
		}

        
		public async Task<List<NoticeModel>> GetOldNoticeList(NoticeViewType viewtype, long minId)
		{
			var notices = new List<NoticeModel>();
			//Get Class Notice
			var request = new RestRequest("notices", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			if (BabyBusContext.RoleType != RoleType.HeadMaster) {
				var types = new List<NoticeType>();
				if (viewtype == NoticeViewType.Notice) {
					types.Add(NoticeType.ClassCommon);
					types.Add(NoticeType.ClassHomework);
					types.Add(NoticeType.ClassEmergency);
				} else {
					types.Add(NoticeType.GrowMemory);
				}
				request.AddParameter("NoticeTypes", JsonConvert.SerializeObject(types.ToArray()), ParameterType.QueryString);
				request.AddParameter("KindergartenId", BabyBusContext.KindergartenId, ParameterType.QueryString);
				request.AddParameter("ClassId", BabyBusContext.ClassId, ParameterType.QueryString);
				request.AddParameter("NoticeIdLessThan", minId, ParameterType.QueryString);
				request.AddParameter("OrderByDesc", "NoticeId", ParameterType.QueryString);
				request.AddParameter("Take", Constants.PAGESIZE, ParameterType.QueryString);
				var list = await GetNoticeList(request);
				notices.AddRange(list);
			}

			if (viewtype != NoticeViewType.GrowMemory) {
				//Get Kindergarten Notice
				request = new RestRequest("notices", Method.GET);
				request.AddHeader("Content-Type", "application/json; charset=utf-8");
				var types = new List<NoticeType>();
				types.Add(NoticeType.KindergartenAll);
				types.Add(NoticeType.KindergartenRecipe);
				if (BabyBusContext.RoleType != RoleType.Parent) {
					types.Add(NoticeType.KindergartenStaff);
				} else {
					types.Add(NoticeType.BabyBusNotice);
					types.Add(NoticeType.BabyBusNoticeHtml);
				}
				request.AddParameter("NoticeTypes", JsonConvert.SerializeObject(types.ToArray()), ParameterType.QueryString);
				request.AddParameter("KindergartenId", BabyBusContext.KindergartenId, ParameterType.QueryString);
				request.AddParameter("NoticeIdLessThan", minId, ParameterType.QueryString);
				request.AddParameter("OrderByDesc", "NoticeId", ParameterType.QueryString);
				request.AddParameter("Take", Constants.PAGESIZE, ParameterType.QueryString);
				var list = await GetNoticeList(request);
				notices.AddRange(list);

				//Get Global Notice
				if (BabyBusContext.RoleType == RoleType.Parent) {
					request = new RestRequest("notices", Method.GET);
					request.AddHeader("Content-Type", "application/json; charset=utf-8");
					types = new List<NoticeType>();
					types.Add(NoticeType.BabyBusNotice);
					types.Add(NoticeType.BabyBusNoticeHtml);
					request.AddParameter("NoticeTypes", JsonConvert.SerializeObject(types.ToArray()), ParameterType.QueryString);
					request.AddParameter("KindergartenId", "0");
					request.AddParameter("NoticeIdLessThan", minId, ParameterType.QueryString);
					request.AddParameter("OrderByDesc", "NoticeId", ParameterType.QueryString);
					request.AddParameter("Take", Constants.PAGESIZE, ParameterType.QueryString);
					list = await GetNoticeList(request);
					notices.AddRange(list);
				}
			}

			return notices.OrderByDescending(x => x.NoticeId).Take(Constants.PAGESIZE).ToList();
		}

		public async Task<List<NoticeModel>> GetNewNoticeList(NoticeViewType viewtype, long maxId)
		{
			var notices = new List<NoticeModel>();
			//Get Class Notice
			var request = new RestRequest("notices", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			if (BabyBusContext.RoleType != RoleType.HeadMaster) {
				var types = new List<NoticeType>();
				if (viewtype == NoticeViewType.Notice) {
					types.Add(NoticeType.ClassCommon);
					types.Add(NoticeType.ClassHomework);
					types.Add(NoticeType.ClassEmergency);
				} else {
					types.Add(NoticeType.GrowMemory);
				}
				request.AddParameter("NoticeTypes", JsonConvert.SerializeObject(types.ToArray()), ParameterType.QueryString);
				request.AddParameter("KindergartenId", BabyBusContext.KindergartenId, ParameterType.QueryString);
				request.AddParameter("ClassId", BabyBusContext.ClassId, ParameterType.QueryString);
				request.AddParameter("NoticeIdGreaterThan", maxId, ParameterType.QueryString);
				request.AddParameter("OrderByDesc", "NoticeId", ParameterType.QueryString);
				if (maxId == 0) {
					request.AddParameter("Take", Constants.PAGESIZE, ParameterType.QueryString);
				}
				var list = await GetNoticeList(request);
				notices.AddRange(list);
			}
				
			if (viewtype != NoticeViewType.GrowMemory) {
				//Get Kindergarten Notice
				request = new RestRequest("notices", Method.GET);
				request.AddHeader("Content-Type", "application/json; charset=utf-8");
				var types = new List<NoticeType>();
				types.Add(NoticeType.KindergartenAll);
				types.Add(NoticeType.KindergartenRecipe);
				if (BabyBusContext.RoleType != RoleType.Parent) {
					types.Add(NoticeType.KindergartenStaff);
				} else {
					types.Add(NoticeType.BabyBusNotice);
					types.Add(NoticeType.BabyBusNoticeHtml);
				}
				request.AddParameter("NoticeTypes", JsonConvert.SerializeObject(types.ToArray()), ParameterType.QueryString);
				request.AddParameter("KindergartenId", BabyBusContext.KindergartenId, ParameterType.QueryString);
				request.AddParameter("NoticeIdGreaterThan", maxId, ParameterType.QueryString);
				request.AddParameter("OrderByDesc", "NoticeId", ParameterType.QueryString);
				if (maxId == 0) {
					request.AddParameter("Take", Constants.PAGESIZE, ParameterType.QueryString);
				}
				var list = await GetNoticeList(request);
				notices.AddRange(list);

				//Get Global Notice
				if (BabyBusContext.RoleType == RoleType.Parent) {
					request = new RestRequest("notices", Method.GET);
					request.AddHeader("Content-Type", "application/json; charset=utf-8");
					types = new List<NoticeType>();
					types.Add(NoticeType.BabyBusNotice);
					types.Add(NoticeType.BabyBusNoticeHtml);
					request.AddParameter("NoticeTypes", JsonConvert.SerializeObject(types.ToArray()), ParameterType.QueryString);
					request.AddParameter("KindergartenId", "0");
					request.AddParameter("NoticeIdGreaterThan", maxId, ParameterType.QueryString);
					request.AddParameter("OrderByDesc", "NoticeId", ParameterType.QueryString);
					if (maxId == 0) {
						request.AddParameter("Take", Constants.PAGESIZE, ParameterType.QueryString);
					}
					list = await GetNoticeList(request);
					notices.AddRange(list);
				}
			}

			return notices.OrderByDescending(x => x.NoticeId).Take(Constants.PAGESIZE).ToList();
		}

		async Task<List<NoticeModel>> GetNoticeList(RestRequest request)
		{
			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<NoticeResponse>>(request);
			TryThrowResponseException(response);
			var notices = response.Data.Results.ConvertAll(x => {
				return x.ToNoticeModel();
			});
			return notices;
		}

		public async Task<NoticeModel> GetLatestKindergartensNotice()
		{
			var request = new RestRequest("notices", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("NoticeTypes", JsonConvert.SerializeObject(new [] {
				NoticeType.KindergartenAll,
				NoticeType.KindergartenRecipe
			})
                , ParameterType.QueryString);
			request.AddParameter("KindergartenId", BabyBusContext.KindergartenId, ParameterType.QueryString);
			request.AddParameter("Take", 1, ParameterType.QueryString);
			request.AddParameter("OrderByDesc", "NoticeId", ParameterType.QueryString);
			var kindergartensNotices = await _iServiceClient.ExecuteTaskAsync<QueryResponse<NoticeResponse>>(request);
           
			TryThrowResponseException(kindergartensNotices);
           
			if (kindergartensNotices.Data.Results.Count == 1) {
				return kindergartensNotices.Data.Results[0].ToNoticeModel();
			} else {
				throw new UnexpectedResponseDataException();
			}
		}

		public async Task<NoticeModel> GetLatestClassNotice()
		{
			var request = new RestRequest("notices", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			var types = JsonConvert.SerializeObject(new [] { NoticeType.ClassCommon, NoticeType.ClassHomework });
			request.AddParameter("NoticeTypes", types, ParameterType.QueryString);
			request.AddParameter("Take", 1, ParameterType.QueryString);
			request.AddParameter("KindergartenId", BabyBusContext.KindergartenId, ParameterType.QueryString);
			request.AddParameter("ClassId", BabyBusContext.ClassId, ParameterType.QueryString);
			request.AddParameter("OrderByDesc", "NoticeId", ParameterType.QueryString);
			request.AddParameter("format", "json", ParameterType.QueryString);

			var classesNotices = await _iServiceClient.ExecuteTaskAsync<QueryResponse<NoticeResponse>>(request);
			TryThrowResponseException(classesNotices);
          
			if (classesNotices.Data.Results.Count == 1) {
				return classesNotices.Data.Results[0].ToNoticeModel();
			} else {
				throw new UnexpectedResponseDataException();
			}
		}

		public async Task<NoticeModel> GetLatestMemory()
		{
			var request = new RestRequest("notices", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("NoticeTypes", new [] { NoticeType.GrowMemory }, ParameterType.QueryString);
			request.AddParameter("Take", 1, ParameterType.QueryString);
			request.AddParameter("OrderByDesc", "NoticeId", ParameterType.QueryString);
			var memories = await _iServiceClient.ExecuteTaskAsync<QueryResponse<NoticeResponse>>(request);
			TryThrowResponseException(memories);

			if (memories.Data.Results.Count == 1) {
				return memories.Data.Results[0].ToNoticeModel();
			} else {
				throw new UnexpectedResponseDataException();
			}
		}

		public async Task<VersionModel> GetVersionByAppType(int appType)
		{
			var request = new RestRequest("version", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("AppType", appType, ParameterType.QueryString);
			var response = await _iServiceClient.ExecuteTaskAsync<VersionResponse>(request);
			TryThrowResponseException(response);
			var ret = response.Data.ToVersionModel();
			return ret;
		}



		public async Task<List<ChildModel>> GetChildren()
		{
			var request = new RestRequest("/children", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			var childResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ChildResponse>>(request);
			TryThrowResponseException(childResponse);
           
			List<ChildModel> childrenWithParents = childResponse.Data.Results.ConvertAll(x => x.ToChildModel());
			List<ChildModel> childrenWithOneParent = ChooseOneParentWhoHasPhone(childrenWithParents);
			return childrenWithOneParent;
		}

		public async Task<List<ChildModel>> GetChildrenAttendance(DateTimeOffset date)
		{
			var request = new RestRequest("/children/attendance", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("Date", date, ParameterType.QueryString);
			request.AddParameter("ClassId", BabyBusContext.UserAllInfo.ClassId);
			var childAttendanceResponse = await _iServiceClient.ExecuteTaskAsync<List<ChildModel>>(request);
			TryThrowResponseException(childAttendanceResponse);
			List<ChildModel> children = childAttendanceResponse.Data;
			return children;
		}

		public async Task<ChildModel> GetChildByParentId(long id)
		{
			var request = new RestRequest("children", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("ParentId", id, ParameterType.QueryString);
			var childResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ChildResponse>>(request);
			TryThrowResponseException(childResponse);
           
			if (childResponse.Data.Results.Count == 1) {
				return childResponse.Data.Results[0].ToChildModel();
			} else {
				throw new UnexpectedResponseDataException();
			}
		}

		public async Task<bool> UpdateChild(ChildModel child)
		{
			var updateChild = Mapper.DynamicMap<UpdateChild>(child);
			updateChild.Id = child.ChildId;
			updateChild.Birthday = child.Birthday;
			var request = new RestRequest("children", Method.PUT);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			request.AddBody(updateChild);
			var ret = await _iServiceClient.ExecuteTaskAsync<bool>(request);
			TryThrowResponseException(ret);
			return ret.Data;
		}

		public async  Task<bool> UpdateUser(UserModel user)
		{
			var updateUser = Mapper.Map<UpdateUser>(user);
//            user.ConvertTo<UpdateUser>();
			updateUser.Id = user.UserId;
			var request = new RestRequest("users", Method.PUT);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			request.AddBody(updateUser);
			var ret = await _iServiceClient.ExecuteTaskAsync<bool>(request);
			TryThrowResponseException(ret);
			return ret.Data;
		}

		public async Task<KindergartenClassModel> GetClass(long classId)
		{
			var request = new RestRequest("classes", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("Id", classId);
			var clsResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<ClassResponse>>(request);
			TryThrowResponseException(clsResponse);
          
			if (clsResponse.Data.Results.Count >= 1) {
				return clsResponse.Data.Results[0].ToKindergartenClassModel();
			} else {
				throw new  UnexpectedResponseDataException();
			}
		}

		public async Task<KindergartenModel> GetKindergarten()
		{
			var request = new RestRequest("kindergartens", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			var kindergartenResponse = await _iServiceClient.ExecuteTaskAsync<QueryResponse<KindergartenResponse>>(request);

			TryThrowResponseException(kindergartenResponse);

			if (kindergartenResponse.Data.Results.Count == 1) {
				var kid = kindergartenResponse.Data.Results[0].ToKindergartenModel();
//                kid.id?
				return kid;
			} else {
				throw new  UnexpectedResponseDataException();
			}
		}

      

		public async Task<bool> Logout()
		{
			var request = new RestRequest("/authenticate/{provider}", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddUrlSegment("provider", "logout");
			var authResponse = await _iServiceClient.ExecuteTaskAsync<AuthenticateResponse>(request);
			TryThrowResponseException(authResponse);
			return true;
		}

		public async Task<AuthenticateResponse> LoginWithWechat(string code)
		{
			var request = new RestRequest("/authenticate/wechat", Method.GET);
			request.RequestFormat = DataFormat.Json;
			request.AddQueryParameter("Code", code);
//            request.AddQueryParameter("RoleType", Constants.Parent);

			var authResponse = await _iServiceClient.ExecuteTaskAsync<AuthenticateResponse>(request);

			TryThrowResponseException(authResponse);

			//set cookies
			_iServiceClient.CookieContainer = new CookieContainer();
			foreach (var sessionCookie in authResponse.Cookies) {
				var cookie = new Cookie(sessionCookie.Name, sessionCookie.Value, sessionCookie.Path, sessionCookie.Domain);
				_cookies.Add(cookie);
				_iServiceClient.CookieContainer.Add(cookie);
			}

			return authResponse.Data;
		}

		public async Task<AuthenticateResponse> Login(string name, string password, RoleType roleType, bool rememberMe = true)
		{
			var request = new RestRequest("/authenticate/credentials", Method.GET);
			request.RequestFormat = DataFormat.Json;
			request.AddQueryParameter("Username", name);
			request.AddQueryParameter("Password", password);
			request.AddQueryParameter("Remeberme", rememberMe.ToString());
			var roleStr = Constants.RoleTypeString(roleType);
			var meta = string.Format("{{\"RoleType\":{0}}}", roleStr);
			request.AddQueryParameter("meta", meta);

			var authResponse = await _iServiceClient.ExecuteTaskAsync<AuthenticateResponse>(request);

			TryThrowResponseException(authResponse);

			//set cookies
			_iServiceClient.CookieContainer = new CookieContainer();
			foreach (var sessionCookie in authResponse.Cookies) {
				var cookie = new Cookie(sessionCookie.Name, sessionCookie.Value, sessionCookie.Path, sessionCookie.Domain);
				_cookies.Add(cookie);
				_iServiceClient.CookieContainer.Add(cookie);
			}

			return authResponse.Data;

		}


		List<ChildModel> ChooseOneParentWhoHasPhone(List<ChildModel> childrenWithParents)
		{
			var childrenWithOneParent = new List<ChildModel>();
			foreach (var childWithParents in childrenWithParents) {
				bool find = false;
				foreach (var childWithOneParent in childrenWithOneParent) {
					if (childWithParents.ChildId == childWithOneParent.ChildId) {
						find = true;
						//尝试找到一个有电话的
						if (childWithParents.PhoneNumber != null) {
							childWithOneParent.PhoneNumber = childWithParents.PhoneNumber;
							childWithOneParent.ParentName = childWithParents.ParentName;
							break;
						}
					}
				}
				if (!find) {
					childrenWithOneParent.Add(childWithParents);
				}
			}
			return childrenWithOneParent;
		}

		void TryThrowResponseException(IRestResponse response)
		{
			if (response.StatusCode == HttpStatusCode.Unauthorized && response.StatusDescription == "Invalid UserName or Password") {
				throw new BabyBusWebServiceException(response.StatusCode, response.StatusDescription, "用户名或者密码错误", response.ErrorException);
			}
			if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent) {
				throw new BabyBusWebServiceException(response.StatusCode, response.StatusDescription, UIConstants.WEB_EXCEPTION, response.ErrorException);
			}
		}

		#region Test

		public async Task<PhysicalExaminationResult> GetPhysicalExaminationResult(long childId)
		{
			var request = new RestRequest("/physicalExamination/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");

			request.AddQueryParameter("ChildId", childId.ToString());
			request.AddQueryParameter("OrderByDesc", "CreateTime");
			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<PhysicalExaminationResult>>(request);
			TryThrowResponseException(response);
			if (response.Data.Results.Count > 0)
				return response.Data.Results[0];
			else
				return new PhysicalExaminationResult();
		}

		public async Task<int> UpdatePhysicalExaminationResult(UpdatePhysicalExamination model)
		{
			var request = new RestRequest("/physicalExamination/", Method.PUT);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("Accept", "application/json");
			request.AddBody(model);
			var ret = await _iServiceClient.ExecuteTaskAsync<int>(request);
			TryThrowResponseException(ret);
			return ret.Data;
		}

		public async Task<AddedValue> GetAddedValue(long kindergartenId)
		{
			long[] kIds = { 0, kindergartenId };
			var request = new RestRequest("/addedvalue/", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			request.AddParameter("KindergartenIds", JsonConvert.SerializeObject(kIds));
			request.AddParameter("OrderByDesc", "KindergartenId");
			var response = await _iServiceClient.ExecuteTaskAsync<QueryResponse<AddedValue>>(request);
			TryThrowResponseException(response);
			return response.Data.Results[0];
		}


		#endregion

		#region Album

		public async Task<List<AlbumModel>> GetAlbums()
		{
			var request = new RestRequest("/albums", Method.GET);
			request.AddHeader("Content-Type", "application/json; charset=utf-8");
			var response = await _iServiceClient.ExecuteTaskAsync<List<AlbumModel>>(request);
			TryThrowResponseException(response);
			return response.Data;
		}

		#endregion
	}
}

