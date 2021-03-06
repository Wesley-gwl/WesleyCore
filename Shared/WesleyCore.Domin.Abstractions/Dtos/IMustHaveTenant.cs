﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WesleyCore.Domin.Abstractions
{
    /// <summary>
    /// 租户
    /// </summary>
    public interface IMustHaveTenant
    {
        public int TenantId { get; set; }
    }
}