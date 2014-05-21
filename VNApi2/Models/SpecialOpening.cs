using System;
using System.Collections.Generic;

namespace VNApi2.Models
{
    public class SpecialOpening
    {
        public String FromDate { get; set; }
        public String ToDate { get; set; }
        public String FromTime { get; set; }
        public String ToTime { get; set; }

        public List<string> Weekday = new List<string>();

    }
}