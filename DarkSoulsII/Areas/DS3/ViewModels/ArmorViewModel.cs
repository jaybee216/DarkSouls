﻿namespace DarkSoulsII.Areas.DS3.ViewModels
{
    public class ArmorViewModel
    {
        public int ArmorId { get; set; }
        public int ArmorTypeId { get; set; }
        
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public int Durability { get; set; }
        public decimal Poise { get; set; }
        public decimal PoiseToWeight { get; set; }
        public double DefenseToWeight { get; set; }
        public double Poison { get; set; }
        public double Toxic { get; set; }
        public double Blood { get; set; }
        public double Curse { get; set; }
        public double Frost { get; set; }
        public double Physical { get; set; }
        public double Slash { get; set; }
        public double Strike { get; set; }
        public double Thrust { get; set; }
        public double Magic { get; set; }
        public double Fire { get; set; }
        public double Lightning { get; set; }
        public double Dark { get; set; }
    }
}