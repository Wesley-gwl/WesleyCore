using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace WesleyUntity
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class Extensions
    {
        static Extensions()
        {
            AddRule();
        }

        #region object 扩展方法

        /// <summary>
        /// 判断对象是否为null, string.Empty 或空白字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullEmpty(this object value)
        {
            if ((value != null) && !(value.ToString() == string.Empty))
            {
                return (value.ToString().Trim().Length == 0);
            }
            return true;
        }

        /// <summary>
        /// 将对象转换为字符串类型，如果对象是null则转换为string.Empty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStringOrEmpty(this object value)
        {
            return value == null ? string.Empty : value.ToString();
        }

        #region 类型转换

        /// <summary>
        /// 转换在布而类型 bool
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">转换失败后的默认值</param>
        /// <returns></returns>
        public static bool? ToBoolean(this object value, bool? defaultValue = null)
        {
            if (value == null) return defaultValue;
            return bool.TryParse(value.ToString(), out bool ret) ? ret : defaultValue;
        }

        /// <summary>
        /// 将字符串转换为日期类型
        /// </summary>
        /// <param name="value">包含要转换的日期和时间的字符串</param>
        /// <param name="defaultValue">转换失败后的默认值</param>
        /// <param name="format">str 格式</param>
        /// <param name="dts">一个或多个枚举值的按位组合，指示 str 允许使用的格式。</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this object value, DateTime? defaultValue = null, string format = null, DateTimeStyles dts = DateTimeStyles.None)
        {
            if (value == null) return defaultValue;
            DateTime dt;
            if (string.IsNullOrEmpty(format))
            {
                return DateTime.TryParse(value.ToString(), out dt) ? dt : defaultValue;
            }
            else
            {
                return DateTime.TryParseExact(value.ToString(), format, CultureInfo.CurrentCulture, dts, out dt)
                    ? dt : defaultValue;
            }
        }

        /// <summary>
        /// 转成32位有符号整形（Int32）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">转换失败后的默认值</param>
        /// <returns></returns>
        public static int? ToInt(this object value, int? defaultValue = null)
        {
            if (value == null) return defaultValue;
            if (value.GetType().IsNumberType()) return Convert.ToInt32(value);
            return int.TryParse(value.ToString(), out int ret) ? ret : defaultValue;
        }

        /// <summary>
        /// 转成64位有符号整形（Int64）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">转换失败后的默认值</param>
        /// <returns></returns>
        public static long? ToLong(this object value, long? defaultValue = null)
        {
            if (value == null) return defaultValue;
            if (value.GetType().IsNumberType()) return Convert.ToInt64(value);
            return long.TryParse(value.ToString(), out long ret) ? ret : defaultValue;
        }

        /// <summary>
        /// 转成单精度浮点数字类型（float）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">转换失败后的默认值</param>
        /// <returns></returns>
        public static float? ToSingle(this object value, float? defaultValue = null)
        {
            if (value == null) return defaultValue;
            if (value.GetType().IsNumberType()) return Convert.ToSingle(value);
            return float.TryParse(value.ToString(), out float ret) ? ret : defaultValue;
        }

        /// <summary>
        /// 转成双精度浮点数字类型（double）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">转换失败后的默认值</param>
        /// <returns></returns>
        public static double? ToDouble(this object value, double? defaultValue = null)
        {
            if (value == null) return defaultValue;
            if (value.GetType().IsNumberType()) return Convert.ToDouble(value);
            return double.TryParse(value.ToString(), out double ret) ? ret : defaultValue;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">转换失败后的默认值</param>
        /// <returns></returns>
        public static decimal? ToDecimal(this object value, decimal? defaultValue = null)
        {
            if (value == null) return defaultValue;
            if (value.GetType().IsNumberType()) return Convert.ToDecimal(value);
            return decimal.TryParse(value.ToString(), out decimal ret) ? ret : defaultValue;
        }

        /// <summary>
        /// 正则判断是否为Guid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsGuidByReg(this object value)
        {
            if (value == null)
                return false;
            Regex reg = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
            if (reg.IsMatch(value.ToStringOrEmpty()))
            {
                if (Guid.Empty != Guid.Parse(value.ToString()))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// object转Guid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid? ToGuid(this object value)
        {
            if (value.IsGuidByReg())
                return Guid.Parse(value.ToString());
            else
                return null;
        }

        #endregion 类型转换

        #endregion object 扩展方法

        #region DateTime? 扩展方法

        /// <summary>
        /// 将日期转换为指定格式的字符串,如果日期为 null, 则返回string.Empty
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToString(this DateTime? dt, string format)
        {
            if (dt.HasValue) return dt.Value.ToString(format);
            else return string.Empty;
        }

        /// <summary>
        /// 计算具体某个日期是星期几
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string CaculateWeekDay(this DateTime? dt)
        {
            string weekstr = string.Empty;
            if (!dt.HasValue)
                return weekstr;
            var y = dt.Value.Year;
            var m = dt.Value.Month;
            var d = dt.Value.Day;
            if (m == 1 || m == 2)
            {
                m += 12;
                y--;
            }
            int week = (d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7;

            switch (week)
            {
                case 0: weekstr = "星期一"; break;
                case 1: weekstr = "星期二"; break;
                case 2: weekstr = "星期三"; break;
                case 3: weekstr = "星期四"; break;
                case 4: weekstr = "星期五"; break;
                case 5: weekstr = "星期六"; break;
                case 6: weekstr = "星期七"; break;
            }
            return weekstr;
        }

        #endregion DateTime? 扩展方法

        #region string 扩展方法

        public static T DeserializeJsonStrTo<T>(this string jsonStr) where T : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonStr))
                    return null;
                var result = JsonConvert.DeserializeObject<T>(jsonStr);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string SerializeToJsonStr<T>(this T t) where T : class
        {
            if (t == null)
                return "";
            var str = JsonConvert.SerializeObject(t);
            return str;
        }

        /// <summary>
        /// 清除字符串中的空白字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearWhiteSpace(this string str)
        {
            return Regex.Replace(str, @"\s*", "");
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="beginTag">开始位置</param>
        /// <param name="endTag">结束位置</param>
        /// <param name="beginOffset">截取结果的检索位置</param>
        /// <returns></returns>
        public static string GetBetweenString(this string str, string beginTag, string endTag, int beginOffset = 1)
        {
            int startIndex = string.IsNullOrEmpty(beginTag) ? -1 : str.IndexOf(beginTag);
            int endIndex = str.LastIndexOf(endTag);
            int length = endIndex - (startIndex);
            return !string.IsNullOrEmpty(beginTag) && startIndex < 0 || length <= 0
                ? string.Empty : str.Substring(startIndex + beginOffset, length - beginOffset);
        }

        /// <summary>
        /// 将String转为byte[]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToByteUTF8(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// 将String转为byte[]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToByteASCII(this string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        /// 将String转为byte[]，截取长度lengthMax
        /// </summary>
        /// <param name="str"></param>
        /// <param name="lengthMax"></param>
        /// <returns></returns>
        public static byte[] ToByteUTF8(this string str, int lengthMax)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            if (bytes.Length > lengthMax)
            {
                byte[] byteNew = new byte[lengthMax];
                Array.Copy(bytes, byteNew, lengthMax);
                return byteNew;
            }
            else
                return bytes;
        }

        /// <summary>
        /// null的byte[]转为byte[0]，底层插入数据库需要用到判断
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToByte(this byte[] arr)
        {
            return arr ?? new byte[0];
        }

        public static byte[] ToByteUnicode(this string str)
        {
            return Encoding.Unicode.GetBytes(str);
        }

        public static byte[] ToByte(this string str)
        {
            byte[] data = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(str.Substring(i, 1)))
                    continue;
                data[i] = byte.Parse(str.Substring(i, 1));
            }
            return data;
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToLower() + input.Substring(1);
            return str;
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToUpper() + input.Substring(1);
            return str;
        }

        #region 校验字符串

        /// <summary>
        /// 验证字符串是否是电话号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPhone(this string str)
        {
            if (str == null)
                return false;
            return Regex.IsMatch(str, @"^(?:\d{3,4}-?)?\d{7,8}$");
        }

        /// <summary>
        /// 验证字符串是否是手机号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsMobile(this string str)
        {
            if (str == null)
                return false;
            return Regex.IsMatch(str, @"^(1[3-9]\d)\d{8}$");
        }

        /// <summary>
        /// 验证字符串EMAIL地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmail(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            return Regex.IsMatch(str, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }

        /// <summary>
        /// 验证字符串URL地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUrl(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            return Regex.IsMatch(str, @"[a-zA-z]+://[^\s]*"); //^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$
        }

        /// <summary>
        /// 验证是否是身份证号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsIDCard(this string str)
        {
            var result = false;
            if (str.Length == 18)
            {
                result = Regex.IsMatch(str, @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X)$");
            }
            if (str.Length == 15)
            {
                result = Regex.IsMatch(str, @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$");
            }

            return result;
        }

        /// <summary>
        /// 验证是否是车牌号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsVehicleNumber(this string str)
        {
            bool result = false;
            if (str.Length == 7 || str.Length == 8)
            {
                //车牌正则
                //var reg = new Regex(@"^(([\u4e00-\u9fa5][a-zA-Z]|[\u4e00-\u9fa5]{2}\d{2}|[\u4e00-\u9fa5]{2}[a-zA-Z])[-]?|([wW][Jj][\u4e00-\u9fa5]{1}[-]?)|([a-zA-Z]{2}))([A-Za-z0-9]{5}|[DdFf][A-HJ-NP-Za-hj-np-z0-9][0-9]{4}|[0-9]{5}[DdFf])$", RegexOptions.Compiled);
                //  result = Regex.IsMatch(str, @"^(([\u4e00-\u9fa5][a-zA-Z]|[\u4e00-\u9fa5]{2}\d{2}|[\u4e00-\u9fa5]{2}[a-zA-Z])[-]?|([wW][Jj][\u4e00-\u9fa5]{1}[-]?)|([a-zA-Z]{2}))([A-Za-z0-9]{5}|[DdFf][A-HJ-NP-Za-hj-np-z0-9][0-9]{4}|[0-9]{5}[DdFf])$", RegexOptions.Compiled);
                result = Regex.IsMatch(str, @"^([京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}(([0-9]{5}[DF])|([DF]([A-HJ-NP-Z0-9])[0-9]{4})))|([京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-HJ-NP-Z0-9]{4}[A-HJ-NP-Z0-9挂学警港澳]{1})$", RegexOptions.Compiled);
            }
            return result;
        }

        public static bool IsZip(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            return Regex.IsMatch(str, @"^[1-9]\d{5}$");
        }

        public static bool IsNumber(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            return Regex.IsMatch(str, @"^[0-9]+$");
        }

        public static bool IsHanZi(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            return Regex.IsMatch(str, @"^[\u4e00-\u9fa5]+$");
        }

        /// <summary>
        /// 指示所指定的正则表达式是否使用指定的匹配选项在指定的输入字符串中找到了匹配项。
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <param name="regex">正则表达式</param>
        /// <param name="options">匹配选项</param>
        /// <returns></returns>
        public static bool IsMatch(this string str, string regex, RegexOptions options = RegexOptions.Singleline)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            return Regex.IsMatch(str, regex, options);
        }

        #endregion 校验字符串

        #region 单词转换

        /// <summary>
        /// 将单词转换为单数形式
        /// </summary>
        /// <param name="word">单词</param>
        /// <returns>单数形式的单词</returns>
        public static string Singular(this string word)
        {
            var result = ApplyRules(_singulars, word);
            return result;
        }

        /// <summary>
        /// 将单词转换为复数形式
        /// </summary>
        /// <param name="word">单词</param>
        /// <returns>复数形式的单词</returns>
        public static string Plural(this string word)
        {
            var result = ApplyRules(_plurals, word);
            return result;
        }

        /// <summary>
        /// 每个单词首字母大写
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string word)
        {
            return Regex.Replace(ToHumanCase(AddUnderscores(word)), @"\b([a-z])",
                delegate (Match match) { return match.Captures[0].Value.ToUpper(); });
        }

        /// <summary>
        /// 将单词转换首字母大写并替换下划线‘_’为空格
        /// </summary>
        /// <param name="lowercaseAndUnderscoredWord"></param>
        /// <returns></returns>
        public static string ToHumanCase(this string lowercaseAndUnderscoredWord)
        {
            return MakeInitialCaps(Regex.Replace(lowercaseAndUnderscoredWord, @"_", " "));
        }

        /// <summary>
        /// 下划线分割单词，同时空白和－替换为下划线
        /// </summary>
        /// <param name="pascalCasedWord"></param>
        /// <returns></returns>
        public static string AddUnderscores(this string pascalCasedWord)
        {
            return Regex.Replace(Regex.Replace(Regex.Replace(pascalCasedWord, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), @"([a-z\d])([A-Z])", "$1_$2"), @"[-\s]", "_").ToLower();
        }

        /// <summary>
        /// 将单词转换为首字母大写形式
        /// </summary>
        /// <param name="word">单词</param>
        /// <returns></returns>
        public static string MakeInitialCaps(this string word)
        {
            return String.Concat(word.Substring(0, 1).ToUpper(), word.Substring(1).ToLower());
        }

        /// <summary>
        /// 将单词转换为首字母小写形式
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string MakeInitialLowerCase(this string word)
        {
            return String.Concat(word.Substring(0, 1).ToLower(), word.Substring(1));
        }

        #region Rules

        private static string ApplyRules(IList<InflectorRule> rules, string word)
        {
            string result = word;
            if (!_uncountables.Contains(word.ToLower()))
            {
                for (int i = rules.Count - 1; i >= 0; i--)
                {
                    string currentPass = rules[i].Apply(word);
                    if (currentPass != null)
                    {
                        result = currentPass;
                        break;
                    }
                }
            }
            return result;
        }

        private static readonly List<InflectorRule> _plurals = new List<InflectorRule>();
        private static readonly List<InflectorRule> _singulars = new List<InflectorRule>();
        private static readonly List<string> _uncountables = new List<string>();

        private static void AddRule()
        {
            AddPluralRule("$", "s");
            AddPluralRule("s$", "s");
            AddPluralRule("(ax|test)is$", "$1es");
            AddPluralRule("(octop|vir)us$", "$1i");
            AddPluralRule("(alias|status)$", "$1es");
            AddPluralRule("(bu)s$", "$1ses");
            AddPluralRule("(buffal|tomat)o$", "$1oes");
            AddPluralRule("([ti])um$", "$1a");
            AddPluralRule("sis$", "ses");
            AddPluralRule("(?:([^f])fe|([lr])f)$", "$1$2ves");
            AddPluralRule("(hive)$", "$1s");
            AddPluralRule("([^aeiouy]|qu)y$", "$1ies");
            AddPluralRule("(x|ch|ss|sh)$", "$1es");
            AddPluralRule("(matr|vert|ind)ix|ex$", "$1ices");
            AddPluralRule("([m|l])ouse$", "$1ice");
            AddPluralRule("^(ox)$", "$1en");
            AddPluralRule("(quiz)$", "$1zes");

            AddSingularRule("s$", String.Empty);
            AddSingularRule("ss$", "ss");
            AddSingularRule("(n)ews$", "$1ews");
            AddSingularRule("([ti])a$", "$1um");
            AddSingularRule("((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$", "$1$2sis");
            AddSingularRule("(^analy)ses$", "$1sis");
            AddSingularRule("([^f])ves$", "$1fe");
            AddSingularRule("(hive)s$", "$1");
            AddSingularRule("(tive)s$", "$1");
            AddSingularRule("([lr])ves$", "$1f");
            AddSingularRule("([^aeiouy]|qu)ies$", "$1y");
            AddSingularRule("(s)eries$", "$1eries");
            AddSingularRule("(m)ovies$", "$1ovie");
            AddSingularRule("(x|ch|ss|sh)es$", "$1");
            AddSingularRule("([m|l])ice$", "$1ouse");
            AddSingularRule("(bus)es$", "$1");
            AddSingularRule("(o)es$", "$1");
            AddSingularRule("(shoe)s$", "$1");
            AddSingularRule("(cris|ax|test)es$", "$1is");
            AddSingularRule("(octop|vir)i$", "$1us");
            AddSingularRule("(alias|status)$", "$1");
            AddSingularRule("(alias|status)es$", "$1");
            AddSingularRule("^(ox)en", "$1");
            AddSingularRule("(vert|ind)ices$", "$1ex");
            AddSingularRule("(matr)ices$", "$1ix");
            AddSingularRule("(quiz)zes$", "$1");

            AddIrregularRule("person", "people");
            AddIrregularRule("man", "men");
            AddIrregularRule("child", "children");
            AddIrregularRule("sex", "sexes");
            AddIrregularRule("tax", "taxes");
            AddIrregularRule("move", "moves");

            AddUnknownCountRule("equipment");
            AddUnknownCountRule("information");
            AddUnknownCountRule("rice");
            AddUnknownCountRule("money");
            AddUnknownCountRule("species");
            AddUnknownCountRule("series");
            AddUnknownCountRule("fish");
            AddUnknownCountRule("sheep");
        }

        private static void AddIrregularRule(string singular, string plural)
        {
            AddPluralRule(String.Concat("(", singular[0], ")", singular.Substring(1), "$"), String.Concat("$1", plural.Substring(1)));
            AddSingularRule(String.Concat("(", plural[0], ")", plural.Substring(1), "$"), String.Concat("$1", singular.Substring(1)));
        }

        private static void AddUnknownCountRule(string word)
        {
            _uncountables.Add(word.ToLower());
        }

        private static void AddPluralRule(string rule, string replacement)
        {
            _plurals.Add(new InflectorRule(rule, replacement));
        }

        private static void AddSingularRule(string rule, string replacement)
        {
            _singulars.Add(new InflectorRule(rule, replacement));
        }

        private class InflectorRule
        {
            public readonly Regex regex;
            public readonly string replacement;

            public InflectorRule(string regexPattern, string replacementText)
            {
                regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
                replacement = replacementText;
            }

            public string Apply(string word)
            {
                if (!regex.IsMatch(word))
                    return null;

                string replace = regex.Replace(word, replacement);
                if (word == word.ToUpper())
                    replace = replace.ToUpper();

                return replace;
            }
        }

        #endregion Rules

        #endregion 单词转换

        #endregion string 扩展方法

        #region StringBuilder 扩展方法

        /// <summary>
        /// 删除结尾的指定字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strchar"></param>
        public static void TrimEnd(this StringBuilder str, string strchar)
        {
            int n = str.ToString().LastIndexOf(strchar);
            if (n > 0)
            {
                str.Remove(n, str.Length - n);
            }
        }

        /// <summary>
        /// 删除结尾的“，”号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static StringBuilder TrimEndComma(this StringBuilder str)
        {
            int n = str.ToString().Trim().TrimEnd(new char[] { ',' }).Length;
            if ((n >= 0) && (n < str.Length))
            {
                str.Remove(n, str.Length - n);
            }
            return str;
        }

        #endregion StringBuilder 扩展方法

        #region Type 扩展方法

        /// <summary>
        /// 验证类型是否是整数类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsIntegralType(this Type t)
        {
            var tc = Type.GetTypeCode(t);
            return tc >= TypeCode.SByte && tc <= TypeCode.UInt64;
        }

        /// <summary>
        /// 验证类型是否是数字类型，包括整型和浮点型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNumberType(this Type t)
        {
            var tc = Type.GetTypeCode(t);
            return tc >= TypeCode.SByte && tc <= TypeCode.Decimal;
        }

        #endregion Type 扩展方法

        #region Page  扩展方法

        //public static void RegisterJS(this Page page, string key, string script, params object[] args)
        //{
        //    if (args != null && args.Length > 0)
        //    {
        //        page.ClientScript.RegisterStartupScript(page.GetType(), key, string.Format(script, args), true);
        //    }
        //    else
        //    {
        //        page.ClientScript.RegisterStartupScript(page.GetType(), key, script, true);
        //    }
        //}

        #endregion Page  扩展方法

        #region Array 扩展方法

        /// <summary>
        /// 用字符串分隔数组
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string JoinToSql(this Array arr)
        {
            var isnumber = arr.GetType().GetElementType().IsNumberType();
            var str = new StringBuilder();
            foreach (var item in arr)
            {
                if (item != null)
                {
                    if (isnumber)
                    {
                        str.AppendFormat("{0},", item);
                    }
                    else
                    {
                        str.AppendFormat("'{0}',", item.ToString().Replace("'", "''"));
                    }
                }
            }
            return str.TrimEndComma().ToString();
        }

        public static string JoinToSql(this List<string> list)
        {
            return list.ToArray().JoinToSql();
        }

        #endregion Array 扩展方法

        #region List<T> 扩展方法

        /// <summary>
        /// 用字符串分隔LIST
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="separator">要用作分隔符的字符串。</param>
        /// <returns></returns>
        public static string Join<T>(this List<T> arr, string separator = ",")
        {
            return string.Join(separator, arr);
        }

        /// <summary>
        /// 通过 TKey , TValue 增加 KeyValuePair 到列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Add<TKey, TValue>(this IList<KeyValuePair<TKey, TValue>> list, TKey key, TValue value)
        {
            list.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        #endregion List<T> 扩展方法

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="length">执行次数</param>
        /// <param name="action">执行方法</param>
        /// <returns>当前实例测量得出的总运行时间。</returns>
        public static TimeSpan Time(this Stopwatch sw, int length, Action action)
        {
            sw.Restart();
            for (int i = 0; i < length; i++)
            {
                action();
            }
            sw.Stop();
            return sw.Elapsed;
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TimeSpan Time(this Stopwatch sw, int length, Action<int> action)
        {
            sw.Restart();
            for (int i = 0; i < length; i++)
            {
                action(i);
            }
            sw.Stop();
            return sw.Elapsed;
        }

        /// <summary>
        /// 将对象转换批指定的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="convertibleValue"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this IConvertible convertibleValue)
        {
            if (null == convertibleValue)
            {
                return default;
            }

            if (!typeof(T).IsGenericType)
            {
                return (T)Convert.ChangeType(convertibleValue, typeof(T));
            }
            else
            {
                Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return (T)Convert.ChangeType(convertibleValue, Nullable.GetUnderlyingType(typeof(T)));
                }
            }
            throw new InvalidCastException(string.Format("从类型 \"{0}\" 转换到类型 \"{1}\" 无效.",
                convertibleValue.GetType().FullName, typeof(T).FullName));
        }

        public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector)
        {
            return from a in outer
                   join b in inner on outerKeySelector(a) equals innerKeySelector(b) into c
                   from b in c.DefaultIfEmpty()
                   select resultSelector(a, b);
        }

        /// <summary>
        /// 将byte[]转为String
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToStringUTF8(this byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// 获取枚举类型的DescriptionAttribute(扩展方法)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum obj)
        {
            Type _enumType = obj.GetType();
            FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, obj));
            DescriptionAttribute dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
            if (dna != null && !string.IsNullOrEmpty(dna.Description))
                return dna.Description;
            return obj.ToString();
        }

        /// <summary>
        /// 获取枚举类型的display扩展方法)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDisplay(this Enum obj)
        {
            var type = obj.GetType();//先获取这个枚举的类型
            var field = type.GetField(obj.ToString());//通过这个类型获取到值
            var re = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute));//得到特性
            return re.Name ?? "";
        }

        public static string ToNumber(this string obj)
        {
            if (obj == null)
                return string.Empty;
            return Regex.Replace(obj, @"[^\d\r\n]*", "");
        }

        #region EF扩展

        /// <summary>
        /// EF分页
        /// </summary>
        /// <typeparam name="TEntiy"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecord"></param>
        /// <param name="ascending">默认升序</param>
        /// <returns></returns>
        public static IQueryable<TEntiy> Paging<TEntiy, TKey>(this IQueryable<TEntiy> query, Expression<Func<TEntiy, TKey>> orderBy, int pageIndex, int pageSize, out int totalRecord, bool ascending = true)
        {
            totalRecord = query.Count();
            if (ascending)
                query = query.OrderBy(orderBy);
            else
                query = query.OrderByDescending(orderBy);
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        #endregion EF扩展

        #region Linq扩展方法

        /// <summary>
        /// 根据字段去重
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型</typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// 是否含有重复项
        /// <para>内部使用DistinctBy方法进行判断</para>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static bool HasRepeatBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var originCount = source.Count();
            var distinctCount = source.DistinctBy(keySelector).Count();
            return originCount != distinctCount;
        }

        #endregion Linq扩展方法

        #region decimal扩展方法

        /// <summary>
        /// 保留N位小数，如果小数部位都是0则取整数，默认保留2位小数（不进行四舍五入）
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="fixedNumber"></param>
        /// <returns></returns>
        public static decimal GetFixedDigit(this decimal? dm, int fixedNumber = 2)
        {
            if (dm.HasValue)
            {
                string strDecimal = dm.ToString();
                int index = strDecimal.IndexOf(".");
                //有小数点
                if (index > -1)
                {
                    //小数点右边位数大于要保留的位数
                    if (strDecimal.Length - index + 1 >= fixedNumber)
                    {
                        int Count = strDecimal.Substring(index + 1, fixedNumber).Count(s => s == '0');
                        //末尾都为0
                        if (Count == fixedNumber)
                        {
                            //保留整数位
                            strDecimal = strDecimal.Substring(0, index);
                        }
                        else
                        {
                            int length = index;
                            if (fixedNumber != 0)
                            {
                                length = index + fixedNumber + 1;
                            }
                            //保留需要的位数
                            strDecimal = strDecimal.Substring(0, length);
                        }
                    }
                }
                return Decimal.Parse(strDecimal);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 保留N位小数，如果小数部位都是0则取整数，默认保留2位小数（不进行四舍五入）
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="fixedNumber"></param>
        /// <returns></returns>
        public static decimal GetFixedDigit(this decimal dm, int fixedNumber = 2)
        {
            string strDecimal = dm.ToString();
            int index = strDecimal.IndexOf(".");
            //有小数点
            if (index > -1)
            {
                //小数点右边位数大于要保留的位数
                if (strDecimal.Length - index + 1 >= fixedNumber)
                {
                    int Count = strDecimal.Substring(index + 1, fixedNumber).Count(s => s == '0');
                    //末尾都为0
                    if (Count == fixedNumber)
                    {
                        //保留整数位
                        strDecimal = strDecimal.Substring(0, index);
                    }
                    else
                    {
                        int length = index;
                        if (fixedNumber != 0)
                        {
                            length = index + fixedNumber + 1;
                        }
                        //保留需要的位数
                        strDecimal = strDecimal.Substring(0, length);
                    }
                }
            }
            return Decimal.Parse(strDecimal);
        }

        #endregion decimal扩展方法

        #region Dictionary

        /// <summary>
        /// 填充表单信息的Stream
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="stream"></param>
        public static void FillFormDataStream(this Dictionary<string, string> formData, Stream stream)
        {
            string dataString = GetQueryString(formData);
            var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
            stream.Write(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        }

        /// <summary> 组装QueryString的方法 参数之间用&连接，首位没有符号，如：a=1&b=2&c=3 </summary> <param
        /// name="formData"></param> <returns></returns>
        public static string GetQueryString(this Dictionary<string, string> formData)
        {
            if (formData == null || formData.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            var i = 0;
            foreach (var kv in formData)
            {
                i++;
                sb.AppendFormat("{0}={1}", kv.Key, kv.Value);
                if (i < formData.Count)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        #endregion Dictionary

        #region image扩展方法

        /// <summary>
        /// 将Image转为byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format">默认获取Image的RawFormat 出错时可以尝试指定此参数</param>
        /// <returns></returns>
        public static byte[] ToByte(this Image image, ImageFormat format = null)
        {
            if (image == null)
                return null;

            var stream = new MemoryStream();

            if (format == null)
                image.Save(stream, image.RawFormat);
            else
                image.Save(stream, ImageFormat.Jpeg);

            var bytes = stream.ToArray();
            stream.Close();
            return bytes;
        }

        /// <summary>
        /// 将byte[]转为Image
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image ToImage(this byte[] bytes)
        {
            if (bytes.ToByte().Length < 1)
                return null;
            var stream = new MemoryStream(bytes, 0, bytes.Length);
            var image = Image.FromStream(stream, true);
            stream.Close();
            return image;
        }

        public static Bitmap ToImageFromBase64(this string base64string)
        {
            byte[] b = Convert.FromBase64String(base64string);
            MemoryStream ms = new MemoryStream(b);
            Bitmap bitmap = new Bitmap(ms);
            return bitmap;
        }

        public static string ToBase64FromImage(this Image imagefile)
        {
            string strbaser64;
            try
            {
                Bitmap bmp = new Bitmap(imagefile);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                strbaser64 = Convert.ToBase64String(arr);
            }
            catch (Exception)
            {
                throw new Exception("Something wrong during convert!");
            }
            return strbaser64;
        }

        #endregion image扩展方法
    }

    /// <summary>
    /// 适用于[a-z][0-9]的字母与数字组合数字形式大小比较
    /// <para>如：A0, B15</para>
    /// </summary>
    public class CharNumComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            char lChar = x.First(),
                 rChar = y.First();
            if (lChar.CompareTo(rChar) != 0)
                return lChar.CompareTo(rChar);
            else
            {
                var lNum = int.Parse(x.Substring(1));
                var rNum = int.Parse(y.Substring(1));

                if (lNum.CompareTo(rNum) != 0)
                    return lNum.CompareTo(rNum);
                else
                    return 0;
            }
        }
    }
}