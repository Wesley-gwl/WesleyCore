namespace WesleyUntity
{
    /// <summary>
    /// 生成导入模板的模型类
    /// </summary>
    public class ExcleTempRow
    {
        public string Title { get; set; }
        public bool Require { get; set; }

        /// <summary>
        /// 单元格的验证数据
        /// </summary>
        public string[] ValidationData { get; set; }

        /// <summary>
        /// 排序号 默认升序
        /// </summary>
        public int Rank { get; set; }

        public ExcleTempRow()
        {
        }

        public ExcleTempRow(string title, string[] vd = null, int rank = 0, bool isReq = false)
        {
            Title = title;
            if (vd != null)
                ValidationData = vd;
            Rank = rank;
            Require = isReq;
        }
    }
}