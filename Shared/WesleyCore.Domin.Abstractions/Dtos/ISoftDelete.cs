﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WesleyCore.Domin.Abstractions
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}