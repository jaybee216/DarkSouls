using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DS3
{
    public class ArmorType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArmorTypeId { get; set; }
        public string Name { get; set; }

        public enum Types
        {
            Chest = 1,
            Gauntlet = 2,
            Helm = 3,
            Legging = 4
        }
    }
}
