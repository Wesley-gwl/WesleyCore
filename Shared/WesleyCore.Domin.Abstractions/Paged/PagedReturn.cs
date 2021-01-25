using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WesleyCore.Domin.Abstractions
{
    /// <summary>
    /// 分页结果
    /// </summary>
    public class PagedReturn<T>
    {
        /// <summary>
        /// 构造
        /// </summary>
        public PagedReturn()
        {
            this.Total = 0;
            this.Rows = new List<T>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="count"></param>
        /// <param name="rows"></param>
        public PagedReturn(int count, List<T> rows)
        {
            this.Total = count;
            if (rows == null || rows.Count == 0)
            {
                this.Rows = new List<T>();
            }
            else
            {
                this.Rows = rows;
            }
        }

        /// <summary>
        /// 总行数
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        /// <summary>
        /// 当前页展示的数据
        /// </summary>
        [JsonProperty(PropertyName = "rows")]
        public List<T> Rows { get; set; }
    }
}