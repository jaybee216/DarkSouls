using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DarkSoulsII.Models
{
    public class Weapon
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeaponId { get; set; }
        public int WeaponCategoryId { get; set; }
        public string Name { get; set; }
        public int? Counter { get; set; }
        public int? PoiseDamage { get; set; }
        public decimal? Range { get; set; }
        public int? CastingSpeed { get; set; }
        public decimal? Stability { get; set; }
        public int? Durability { get; set; }
        public decimal? Weight { get; set; }
        //public decimal PhysicalReduction { get; set; }
        //public decimal MagicReduction { get; set; }
        //public decimal FireReduction { get; set; }
        //public decimal LightningReduction { get; set; }
        //public decimal DarkReduction { get; set; }
        //public decimal PoisionReduction { get; set; }
        //public decimal BleedReduction { get; set; }
        //public decimal PetrifyReduction { get; set; }
        //public decimal CurseReduction { get; set; }

        [ForeignKey("WeaponCategoryId")]
        public WeaponCategory Category { get; set; }
    }
}