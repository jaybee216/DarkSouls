namespace DarkSoulsII.Areas.DS3.ViewModels
{
    public class WeaponViewModel
    {
        public int WeaponId { get; set; }
        public int WeaponTypeId { get; set; }
        public int InfusionId { get; set; }

        public string Name { get; set; }
        public string WeaponType { get; set; }
        public string Infusion { get; set; }
        //public bool IsBuffable { get; set; }
        public int StrReq { get; set; }
        public int DexReq { get; set; }
        public int IntReq { get; set; }
        public int FthReq { get; set; }
        public double Bleed { get; set; }
        public double Poison { get; set; }
        public double Frost { get; set; }
        public double Physical { get; set; }
        public double Magic { get; set; }
        public double Fire { get; set; }
        public double Lightning { get; set; }
        public double Dark { get; set; }
        public double StrScaling { get; set; }
        public double DexScaling { get; set; }
        public double IntScaling { get; set; }
        public double FthScaling { get; set; }
        public double LckScaling { get; set; }
        public int UpgradeLevel { get; set; }
        public decimal Weight { get; set; }
        public int Critical { get; set; }
        public string UpgradePath { get; set; }
    }
}