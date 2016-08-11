namespace DarkSoulsII.ViewModels
{
    public class WeaponARModel
    {
        public string WeaponName { get; set; }
        public int WeaponId { get; set; }
        public int InfusionId { get; set; }
        public double TotalAR { get; set; }
        public double PhysAR { get; set; }
        public double MagicAR { get; set; }
        public double FireAR { get; set; }
        public double LightningAR { get; set; }
        public double DarkAR { get; set; }

        public bool RequirementsMet { get; set; }
        public int StrReq { get; set; }
        public int DexReq { get; set; }
        public int IntReq { get; set; }
        public int FthReq { get; set; }
    }
}