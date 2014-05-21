using DomainModels.Domain.Enums;

namespace DomainModels.Domain
{
    //The Local Organisation which the product belongs to
    //like VisitOslo or OpplevLarvik
    public class LocalOrg : DomainBase 
    {
        public string Name { get; set; }
        public int ExternalId { get; set; }
        public string Website { get; set; }
        public ExternalProvider Provider { get; set; }

        public override string ToString()
        {
            var s = "\n" + Name;
            if (Website != null && !Website.Equals(""))
                s += "\nWebside: " + Website;
            return s;
        }
    }
}
