﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WesleyCore.Domin.Abstractions
{
    public class PagedInputDto
    {
        /// <summary>
        /// 页码默认1
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 页数默认50
        /// </summary>
        public int Rows { get; set; } = 50;

        /// <summary>
        /// 页数，默认1
        /// </summary>
        public int MaxResultCount { get { return Rows; } set { } }

        /// <summary>
        ///
        /// </summary>
        private int _skipCount;

        /// <summary>
        /// 数量 默认0
        /// </summary>
        public int SkipCount
        {
            get
            {
                if (_skipCount == 0)
                    return (Page - 1) * Rows;
                else
                    return _skipCount;
            }
            set { _skipCount = value; }
        }
    }
}