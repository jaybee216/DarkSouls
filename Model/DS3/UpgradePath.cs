using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DS3
{
    public class UpgradePath
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UpgradePathId { get; set; }

        public string Type { get; set; }

        public int MaxUpgradeLevel { get; set; }

        public enum Types
        {
            Normal = 1,
            Twinkling = 2,
            Scale = 3
        }
    }
}
