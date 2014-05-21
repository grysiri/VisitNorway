using DomainModels.Domain.Enums;

namespace DomainModels.Domain

{
    public class MediaInstance : DomainBase
    {
        public MediaSize Size { get; set; }
        public int Width { get; set; }//in px
        public int Height { get; set; }//in px  
        public string Uri { get; set; }



        public override string ToString()
        {
            var s = "\n\tStr: " + Size;
            if (Width > 0 && Height > 0)
                s += " " + Height + " " + Width;
            if (Uri != null && !Uri.Equals(""))
                s += "\n\tURL: " + Uri;
            return s;
        }
    }


}
