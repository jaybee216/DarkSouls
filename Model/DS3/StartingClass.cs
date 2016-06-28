using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DS3
{
    public class StartingClass
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StartingClassId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Vigor { get; set; }
        public int Attunement { get; set; }
        public int Endurance { get; set; }
        public int Vitality { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Faith { get; set; }
        public int Luck { get; set; }
    }
}
