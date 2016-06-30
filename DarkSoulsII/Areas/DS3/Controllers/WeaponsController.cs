using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DarkSoulsII.Infrastructure;
using Model;
using DataAccess;
using Model.DS3;
using DarkSoulsII.Areas.DS3.ViewModels;
using DarkSoulsII.Areas.DS3.Helpers;
//using Autofac;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace DarkSoulsII.Areas.DS3.Controllers
{
    public class WeaponsController : Controller
    {
        private readonly WeaponsHelper _weaponsHelper = new WeaponsHelper();
        private readonly StatsHelper _statsHelper = new StatsHelper();

        // GET: DS3/Weapons
        public ActionResult Index(string searchValue = "", int weaponTypeId = 0, int infusionId = 1, int upgradeLevel = 10, 
                                  double? weight = null, int? STR = null, int? DEX = null, int? INT = null, int? FTH = null)
        {
            //if (searchValue != null)
            //{
            //    Session["DS3_Weapon_Search"] = searchValue;
            //}

            //Session["DS3_Weapon_WeaponTypeId"] = weaponTypeId;

            //Session["DS3_Weapon_InfusionId"] = infusionId;

            //Session["DS3_Weapon_Weight"] = weight;

            //if (searchValue == null && Session["DS3_Weapon_Search"] != null)
            //{
            //    searchValue = Session["DS3_Weapon_Search"].ToString();
            //}

            //if (Session["DS3_Weapon_WeaponTypeId"] != null)
            //{
            //    weaponTypeId = (int)Session["DS3_Weapon_WeaponTypeId"];
            //}

            //if (Session["DS3_Weapon_InfusionId"] != null)
            //{
            //    infusionId = (int)Session["DS3_Weapon_InfusionId"];
            //}

            //if (Session["DS3_Weapon_Weight"] != null)
            //{
            //    weight = (double)Session["DS3_Weapon_Weight"];
            //}

            searchValue = (searchValue == null) ? "" : searchValue.Trim();

            //TODO: Include Bleed/Poison/Frost
            IEnumerable<WeaponViewModel> weapons = _weaponsHelper.WeaponSearch(searchValue, null, weaponTypeId, infusionId, upgradeLevel, weight, STR, DEX, INT, FTH);
            ViewData.Model = weapons.OrderBy(w => w.Name).ToList();

            ViewBag.WeaponTypes = _weaponsHelper.GetWeaponTypes(weaponTypeId);
            ViewBag.ReinforcementLevels = _statsHelper.GetReinforcementLevels(upgradeLevel);
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
        public ActionResult Details(int weaponId, int upgradeLevel)
        {
            //TODO: Include Bleed/Poison/Frost
            IEnumerable<WeaponViewModel> weapons = _weaponsHelper.WeaponSearch(weaponId: weaponId, upgradeLevel: upgradeLevel);

            WeaponDetailsView model = new WeaponDetailsView();
            model.Normal = weapons.Single(w => w.Infusion == "Normal");
            model.Infusions = weapons.Where(w => w.Infusion != "Normal").OrderBy(w => w.Infusion).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult ARCalculator(int? weaponId = null, int weaponTypeId = 0, int infusionId = 1, int upgradeLevel = 10)
        {
            ViewBag.WeaponTypes = _weaponsHelper.GetWeaponTypes(weaponTypeId);
            ViewBag.Weapons = _weaponsHelper.GetWeapons(weaponId, weaponTypeId);
            ViewBag.InfusionTypes = _weaponsHelper.GetInfusionTypes(infusionId);
            ViewBag.ReinforcementLevels = _statsHelper.GetReinforcementLevels(upgradeLevel);
            ViewBag.StartingClasses = _statsHelper.GetStartingClasses();
            return View();
        }

        [HttpGet]
        public JsonResult ChangeStartingClass(int startingClassId)
        {
            StartingClass startingClass = _statsHelper.GetStartingClass(startingClassId);
            return Json(startingClass, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CalculateAR(int weaponId, int infusionId, int upgradeLevel, int STR, int DEX, int INT, int FTH)
        {
            //TODO: Include Bleed/Poison/Frost
            
            WeaponARModel model = _weaponsHelper.CalculateAR(weaponId, infusionId, upgradeLevel, STR, DEX, INT, FTH);
            return PartialView("_ARValues", model);
        }

        [HttpGet]
        public JsonResult Weapons(int weaponTypeId = 0)
        {
            var weapons = _weaponsHelper.GetWeapons(weaponTypeId: weaponTypeId);
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