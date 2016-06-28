using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DS2
{
    public class WeaponCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeaponCategoryId { get; set; }
        public string Name { get; set; }
    }
}