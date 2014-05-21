using System.Collections.Generic;

namespace VNApi2.Models
{
    public class StandardOpeningTime
    {
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public List<string> Weekday = new List<string>();
    }
}