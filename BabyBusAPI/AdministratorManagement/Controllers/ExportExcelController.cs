using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using AutoMapper;
using BabyBus.AutoModel;
using BabyBus.Core.Helper;
using System.Web.Http;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using AdministratorManagement.Models;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using BabyBus.NPOI;


namespace AdministratorManagement.Controllers
{
    public class ExportExcelController : BabyBusApiController
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ExportExcelController));

        public ExportExcelController() {
        }

        public enum ExcelDataRequestType
        {

            Kindergarten = 1,
            Class = 2,
            Child = 3,
        }

        public class ExcelDataRequest
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int KindergartenId { get; set; }
            public int Type { get; set; }
            public int ClassId { get; set; }
            public ExcelDataRequestType ExcelType { get; set; }

        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(int kindergartenId, int excelType) {
            HttpResponseMessage response = null;
            if (excelType == (decimal)ExcelDataRequestType.Kindergarten) {
                response = GenerateExlsInfoForKindergarten(kindergartenId);
            }
            else if (excelType == (decimal)ExcelDataRequestType.Class) {
                response = GenerateExlsInfoForClass(kindergartenId);
            }
            else if (excelType == (decimal)ExcelDataRequestType.Child) {
                return GenerateExlsInfoForChild(kindergartenId);
            }

            return response;
        }
        private HttpResponseMessage GenerateExlsInfoForKindergarten(int kindergartenId) {
            StartWatch();
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            try {
                NPOIHelper.ListColumnsName = new SortedList(new NoSort());
                NPOIHelper.ListColumnsName.Add("Year", "年份");
                NPOIHelper.ListColumnsName.Add("KindergartenName", "幼儿园名称");
                NPOIHelper.ListColumnsName.Add("ClassName", "班级名称");
                NPOIHelper.ListColumnsName.Add("Total", "总数");
                NPOIHelper.ListColumnsName.Add("Attence", "实际出勤");
                NPOIHelper.ListColumnsName.Add("UnAttence", "未出勤");


                //获取当前的年份和月份
                int currentYear = DateTime.Now.Year;
                DataTable dt;
                using (var db = new BabyBus_Entities()) {
                    var data = db.UP_Attendance_GenerateExlsInfoForKindergarten(currentYear, kindergartenId);

                    dt = Utilities.ToDataTable(data.ToList());
                }//using end



                //下载Excel文件到客户端
                response.Headers.CacheControl = new CacheControlHeaderValue() {
                    MaxAge = TimeSpan.FromMinutes(20)
                };
                string filename = HttpUtility.UrlEncode(DateTime.Now.ToString("按年考勤记录yyyyMMdd"));
                MemoryStream ms = new MemoryStream();
                NPOIHelper.ExportExcel(dt, ms);
                response.Content = new ByteArrayContent(ms.ToArray());
                //response.Content = new StreamContent(ms);
                ContentDispositionHeaderValue d = new ContentDispositionHeaderValue("attachment");
                d.FileName = filename + ".xls";
                response.Content.Headers.ContentDisposition = d;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");

            }
            catch (Exception ex) {
                response.StatusCode = HttpStatusCode.ExpectationFailed;
                Log.Fatal(ex.Message, ex);
                return response;
            }
            EndWatch();
            return response;
        }
        private HttpResponseMessage GenerateExlsInfoForClass(int kindergartenId) {
            StartWatch();
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            try {
                NPOIHelper.ListColumnsName = new SortedList(new NoSort());
                NPOIHelper.ListColumnsName.Add("Year", "年份");
                NPOIHelper.ListColumnsName.Add("Month", "月份");
                NPOIHelper.ListColumnsName.Add("KindergartenName", "幼儿园名称");
                NPOIHelper.ListColumnsName.Add("ClassName", "班级名称");
                NPOIHelper.ListColumnsName.Add("Total", "总数");
                NPOIHelper.ListColumnsName.Add("Attence", "出勤");
                NPOIHelper.ListColumnsName.Add("UnAttence", "未出勤");


                //获取当前的年份和月份
                int currentYear = DateTime.Now.Year;
                DataTable dt;
                using (var db = new BabyBus_Entities()) {
                    var data = db.UP_Attendance_GenerateExlsInfoForClass(currentYear, kindergartenId);

                    dt = Utilities.ToDataTable(data.ToList());
                }//using end



                //下载Excel文件到客户端
                response.Headers.CacheControl = new CacheControlHeaderValue() {
                    MaxAge = TimeSpan.FromMinutes(20)
                };
                string filename = HttpUtility.UrlEncode(DateTime.Now.ToString("按月考勤记录-yyyyMMdd"));
                MemoryStream ms = new MemoryStream();
                NPOIHelper.ExportExcel(dt, ms);
                response.Content = new ByteArrayContent(ms.ToArray());
                //response.Content = new StreamContent(ms);
                ContentDispositionHeaderValue d = new ContentDispositionHeaderValue("attachment");
                d.FileName = filename + ".xls";
                response.Content.Headers.ContentDisposition = d;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");

            }
            catch (Exception ex) {
                response.StatusCode = HttpStatusCode.ExpectationFailed;
                Log.Fatal(ex.Message, ex);
                return response;
            }
            EndWatch();
            return response;
        }

        private HttpResponseMessage GenerateExlsInfoForChild(int kindergartenId) {
            StartWatch();
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            try {

                //获取当前的年份和月份
                int currentYear = DateTime.Now.Year;
                DataTable dt;

                string str = @"server=babybusminmin.sqlserver.rds.aliyuncs.com,3433;uid=mingming;pwd=asd123456; Trusted_Connection=no;database=babybus_test;";

                SqlConnection conn = new SqlConnection(str);
                try {
                    conn.Open();
                    DataSet ds = new DataSet();

                    SqlParameter[] sps = {
                        new SqlParameter("@year", SqlDbType.Int)
                        , new SqlParameter("@kindergartenId", SqlDbType.Int)
                    };
                    sps[0].Value = currentYear;
                    sps[1].Value = kindergartenId;
                    SqlCommand cmd = new SqlCommand("UP_Attendance_GenerateExlsInfoForChild", conn);

                    foreach (SqlParameter ps in sps) {
                        cmd.Parameters.Add(ps);
                    }
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                    dt = ds.Tables[0];
                    
                }
                catch (Exception ex) {
                    response.StatusCode = HttpStatusCode.ExpectationFailed;
                    Log.Fatal(ex.Message, ex);
                    return response;
                }
                finally {
                    conn.Close();
                }

                //下载Excel文件到客户端
                response.Headers.CacheControl = new CacheControlHeaderValue() {
                    MaxAge = TimeSpan.FromMinutes(20)
                };
                string filename = HttpUtility.UrlEncode(DateTime.Now.ToString("宝宝考勤记录-yyyyMMdd"));
                MemoryStream ms = new MemoryStream();
                ExportChildrenAttandenceExcel.Execute(dt, ref ms);
                response.Content = new ByteArrayContent(ms.ToArray());
                //response.Content = new StreamContent(ms);
                ContentDispositionHeaderValue d = new ContentDispositionHeaderValue("attachment");
                d.FileName = filename + ".xls";
                response.Content.Headers.ContentDisposition = d;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");

            }
            catch (Exception ex) {
                response.StatusCode = HttpStatusCode.ExpectationFailed;
                Log.Fatal(ex.Message, ex);
                return response;
            }
            EndWatch();
            return response;
        }
    }
}