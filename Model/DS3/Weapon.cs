using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DS3
{
    public class Weapon
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeaponId { get; set; }
        public string Name { get; set; }
        public int WeaponTypeId { get; set; }
        public bool IsBuffable { get; set; }
        public int StrReq { get; set; }
        public int DexReq { get; set; }
        public int IntReq { get; set; }
        public int FthReq { get; set; }
        public decimal Weight { get; set; }
        public int? UpgradePathId { get; set; }
        public bool CanInfuse { get; set; }

        [ForeignKey("WeaponTypeId")]
        public WeaponType WeaponType { get; set; }

        [ForeignKey("UpgradePathId")]
        public UpgradePath UpgradePath { get; set; }
    }
}
