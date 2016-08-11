namespace DarkSoulsII.ViewModels
{
    public class WeaponViewModel
    {
        public int WeaponId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Infusion { get; set; }
        public string FullName { get; set; }
        public int? DisplayOrder { get; set; }
        public int? Counter { get; set; }
        public int? PoiseDamage { get; set; }
        //public decimal? Range { get; set; }
        //public int? CastingSpeed { get; set; }
        //public decimal? Stability { get; set; }
        public int? Durability { get; set; }
        public decimal? Weight { get; set; }
        public int MaxUpgrade { get; set; }
        public double BasePhysical { get; set; }
        public double BaseMagic { get; set; }
        public double BaseFire { get; set; }
        public double BaseLightning { get; set; }
        public double BaseDark { get; set; }
        public double StrScaling { get; set; }
        public double DexScaling { get; set; }
        public double MagicScaling { get; set; }
        public double FireScaling { get; set; }
        public double LightningScaling { get; set; }
        public double DarkScaling { get; set; }
        public int? StrReq { get; set; }
        public int? DexReq { get; set; }
        public int? FthReq { get; set; }
        public int? IntReq { get; set; }
    }
}