using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DarkSoulsII.Models
{
    public class Infusion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InfusionId { get; set; }
        public string Name { get; set; }
        public int? DisplayOrder { get; set; }
    }
}