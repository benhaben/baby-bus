using System;
using System.Text.RegularExpressions;

namespace BabyBus.Helpers {
    public static class Utils {
        private static DateTime _startTime = new DateTime(1970, 1, 1).AddHours(8);

        public static DateTime ConvertIntDateTime(int d) {
            DateTime time = _startTime.AddSeconds(d);
            return time;
        }

        /// <summary>
        /// DateTime转Int（TimeStamp）
        /// </summary>
        /// <returns></returns>
        public static int ConvertDateTimeInt(DateTime d) {
            var stamp = Convert.ToInt32((d - _startTime).TotalSeconds);
            return stamp;
        }

        /// <summary>
        /// DateTime转String
        /// </summary>
        public static string DateTimeString(DateTime datetime) {
            var timeSpan = DateTime.Now.Date - datetime.Date;
            if (timeSpan.Days == 0) {
                return string.Format("今天 {0}", datetime.ToString("HH:mm"));
            }
            if (timeSpan.Days == 1) {
                return string.Format("昨天 {0}", datetime.ToString("HH:mm"));
            }
            return datetime.ToString("yyyy年M月d日 HH:mm");
        }

        /// <summary>
        /// DateTime转Int（TimeStamp），默认当前时间
        /// </summary>
        /// <returns></returns>
        public static int ConvertDateTimeInt() {
            return ConvertDateTimeInt(DateTime.Now);
        }

        //验证电话号码的主要代码如下：

        static public bool IsTelephone(string str_telephone) {

            return System.Text.RegularExpressions.Regex.IsMatch(str_telephone, @"^(\d{3,4}-)?\d{6,8}$");

        }

        //验证手机号码的主要代码如下：

        static public bool IsHandset(string str_handset) {

            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^[1]+[3,4,5,6,7,8,9]+\d{9}");

        }

        //验证身份证号的主要代码如下：

        static public bool IsIDcard(string str_idcard) {

            return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"(^\d{17}(?:\d|x)$)|(^\d{15}$)");

        }

        //验证输入为数字的主要代码如下：

        static public bool IsNumber(string str_number) {

            return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"^[0-9]*$");

        }

        //验证邮编的主要代码如下：

        static public bool IsPostalcode(string str_postalcode) {

            return System.Text.RegularExpressions.Regex.IsMatch(str_postalcode, @"^\d{6}$");

        }


        //密码字符包括：小写字母、大写字母、数字、符号等。

        //这个正则会得到五个捕获组，前四个捕获组会告诉我们这个字符串包含有多少种组合，第五个捕获组如果这个字符串大于6位的话则会得到匹配。

        //最终将匹配项加起来计算长度，长度为1即强度为1，如果没有输入，强度为0。

        //如果要判断包含四种类型的字符中的三种，则判断长度大于3。
        static public bool CheckPasswordStrength(string pwd) {
            return Regex.Replace(pwd, "^(?:([a-z])|([A-Z])|([0-9])|(.)){6,}|(.)+$", "$1$2$3$4$5").Length >= 2;
        }
    }
}