namespace DarkSoulsII.Areas.DS3.ViewModels
{
    public class WeaponARModel
    {
        public string WeaponName { get; set; }
        public int WeaponId { get; set; }
        public int InfusionId { get; set; }
        public int ReinforcementLevel { get; set; }
        public double TotalAR { get; set; }
        public double PhysAR { get; set; }
        public double MagicAR { get; set; }
        public double FireAR { get; set; }
        public double LightningAR { get; set; }
        public double DarkAR { get; set; }
    }
}