using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarkSoulsII.ViewModels
{
    public class Weapon
    {
        public int WeaponId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Infusion { get; set; }
        public string FullName { get; set; }
        public int? DisplayOrder { get; set; }
        public int? Counter { get; set; }
        public int? PoiseDamage { get; set; }
        public decimal? Range { get; set; }
        public int? CastingSpeed { get; set; }
        public decimal? Stability { get; set; }
        public int? Durability { get; set; }
        public decimal? Weight { get; set; }
        public int MaxUpgrade { get; set; }
        public string BasePhysical { get; set; }
        public string BaseMagic { get; set; }
        public string BaseFire { get; set; }
        public string BaseLightning { get; set; }
        public string BaseDark { get; set; }
        public string StrScaling { get; set; }
        public string DexScaling { get; set; }
        public string MagicScaling { get; set; }
        public string FireScaling { get; set; }
        public string LightningScaling { get; set; }
        public string DarkScaling { get; set; }
        public int? StrReq { get; set; }
        public int? DexReq { get; set; }
        public int? FthReq { get; set; }
        public int? IntReq { get; set; }
        public int? RobBonus { get; set; }
        public int? FlynnBonus { get; set; }
    }
}