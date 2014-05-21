using System;
using System.Collections.ObjectModel;
using System.Linq;
using DomainModels.Domain.Enums;

namespace DomainModels.Domain
{
    //Language-independent info about a product
    public class Product : DomainBase
    {
        public string StreetAddress1 { get; set; }
        public string PostalCode { get; set; }        
        public virtual Zipcodes Zipcode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
         
        public virtual Owner Owner { get; set; }
        public ExternalProvider ExternalProvider { get; set; } //tellus, cbis etc
        public int ExternalId { get; set; }
        public virtual LocalOrg LocalOrg { get; set; }
        public virtual Collection<ProductInfo> ProductInfos { get; set; }

        public virtual Collection<OpeningTime> OpeningTimes { get; set; } //standard opening hours
        public virtual Collection<SpecialOpening> SpecialOpenings { get; set; } //date-specific opening hours

        public virtual Collection<Category> Categories { get; set; }
        public virtual Collection<Thirdparty> Thirdparties { get; set; } //external, like social media sites 

        public override string ToString()
        {
            string s = "External Id: " + ExternalId +
                       "\nExternal Provider: " + ExternalProvider +
                       "\nAddress: " + StreetAddress1 + 
                       "\nPostal code: " + PostalCode +
                       "\nWebsite: " + Website +
                       "\nPhone: " + Phone +
                       "\nEmail: " + Email +
                       "\n\nLatitude: " + Latitude + " Longitude: " + Longitude +
                       "\nCreated: " + CreatedDate + " Modified: " + ModifiedDate +
                       "\n";
            if (LocalOrg != null)
                s += "\nLocal org: " + LocalOrg;

            if (Owner != null)
                s += "\nOwner: " + Owner;


            if (OpeningTimes != null && OpeningTimes.Any())
            {
                s += "\n\nVanlige åpningstider:\n";
                foreach (var time in OpeningTimes)
                {
                    s += time;
                }
            }

            if (SpecialOpenings != null && SpecialOpenings.Any())
            {
                s += "\nOpen: ";
                foreach (var opening in SpecialOpenings)
                {
                    s += opening;
                }
            }

            if (Categories.Any())
            {
                s += "\nCategories: ";
                foreach (var cat in Categories)
                {
                    s += "\n " + cat;
                }
            }

            if (Thirdparties.Any())
            {
                s += "\nThirdparty: ";
                foreach (var t in Thirdparties)
                {
                    s += "\n " + t;
                }
            }


            s += "\n\n  **\n\n";
            return s;
        }
    }
}