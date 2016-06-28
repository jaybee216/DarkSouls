using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DarkSoulsII.Models
{
    public class WeaponAttackValues
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeaponAttackValuesId { get; set; }
        public int WeaponId { get; set; }
        public int InfusionId { get; set; }
        public int MaxUpgrade { get; set; }
        public decimal? BasePhysical { get; set; }
        public decimal? BaseMagic { get; set; }
        public decimal? BaseFire { get; set; }
        public decimal? BaseLightning { get; set; }
        public decimal? BaseDark { get; set; }
        public decimal? StrScaling { get; set; }
        public decimal? DexScaling { get; set; }
        public decimal? MagicScaling { get; set; }
        public decimal? FireScaling { get; set; }
        public decimal? LightningScaling { get; set; }
        public decimal? DarkScaling { get; set; }
        public int? StrReq { get; set; }
        public int? DexReq { get; set; }
        public int? FthReq { get; set; }
        public int? IntReq { get; set; }
        public int? RoBBonus { get; set; }
        public int? FlynnBonus { get; set; }

        [ForeignKey("WeaponId")]
        public Weapon Weapon { get; set; }
        [ForeignKey("InfusionId")]
        public Infusion Infusion { get; set; }
    }
}