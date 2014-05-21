using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Domain
{
    public class Postalarea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]        
        public string PostalareaId { get; set; }
        public Municipality Municipality { get; set; }
        public string PostalareaName { get; set; }
    }
}
