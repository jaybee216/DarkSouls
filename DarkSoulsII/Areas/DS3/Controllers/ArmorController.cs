using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DarkSoulsII.Areas.DS3.ViewModels;
using DarkSoulsII.Areas.DS3.Helpers;

namespace DarkSoulsII.Areas.DS3.Controllers
{
    public class ArmorController : Controller
    {
        private readonly ArmorHelper _armorHelper = new ArmorHelper();
        private readonly StatsHelper _statsHelper = new StatsHelper();

        // GET: DS3/Armor
        public ActionResult Index(string searchValue = "", int armorTypeId = 0, double? maxWeight = null, double? minPoise = null)
        {
            searchValue = (searchValue == null) ? "" : searchValue.Trim();
            
            IEnumerable<ArmorViewModel> armors = _armorHelper.ArmorSearch(searchValue, null, armorTypeId, maxWeight, minPoise);
            ViewData.Model = armors.OrderBy(a => a.Name).ToList();

            ViewBag.ArmorTypes = _armorHelper.GetArmorTypes(armorTypeId);
            ViewBag.ArmorTypeId = armorTypeId;
            ViewBag.SearchValue = searchValue;
            ViewBag.MaxWeight = maxWeight;
            ViewBag.MinPoise = minPoise;

            return View();
        }

        [HttpGet]
        public ActionResult Details(int armorId)
        {
            ArmorViewModel armor = _armorHelper.ArmorSearch(armorId: armorId).Single();
            return View(armor);
        }

        [HttpGet]
        public JsonResult ArmorAutocomplete(string term)
        {
            term = term.ToLower();
            var armor = _armorHelper.GetArmor().Where(w => w.Text.ToLower().Contains(term));
            return Json(armor, JsonRequestBehavior.AllowGet);
        }
    }
}