using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DarkSoulsII.Models
{
    public class WeaponCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeaponCategoryId { get; set; }
        public string Name { get; set; }
    }
}