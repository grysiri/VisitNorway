using System.Collections.ObjectModel;
using System.Linq;
using DomainModels.Domain.Enums;

namespace DomainModels.Domain
{
    //The owner of the product, in tellus called Customer, in cbis ProductOwner
    public class Owner : DomainBase
    {
        public int ExternalId { get; set; }
        public ExternalProvider Provider { get; set; }
        public string Name { get; set; }        
        public virtual Collection<Product> Products { get; set; }

        public override string ToString()
        {
            var s = "\n" + Name;
            if (Products.Any())
                s += "\nAntall produkter: " + Products.Count;            
            return s;
        }
    }
}
