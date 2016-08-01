using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DarkSoulsII.Infrastructure;
using Model.DS3;
namespace DarkSoulsII.Areas.DS3.Helpers
{
    public class StatsHelper
    {
        private readonly ICache _cache = IoC.Resolve<ICache>();

        public IEnumerable<SelectListItem> GetReinforcementLevels(int upgradeLevel = 0)
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = "+0", Value = "0", Selected = upgradeLevel == 0 },
                new SelectListItem() { Text = "+1 (+0)", Value = "1", Selected = upgradeLevel == 1 },
                new SelectListItem() { Text = "+2 (+1)", Value = "2", Selected = upgradeLevel == 2 },
                new SelectListItem() { Text = "+3 (+1)", Value = "3", Selected = upgradeLevel == 3 },
                new SelectListItem() { Text = "+4 (+2)", Value = "4", Selected = upgradeLevel == 4 },
                new SelectListItem() { Text = "+5 (+2)", Value = "5", Selected = upgradeLevel == 5 },
                new SelectListItem() { Text = "+6 (+3)", Value = "6", Selected = upgradeLevel == 6 },
                new SelectListItem() { Text = "+7 (+3)", Value = "7", Selected = upgradeLevel == 7 },
                new SelectListItem() { Text = "+8 (+4)", Value = "8", Selected = upgradeLevel == 8 },
                new SelectListItem() { Text = "+9 (+4)", Value = "9", Selected = upgradeLevel == 9 },
                new SelectListItem() { Text = "+10 (+5)", Value = "10", Selected = upgradeLevel == 10 }
            };
        }

        public IEnumerable<SelectListItem> GetStartingClasses(int? startingClassId = null)
        {
            IEnumerable<StartingClass> classes = _cache.GetDS3StartingClasses();
            List<SelectListItem> classSelectList = classes
            .OrderBy(c => c.StartingClassId)
            .Select(c => new SelectListItem()
            {
                Selected = (startingClassId != null || c.StartingClassId == startingClassId),
                Text = c.Name,
                Value = c.StartingClassId.ToString()
            })
            .ToList();
            return classSelectList;
        }

        public StartingClass GetStartingClass(int startingClassId)
        {
            return _cache.GetDS3StartingClasses().Single(c => c.StartingClassId == startingClassId);
        }
    }
}