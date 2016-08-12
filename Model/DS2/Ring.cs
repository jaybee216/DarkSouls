using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DS2
{
    public class Ring
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RingId { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set;}

        public virtual ICollection<Effect> Effects { get; set; }
    }

    public class Effect
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EffectId { get; set; }
        public int RingId { get; set; }
        public Modifies Affects { get; set; }
        public double Amount { get; set; }        

        [ForeignKey("RingId")]
        public virtual Ring Ring { get; set; }

        public enum Modifies
        {
            EquipLoad = 0,
            HP = 1,
            Stamina = 2
        }
    }
}