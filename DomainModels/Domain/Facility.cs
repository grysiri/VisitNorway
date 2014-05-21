using System.Collections.Generic;
using System.Linq;

namespace DomainModels.Domain
{
    public class Facility : DomainBase
    {
        public string FacilityName { get; set; } 
        public string Value { get; set; } //if facility has "comment" or something measurable

        public override string ToString()
        {
            var s = FacilityName;
            if (Value != null && !Value.Equals(""))
                s += ": " + Value;
            return s;
        }
    }

    //"Head" facility with list of other facilities
    public class FacilityCategory : Facility
    {
        public virtual List<Facility> List { get; set; }

        public override string ToString()
        {
            var s = base.ToString();
            return List.Aggregate(s, (current, f) => current + ("\n\t" + f.ToString()));
        }
    }
}