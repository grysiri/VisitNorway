using System.Collections.Generic;

namespace VNApi2.Models
{
    public class Media
    {
        public string MediaType { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public string Copyright { get; set; }
        public string EmbeddedUri { get; set; }
        public virtual List<MediaInstance> Instances { get; set; }
    }

    public class MediaInstance
    {
        public string Size { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Uri { get; set; }
    }

  
}