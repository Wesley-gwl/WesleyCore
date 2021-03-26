using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

namespace WesleyUntity
{
    /// <summary>
    /// 常用工具类
    /// </summary>
    [Serializable]
    public static class ComFunc
    {
        /// <summary>
        /// 获取6位随机验证码
        /// </summary>
        /// <returns></returns>
        public static int GetRandom()
        {
            var random = new Random(unchecked((int)(DateTime.Now.Ticks)));
            //睡眠1毫秒，让随机种子不一致   (多线程web应用里未必起效)
            //System.Threading.Thread.Sleep(1);
            return random.Next(100000, 999999);
        }

        /// <summary>
        /// 去除对象中string类型属性的空格
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T TrimStringValue<T>(T t) where T : new()
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            Parallel.ForEach(props, p =>
            {
                if (p.PropertyType.Name == "String")
                {
                    var tmp = (string)p.GetValue(t, null);
                    if (!string.IsNullOrEmpty(tmp))
                        p.SetValue(t, tmp.Trim(), null);
                }
            });
            return t;
        }

        ///<summary>
        /// 返回 GUID 用于数据库操作，特定的时间代码可以提高检索效率
        /// </summary>
        /// <returns>COMB (GUID 与时间混合型) 类型 GUID 数据</returns>
        public static Guid NewCombGuid()
        {
            byte[] guidArray = System.Guid.NewGuid().ToByteArray();
            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            // Get the days and milliseconds which will be used to build the byte string 
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));
            // Convert to a byte array 
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));
            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);
            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);
            return new System.Guid(guidArray);
        }

        /// <summary>
        /// 通过序列化的方式，深拷贝一个对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="t">对象</param>
        /// <returns></returns>
        public static T Copy<T>(T t)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(t);
            T copy = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return copy;
        }

        /// <summary>
        /// DataTable转换成泛型列表 2019.5.23 added by yz
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt)
        {
            var lst = new List<T>();
            var plist = new List<System.Reflection.PropertyInfo>(typeof(T).GetProperties());
            foreach (DataRow item in dt.Rows)
            {
                T t = System.Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        if (!Convert.IsDBNull(item[i]))
                        {
                            info.SetValue(t, item[i], null);
                        }
                    }
                }
                lst.Add(t);
            }
            return lst;
        }
    }
}