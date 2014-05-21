using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Domain
{
    public class County
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]                
        public string CountyId { get; set; }
        public string CountyName { get; set; }
    }
}
