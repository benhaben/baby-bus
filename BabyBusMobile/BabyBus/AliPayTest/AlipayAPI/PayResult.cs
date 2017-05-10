using Android.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlipayApp.AlipayAPI
{
    public class PayResult
    {
        private string resultStatus;
        private string result;
        private string memo;

        public PayResult(string rawResult)
        {

            if (TextUtils.IsEmpty(rawResult))
                return;

            string[] resultParams = rawResult.Split(';');
            foreach (string resultParam in resultParams)
            {
                if (resultParam.StartsWith("resultStatus"))
                {
                    resultStatus = gatValue(resultParam, "resultStatus");
                }
                if (resultParam.StartsWith("result"))
                {
                    result = gatValue(resultParam, "result");
                }
                if (resultParam.StartsWith("memo"))
                {
                    memo = gatValue(resultParam, "memo");
                }
            }
        }

        public  string toString()
        {
            return "resultStatus={" + resultStatus + "};memo={" + memo + "};result={" + result + "}";
        }

        private string gatValue(string content, string key)
        {
            string prefix = key + "={";
            return content.Substring(content.IndexOf(prefix) + prefix.Length, content.LastIndexOf("}"));
        }

        public string getResultStatus()
        {
            return resultStatus;
        }

        public string getMemo()
        {
            return memo;
        }

        public string getResult()
        {
            return result;
        }
    }
}
