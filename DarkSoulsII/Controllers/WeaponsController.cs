using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Model.DS2;
using DarkSoulsII.ViewModels;
using DarkSoulsII.Helpers;

namespace DarkSoulsII.Controllers
{
    public class WeaponsController : Controller
    {
        private readonly WeaponsHelper _weaponsHelper = new WeaponsHelper();
        //private readonly StatsHelper _statsHelper = new StatsHelper();

        // GET: DS3/Weapons
        public ActionResult Index(string searchValue = "", int weaponTypeId = 0, int infusionId = 6,
                                  double? weight = null, int? STR = null, int? DEX = null, int? INT = null, int? FTH = null)
        {
            searchValue = (searchValue == null) ? "" : searchValue.Trim();

            //TODO: Include Bleed/Poison
            IEnumerable<WeaponViewModel> weapons = _weaponsHelper.WeaponSearch(searchValue, null, weaponTypeId, infusionId, weight, STR, DEX, INT, FTH);
            ViewData.Model = weapons.OrderBy(w => w.Name).ToList();

            ViewBag.WeaponTypes = _weaponsHelper.GetWeaponTypes(weaponTypeId);
            ViewBag.InfusionTypes = _weaponsHelper.GetInfusionTypes(infusionId);
            ViewBag.WeaponTypeId = weaponTypeId;
            ViewBag.SearchValue = searchValue;
            ViewBag.Weight = weight;
            ViewBag.STR = STR;
            ViewBag.DEX = DEX;
            ViewBag.INT = INT;
            ViewBag.FTH = FTH;

            return View();
        }

        [HttpGet]
        public ActionResult Details(int weaponId)
        {
            //TODO: Include Bleed/Poison
            IEnumerable<WeaponViewModel> weapons = _weaponsHelper.WeaponSearch(weaponId: weaponId);
            WeaponDetailsView model = new WeaponDetailsView();
            model.Normal = weapons.Single(w => w.Infusion == "Normal");
            model.Infusions = weapons.Where(w => w.Infusion != "Normal").OrderBy(w => w.Infusion).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult ARCalculator(int? weaponId = null, int weaponTypeId = 0, int infusionId = 6)
        {
            ViewBag.WeaponTypes = _weaponsHelper.GetWeaponTypes(weaponTypeId);
            ViewBag.Weapons = _weaponsHelper.GetWeapons(weaponId, weaponTypeId);
            ViewBag.InfusionTypes = _weaponsHelper.GetInfusionTypes(infusionId);
            //ViewBag.StartingClasses = _statsHelper.GetStartingClasses();
            return View();
        }

        //[HttpGet]
        //public JsonResult ChangeStartingClass(int startingClassId)
        //{
        //    StartingClass startingClass = _statsHelper.GetStartingClass(startingClassId);
        //    return Json(startingClass, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ActionResult CalculateAR(int weaponId, int infusionId, int STR, int DEX, int INT, int FTH)
        {
            //TODO: Include Bleed/Poison
            //if (twoHand)
            //{
            //    STR = (int)System.Math.Floor((double)STR * 1.5);
            //}
            WeaponARModel model = _weaponsHelper.CalculateAR(weaponId, infusionId, STR, DEX, INT, FTH);
            return PartialView("_ARValues", model);
        }

        [HttpGet]
        public JsonResult WeaponsAutocomplete(string term)
        {
            term = term.ToLower();
            var weapons = _weaponsHelper.GetWeapons().Where(w => w.Text.ToLower().Contains(term));
            return Json(weapons, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Weapons(int weaponTypeId = 0)
        {
            var weapons = _weaponsHelper.GetWeapons(weaponCategoryId: weaponTypeId);
            return Json(weapons, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Infusions(int? weaponId = null)
        {
            var infusions = _weaponsHelper.GetInfusionTypes(weaponId: weaponId);
            return Json(infusions, JsonRequestBehavior.AllowGet);
        }
    }
}