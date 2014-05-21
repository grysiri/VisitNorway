using DomainModels.Domain.Enums;

namespace DomainModels.Domain
{
    public class Thirdparty : DomainBase
    {
        public int ExternalId { get; set; }
        public ExternalProvider Provider { get; set; }
        public string Uri { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            var s = Name;
            if (Uri != null && !Uri.Equals(""))
                s += " - " + Uri;
            return s;
        }
    }
}