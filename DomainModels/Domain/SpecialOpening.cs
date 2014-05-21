using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels.Domain.Enums;

namespace DomainModels.Domain
{
    //date-specific opening hours
    public class SpecialOpening : DomainBase
    {

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromTime { get; set; }
        public DateTime? ToTime { get; set; }
        public virtual List<Weekdays> Weekday { get; set; }
        public Weekdays[] Weekdayarray;        

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        public SpecialOpening()
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
            const string norskformat = "dd-MM-yy";
            var s = "";
            if (FromDate != null)
                s += "\n  Fra "+((DateTime)FromDate).ToString(norskformat);
            if (ToDate != null)
                s += "\n  Til "+((DateTime)ToDate).ToString(norskformat);
            if (FromTime != null)
                s += "\n\t Kl " + ((DateTime) FromTime).ToString("HH:mm");
            if (ToTime != null)
                s += " - " + ((DateTime)ToTime).ToString("HH:mm");
            if (Weekday.Any())
            {
                s += "\n  Dager: ";
                s = Weekday.Aggregate(s, (current, days) => current + (days + " "));
            }
            return s+"\n";
        }
    }
}