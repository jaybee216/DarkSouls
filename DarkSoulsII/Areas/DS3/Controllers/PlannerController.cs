using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Model.DS3;
using DarkSoulsII.Areas.DS3.Helpers;

namespace DarkSoulsII.Areas.DS3.Controllers
{
    public class PlannerController : Controller
    {
        private readonly WeaponsHelper _weaponsHelper = new WeaponsHelper();
        private readonly ArmorHelper _armorHelper = new ArmorHelper();
        private readonly StatsHelper _statsHelper = new StatsHelper();

        // GET: DS3/Planner
        public ActionResult Index()
        {
            ViewBag.Weapons = _weaponsHelper.GetWeapons();
            ViewBag.ReinforcementLevels = _statsHelper.GetReinforcementLevels();
            ViewBag.Helms = _armorHelper.GetArmor(armorTypeId: (int)ArmorType.Types.Helm);
            ViewBag.Chests = _armorHelper.GetArmor(armorTypeId: (int)ArmorType.Types.Chest);
            ViewBag.Gauntlets = _armorHelper.GetArmor(armorTypeId: (int)ArmorType.Types.Gauntlet);
            ViewBag.Leggings = _armorHelper.GetArmor(armorTypeId: (int)ArmorType.Types.Legging);
            ViewBag.StartingClasses = _statsHelper.GetStartingClasses();
            ViewBag.InfusionTypes = _weaponsHelper.GetInfusionTypes();
            ViewBag.Rings = new List<SelectListItem>();
            return View();
        }

        [HttpGet]
        public JsonResult ChangeStartingClass(int startingClassId)
        {
            StartingClass startingClass = _statsHelper.GetStartingClass(startingClassId);
            return Json(startingClass, JsonRequestBehavior.AllowGet);
        }
    }
}