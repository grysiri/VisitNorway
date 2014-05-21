using System;
using System.Collections.Generic;
using DomainModels.Domain.Enums;

namespace DomainModels.Domain
{
    //used when a product has standard opening times without specific dates
    public class OpeningTime : DomainBase
    {
        
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public virtual List<Weekdays> Weekday { get; set; }
        public Boolean Monday { get; set; }
        public Boolean Tuesday { get; set; }
        public Boolean Wednesday { get; set; }
        public Boolean Thursday { get; set; }
        public Boolean Friday { get; set; }
        public Boolean Saturday { get; set; }
        public Boolean Sunday { get; set; }

        public OpeningTime()
        {
            Monday = false;
            Tuesday = false;
            Wednesday = false;
            Thursday = false;
            Friday = false;
            Saturday = false;
            Sunday = false;
        }

        public override string ToString()
        {
            var s = "";
            foreach (var day in Weekday)
            {
                s += day + ": " + FromTime + " - " + ToTime+ "\n";
            }
            return s;
        }
    }
}