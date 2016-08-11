namespace DarkSoulsII.ViewModels
{
    public class ArmorViewModel
    {
        public int ArmorId { get; set; }
        public int ArmorTypeId { get; set; }
        
        public string Name { get; set; }
        public string ArmorType { get; set; }
        public double Weight { get; set; }
        public int Durability { get; set; }
        public int Poise { get; set; }
        public double PoiseToWeight { get; set; }
        public double DefenseToWeight { get; set; }
        public int Poison { get; set; }
        public int Petrification { get; set; }
        public int Bleed { get; set; }
        public int Curse { get; set; }
        public int Physical { get; set; }
        public int Slash { get; set; }
        public int Strike { get; set; }
        public int Thrust { get; set; }
        public int Magic { get; set; }
        public int Fire { get; set; }
        public int Lightning { get; set; }
        public int Dark { get; set; }
    }
}