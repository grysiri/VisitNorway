using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Domain
{
    public class Municipality
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]                
        public string MunicipalityId { get; set; }
        public County County { get; set; }
        public string MunicipalityName { get; set; }
    }
}
