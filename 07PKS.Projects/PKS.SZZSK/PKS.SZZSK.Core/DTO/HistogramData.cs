﻿using System;
using System.Collections.Generic;

namespace PKS.SZZSK.Core.DTO
{
    public class HistogramData
    {
        public HistogramSetting Setting { get; set; }
        public List<HistogramColumn> Columns { get; set; }
        public Array Rows { get; set; }
    }
}
