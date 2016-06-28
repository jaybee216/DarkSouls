using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DS3
{
    public class InfusionType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InfusionId { get; set; }
        public string Name { get; set; }
    }
}
