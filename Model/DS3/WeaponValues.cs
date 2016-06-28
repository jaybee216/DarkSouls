using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DS3
{
    public class WeaponValues
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeaponValuesId { get; set; }
        public int WeaponId { get; set; }
        public int InfusionId { get; set; }
        public float Bleed { get; set; }
        public float Poison { get; set; }
        public float Frost { get; set; }
        public int Physical { get; set; }
        public int Magic { get; set; }
        public int Fire { get; set; }
        public int Lightning { get; set; }
        public int Dark { get; set; }
        public float Str { get; set; }
        public float Dex { get; set; }
        public float Int { get; set; }
        public float Fth { get; set; }
        public float Lck { get; set; }

        [ForeignKey("WeaponId")]
        public Weapon Weapon { get; set; }

        [ForeignKey("InfusionId")]
        public InfusionType Infusion { get; set; }
    }
}
