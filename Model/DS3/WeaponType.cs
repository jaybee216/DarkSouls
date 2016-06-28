using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DS3
{
    public class WeaponType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeaponTypeId { get; set; }
        public string Name { get; set; }
    }
}
