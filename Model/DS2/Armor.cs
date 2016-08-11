using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DS2
{
    public class DS2Armor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArmorId { get; set; }
        public string Name { get; set; }
        public int ArmorTypeId { get; set; }
        public string Phys { get; set; }
        public int? Physical { get; set; }
        public int? Strike { get; set; }
        public int? Slash { get; set; }
        public int? Thrust { get; set; }
        public int Magic { get; set; }
        public int Fire { get; set; }
        public int Lightning { get; set; }
        public int Dark { get; set; }
        public int Poise { get; set; }
        public int Poison { get; set; }
        public int Bleed { get; set; }
        public int Petrification { get; set; }
        public int Curse { get; set; }
        public int Durability { get; set; }
        public double Weight { get; set; }
        //public int StrengthRequirement { get; set; }
        //public int DexterityRequirement { get; set; }
        //public int IntelligenceRequirement { get; set; }
        //public int FaithRequirement { get; set; }

        [ForeignKey("ArmorTypeId")]
        public DS2ArmorType ArmorType { get; set; }
    }
}