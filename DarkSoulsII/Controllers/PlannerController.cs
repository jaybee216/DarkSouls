using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using DarkSoulsII.Infrastructure;
using Autofac;
using System.Data.SqlClient;
using Model.DS2;
using DarkSoulsII.ViewModels;
using DarkSoulsII.Helpers;

namespace DarkSoulsII.Controllers
{
    public class PlannerController : Controller
    {
        private readonly WeaponsHelper _weaponsHelper = new WeaponsHelper();
        private readonly ArmorHelper _armorHelper = new ArmorHelper();
        private readonly RingsHelper _ringsHelper = new RingsHelper();

        // GET: Planner
        public ActionResult Index()
        {
            ViewBag.Vit = 9;
            ViewBag.ADP = 10;
            ViewBag.END = 10;
            ViewBag.WeaponTypes = _weaponsHelper.GetWeaponTypes();
            ViewBag.Weapons = _weaponsHelper.GetWeapons();
            ViewBag.Helms = _armorHelper.GetArmor(armorTypeId: (int)DS2ArmorType.Types.Helm);
            ViewBag.Chests = _armorHelper.GetArmor(armorTypeId: (int)DS2ArmorType.Types.Chest);
            ViewBag.Gauntlets = _armorHelper.GetArmor(armorTypeId: (int)DS2ArmorType.Types.Gauntlet);
            ViewBag.Leggings = _armorHelper.GetArmor(armorTypeId: (int)DS2ArmorType.Types.Legging);
            ViewBag.Rings = _ringsHelper.GetRings();
            return View();
        }

        [HttpPost]
        public ActionResult CalculateEquipBurden(int vit,
                                                 int adp,
                                                 int end,
                                                 int? rWeapon1 = null, 
                                                 int? lWeapon1 = null,
                                                 int? helm = null,
                                                 int? chest = null,
                                                 int? gauntlet = null,
                                                 int? legging = null,
                                                 int? ring1 = null,
                                                 int? ring2 = null,
                                                 int? ring3 = null,
                                                 int? ring4 = null)
        {
            double maxEquipLoad = vit > 29 ? (38.5 + (1.5 * 29) + 1 * (vit - 29)) : (38.5 + (1.5 * vit));
            decimal currentEquipLoad = 0;
            double equipPercentage = 0;
            double poise = 0;

            int relevantPoiseStat = Math.Min(adp, end);
            if (relevantPoiseStat > 50) {
                poise = 30 * .3 + 20 * .2 + .1 * (relevantPoiseStat - 50);
            }
            else if (relevantPoiseStat > 30)
            {
                poise = 30 * .3 + .2 * (relevantPoiseStat - 50);
            } else
            {
                poise = relevantPoiseStat * .3;
            }

            List<Ring> rings = new List<Ring>();
            if (ring1 > 0)
                rings.Add(_ringsHelper.GetRing(ring1.Value));
            if (ring2 > 0)
                rings.Add(_ringsHelper.GetRing(ring2.Value));
            if (ring3 > 0)
                rings.Add(_ringsHelper.GetRing(ring3.Value));
            if (ring4 > 0)
                rings.Add(_ringsHelper.GetRing(ring4.Value));

            foreach (Ring ring in rings)
            {
                currentEquipLoad += ring.Weight;

                if (ring.Effects != null)
                {
                    foreach (Effect effect in ring.Effects)
                    {
                        if (effect.Affects == Effect.Modifies.EquipLoad)
                        {
                            maxEquipLoad = (maxEquipLoad * effect.Amount);
                        }
                    }
                }
            }

            if (rWeapon1 != null)
            {
                WeaponViewModel rW1 = _weaponsHelper.WeaponSearch(weaponId: rWeapon1).First();
                currentEquipLoad += rW1.Weight.Value;
            }

            if (lWeapon1 != null)
            {
                WeaponViewModel lW1 = _weaponsHelper.WeaponSearch(weaponId: lWeapon1).First();
                currentEquipLoad += lW1.Weight.Value;
            }

            if (helm != null)
            {
                ArmorViewModel hlm = _armorHelper.ArmorSearch(armorId: helm).First();
                currentEquipLoad += (decimal)hlm.Weight;
                poise += hlm.Poise;
            }

            if (chest != null)
            {
                ArmorViewModel cht = _armorHelper.ArmorSearch(armorId: chest).First();
                currentEquipLoad += (decimal)cht.Weight;
                poise += cht.Poise;
            }

            if (gauntlet != null)
            {
                ArmorViewModel glt = _armorHelper.ArmorSearch(armorId: gauntlet).First();
                currentEquipLoad += (decimal)glt.Weight;
                poise += glt.Poise;
            }

            if (legging != null)
            {
                ArmorViewModel lgg = _armorHelper.ArmorSearch(armorId: legging).First();
                currentEquipLoad += (decimal)lgg.Weight;
                poise += lgg.Poise;
            }

            equipPercentage = (double)currentEquipLoad / maxEquipLoad;
            string equipLoad = String.Format("{0} / {1} ({2:P2})", currentEquipLoad, maxEquipLoad, equipPercentage);

            return Json(new { equipLoad = equipLoad, poise = poise }, JsonRequestBehavior.AllowGet);
        }
    }
}