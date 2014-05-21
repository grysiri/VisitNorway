using System.Collections.ObjectModel;
using DomainModels.Domain.Enums;

namespace DomainModels.Domain
{
    //Language-dependent info about a product
    public class ProductInfo : DomainBase 
    {
        public virtual Product Product { get; set; }
        public LanguageCode Language { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Transportation { get; set; }

        public virtual Collection<Media> Medias { get; set; }
        public virtual Collection<FacilityCategory> Facilities { get; set; }        
    }
}