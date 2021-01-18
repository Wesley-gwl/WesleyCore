using System;

namespace WesleyCore.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class DecimalPrecisionAttribute : Attribute
    {
        /// <summary>
        /// <para>自定义Decimal类型的精确度属性</para>
        /// </summary>
        /// <param name="precision">precision
        /// <para>精度（默认18）</para></param>
        /// <param name="scale">scale
        /// <para>小数位数（默认2）</para></param>
        public DecimalPrecisionAttribute(byte precision = 18, byte scale = 2)
        {
            Precision = precision;
            Scale = scale;
        }

        /// <summary>
        /// 精确度（默认18）
        /// </summary>
        public byte Precision { get; set; } = 18;

        /// <summary>
        /// 保留位数（默认2）
        /// </summary>
        public byte Scale { get; set; } = 2;
    }
}