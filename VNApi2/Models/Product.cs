using System.Collections.Generic;

namespace VNApi2.Models
{
    public class Product
    {
        
        public int Id { get; set; }

        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string PostalArea { get; set; }
        public string Municipality { get; set; }
        public string County { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Transportation { get; set; }

        public string LocalOrgName { get; set; }
        public string LocalOrgWebsite { get; set; }
        public string OwnerName { get; set; }

        public List<Media> Mediae { get; set; } 
        public List<StandardOpeningTime> StandardOpeningTimes { get; set; } 
        public List<SpecialOpening> SpecialOpenings { get; set; } 
        public List<Thirdparty> Thirdparties { get; set; }
        public List<FacilityCategory> Facilities { get; set; }
        public List<Category> Categories { get; set; } 
    }
}