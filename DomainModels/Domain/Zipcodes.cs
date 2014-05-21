using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Domain
{
    public class Zipcodes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]        
        public string Zipcode { get; set; }
        public Postalarea Postalarea { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
    }
}
