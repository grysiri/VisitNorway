using System.Collections.ObjectModel;
using System.Linq;
using DomainModels.Domain.Enums;

namespace DomainModels.Domain
{
    public class Media : DomainBase
    {        
        public MediaType MediaType { get; set; } //enum 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public string EmbeddedUri { get; set; }
        public virtual Collection<MediaInstance> Instances { get; set; }


        public override string ToString()
        {
            var s = "\nName: " + Name + "\nType: "+ MediaType;
            if (Description != null && !Description.Equals(""))
                s += "\nDesc: " + Description;
            if (Copyright != null && !Copyright.Equals(""))
                s += "\nCopyright: " + Copyright;
            if (EmbeddedUri != null && !EmbeddedUri.Equals(""))
                s += "\nEmbedded Uri: " + EmbeddedUri;
            if (Instances.Any())
                s = Instances.Aggregate(s, (current, inst) => current + inst);

            return s;
        }

       

        public string TypeEnumToString()
        {
            switch (MediaType)
            {
                case MediaType.Image:
                    return "Image";
                case MediaType.Sound:
                    return "Sound";
                case MediaType.Video:
                    return "Video";
                case MediaType.Vimeo:
                    return "Vimeo";
                case MediaType.Youtube:
                    return "Youtube";
                default:
                    return "Other";
            }
        }
    }
}