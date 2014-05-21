using System.Collections.Generic;

namespace VNApi2.Models
{
    public class Facility
    {
        public string Name { get; set; }
    }

    public class SubFacility : Facility
    {
        public string Value { get; set; }
    }

    public class FacilityCategory : Facility
    {
        public List<SubFacility> SubFacilities { get; set; } 
    }

}