using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using DarkSoulsII.Infrastructure;
using DataAccess;
//using Models;
using Model;
using Model.DS2;
using Webdiyer.WebControls.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace DarkSoulsII.Controllers
{
    public class EquipmentController : Controller
    {
        // GET: Equipment
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ARCalculator(int? categoryId = null, int? weaponId = null, int? infusionId = null, int? STR = null, int? DEX = null, int? INT = null, int? FTH = null)
        {
            //Set session cache values
            if (categoryId != null)
            {
                Session["ARCalulator_CategoryId"] = categoryId;
            }
            if (weaponId != null)
            {
                Session["ARCalulator_WeaponId"] = weaponId;
            }
            if (infusionId != null)
            {
                Session["ARCalulator_InfusionId"] = weaponId;
            }
            if (STR != null)
            {
                Session["ARCalulator_STR"] = STR;
            }
            if (DEX != null)
            {
                Session["ARCalulator_DEX"] = DEX;
            }
            if (INT != null)
            {
                Session["ARCalulator_INT"] = INT;
            }
            if (FTH != null)
            {
                Session["ARCalulator_FTH"] = FTH;
            }

            //Retrieve previous session cache values
            if (categoryId == null && Session["ARCalulator_CategoryId"] != null)
            {
                categoryId = (int)Session["ARCalulator_CategoryId"];
            }
            if (categoryId == null && weaponId == null && Session["ARCalulator_WeaponId"] != null)
            {
                weaponId = (int)Session["ARCalulator_WeaponId"];
            }
            if (infusionId == null && Session["ARCalulator_InfusionId"] != null)
            {
                infusionId = (int)Session["ARCalulator_InfusionId"];
            }
            if (STR == null && Session["ARCalulator_STR"] != null)
            {
                STR = (int)Session["ARCalulator_STR"];
            }
            if (DEX == null && Session["ARCalulator_DEX"] != null)
            {
                DEX = (int)Session["ARCalulator_DEX"];
            }
            if (INT == null && Session["ARCalulator_INT"] != null)
            {
                INT = (int)Session["ARCalulator_INT"];
            }
            if (FTH == null && Session["ARCalulator_FTH"] != null)
            {
                FTH = (int)Session["ARCalulator_FTH"];
            }

            IList<WeaponCategory> categories = new List<WeaponCategory>();
            IList<DS2Weapon> weapons = new List<DS2Weapon>();
            IList<Infusion> infusions = new List<Infusion>();

            using (var lifetime = IoC.Container.BeginLifetimeScope())
            {
                using (var unit = lifetime.Resolve<IUnitOfWork>())
                {
                    var cache = lifetime.Resolve<ICache>();
                    categories = cache.GetWeaponCategories();
                    weapons = cache.GetWeapons(w => categoryId == null || w.WeaponCategoryId == categoryId);
                    infusions = cache.GetInfusions();

                    List<SelectListItem> categorySelectList = categories.Select(c => new SelectListItem
                    {
                        Selected = c.WeaponCategoryId == categoryId,
                        Text = c.Name,
                        Value = c.WeaponCategoryId.ToString()
                    }).ToList();

                    List<SelectListItem> weaponSelectList = weapons.Select(w => new SelectListItem
                    {
                        Selected = w.WeaponId == weaponId,
                        Text = w.Name,
                        Value = w.WeaponId.ToString()
                    }).ToList();

                    List<SelectListItem> infusionSelectList = infusions.Select(i => new SelectListItem
                    {
                        Selected = i.InfusionId == infusionId,
                        Text = i.Name,
                        Value = i.InfusionId.ToString()
                    }).ToList();

                    ViewBag.WeaponCategories = categorySelectList;
                    ViewBag.Weapons = weaponSelectList;
                    ViewBag.Infusions = infusionSelectList;
                    ViewBag.WeaponCategoryId = categoryId;
                    ViewBag.WeaponId = weaponId;
                    ViewBag.STR = STR;
                    ViewBag.DEX = DEX;
                    ViewBag.INT = INT;
                    ViewBag.FTH = FTH;

                    bool calculateAR = (weaponId.HasValue && infusionId.HasValue && STR.HasValue && DEX.HasValue && INT.HasValue && FTH.HasValue);
                    int? totalAR = null;
                    if (calculateAR)
                    {
                        totalAR = CalculateAR(weaponId.Value, infusionId.Value, STR.Value, DEX.Value, INT.Value, FTH.Value);
                    }
                    ViewBag.TotalAR = totalAR;

                    return View();
                }
            }
        }
        
        private int CalculateAR(int weaponId, int infusionId, int STR, int DEX, int INT, int FTH)
        {
            //using (var lifetime = IoC.Container.BeginLifetimeScope())
            //{
            //    using (var unit = lifetime.Resolve<IUnitOfWork>())
            //    {
            //        SqlParameter[] parameters = new SqlParameter[]
            //            {
            //                new SqlParameter("@WeaponId", weaponId),
            //                new SqlParameter("@InfusionId", infusionId),
            //                new SqlParameter("@STR", STR),
            //                new SqlParameter("@DEX", DEX),
            //                new SqlParameter("@INT", INT),
            //                new SqlParameter("@FTH", FTH),
            //                new SqlParameter("@ROB", false),
            //                new SqlParameter("@Flynn", false)
            //            };

            //        totalAR = unit.SqlQuery<int>("Exec [dbo].[CalculateAR] @WeaponId, @InfusionId, @STR, @DEX, @INT, @FTH, @ROB, @Flynn", parameters).FirstOrDefault();
            //    }
            //}

            int totalAR = 0;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DarkSoulsIIContext"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("dbo.CalculateAR", connection))
                {
                    connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@WeaponId", weaponId));
                    command.Parameters.Add(new SqlParameter("@InfusionId", infusionId));
                    command.Parameters.Add(new SqlParameter("@STR", STR));
                    command.Parameters.Add(new SqlParameter("@DEX", DEX));
                    command.Parameters.Add(new SqlParameter("@INT", INT));
                    command.Parameters.Add(new SqlParameter("@FTH", FTH));
                    command.Parameters.Add(new SqlParameter("@ROB", false));
                    command.Parameters.Add(new SqlParameter("@Flynn", false));
                    command.CommandTimeout = 1;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                totalAR = Convert.ToInt32(reader[0]);
                            }
                        }
                    }
                }
            }
            
            return totalAR;
        }

        [HttpGet]
        public ActionResult Weapons(string search = null, int? categoryId = null,
                                    int? minStr = null, int? minDex = null, int? minInt = null, int? minFth = null)
        {
            IList<WeaponCategory> categories = new List<WeaponCategory>();

            //Set session cache values
            if (search != null)
            {
                Session["Weapon_Search"] = search;
            }
            if (categoryId != null)
            {
                Session["Weapon_CategoryId"] = categoryId;
            }

            //Retrieve previous session cache values
            if (search == null && Session["Weapon_Search"] != null)
            {
                search = Session["Weapon_Search"].ToString();
            }
            if (categoryId == null && Session["Weapon_CategoryId"] != null)
            {
                categoryId = (int)Session["Weapon_CategoryId"];
            }

            search = (search == null) ? "" : search.Trim();

            using (var lifetime = IoC.Container.BeginLifetimeScope())
            {
                using (var unit = lifetime.Resolve<IUnitOfWork>())
                {
                    var cache = lifetime.Resolve<ICache>();
                    categories = cache.GetWeaponCategories();

                    IEnumerable<ViewModels.Weapon> weapons = unit.GetRepository<WeaponAttackValues>().Get(w => w.Weapon.Name.Contains(search) &&
                                                                                                         (categoryId == null || w.Weapon.Category.WeaponCategoryId == categoryId) &&
                                                                                                         (minStr == null || w.StrReq <= minStr) &&
                                                                                                         (minDex == null || w.DexReq <= minDex) &&
                                                                                                         (minFth == null || w.FthReq <= minFth) &&
                                                                                                         (minInt == null || w.IntReq <= minInt) &&
                                                                                                         w.Infusion.Name == "Normal", null, "Weapon.Category", "Infusion")
                                                                                                         .Select(w => new ViewModels.Weapon()
                                                                                                         {
                                                                                                             WeaponId = w.WeaponId,
                                                                                                             BaseDark = w.BaseDark.Value.ToString("G29"),
                                                                                                             BaseFire = w.BaseFire.Value.ToString("G29"),
                                                                                                             BaseLightning = w.BaseLightning.Value.ToString("G29"),
                                                                                                             BaseMagic = w.BaseMagic.Value.ToString("G29"),
                                                                                                             BasePhysical = w.BasePhysical.Value.ToString("G29"),
                                                                                                             CastingSpeed = w.Weapon.CastingSpeed,
                                                                                                             Category = w.Weapon.Category.Name,
                                                                                                             Counter = w.Weapon.Counter,
                                                                                                             Name = w.Weapon.Name,
                                                                                                             DarkScaling = (w.DarkScaling.Value * 100).ToString("G29"),
                                                                                                             DexReq = w.DexReq,
                                                                                                             Durability = w.Weapon.Durability,
                                                                                                             FireScaling = (w.FireScaling.Value * 100).ToString("G29"),
                                                                                                             FlynnBonus = w.FlynnBonus,
                                                                                                             DexScaling = (w.DexScaling.Value * 100).ToString("G29"),
                                                                                                             FthReq = w.FthReq,
                                                                                                             IntReq = w.IntReq,
                                                                                                             LightningScaling = (w.LightningScaling.Value * 100).ToString("G29"),
                                                                                                             MagicScaling = (w.MagicScaling.Value * 100).ToString("G29"),
                                                                                                             MaxUpgrade = w.MaxUpgrade,
                                                                                                             PoiseDamage = w.Weapon.PoiseDamage,
                                                                                                             Range = w.Weapon.Range,
                                                                                                             RobBonus = w.RoBBonus,
                                                                                                             Stability = w.Weapon.Stability,
                                                                                                             StrReq = w.StrReq,
                                                                                                             StrScaling = (w.StrScaling.Value * 100).ToString("G29"),
                                                                                                             Weight = w.Weapon.Weight,
                                                                                                             Infusion = w.Infusion.Name
                                                                                                         });

                    weapons = weapons.OrderBy(w => w.Name);
                    ViewData.Model = weapons.ToList();
                }
            }

            List<SelectListItem> categorySelectList = categories.Select(c => new SelectListItem
            {
                Selected = c.WeaponCategoryId == categoryId,
                Text = c.Name,
                Value = c.WeaponCategoryId.ToString()
            }).ToList();
            ViewBag.WeaponCategories = categorySelectList;

            ViewBag.WeaponCategoryId = categoryId;
            ViewBag.MinimumSTR = minStr;
            ViewBag.MinimumDEX = minDex;
            ViewBag.MinimumINT = minInt;
            ViewBag.MinimumFTH = minFth;
            ViewBag.Search = search;

            return View();
        }

        [HttpGet]
        public ActionResult WeaponDetails(int id)
        {
            using (var lifetime = IoC.Container.BeginLifetimeScope())
            {
                using (var unit = lifetime.Resolve<IUnitOfWork>())
                {
                    var weapons = unit.GetRepository<WeaponAttackValues>().Get(w => w.Weapon.WeaponId == id, null, "Weapon.Category", "Infusion")
                                                            .Select(w => new ViewModels.Weapon()
                                                            {
                                                                BaseDark = w.BaseDark.Value.ToString("G29"),
                                                                BaseFire = w.BaseFire.Value.ToString("G29"),
                                                                BaseLightning = w.BaseLightning.Value.ToString("G29"),
                                                                BaseMagic = w.BaseMagic.Value.ToString("G29"),
                                                                BasePhysical = w.BasePhysical.Value.ToString("G29"),
                                                                CastingSpeed = w.Weapon.CastingSpeed,
                                                                Category = w.Weapon.Category.Name,
                                                                Counter = w.Weapon.Counter,
                                                                Name = w.Weapon.Name,
                                                                DarkScaling = (w.DarkScaling.Value * 100).ToString("G29"),
                                                                DexReq = w.DexReq,
                                                                Durability = w.Weapon.Durability,
                                                                FireScaling = (w.FireScaling.Value * 100).ToString("G29"),
                                                                FlynnBonus = w.FlynnBonus,
                                                                DexScaling = (w.DexScaling.Value * 100).ToString("G29"),
                                                                FthReq = w.FthReq,
                                                                IntReq = w.IntReq,
                                                                LightningScaling = (w.LightningScaling.Value * 100).ToString("G29"),
                                                                MagicScaling = (w.MagicScaling.Value * 100).ToString("G29"),
                                                                MaxUpgrade = w.MaxUpgrade,
                                                                PoiseDamage = w.Weapon.PoiseDamage,
                                                                Range = w.Weapon.Range,
                                                                RobBonus = w.RoBBonus,
                                                                Stability = w.Weapon.Stability,
                                                                StrReq = w.StrReq,
                                                                StrScaling = (w.StrScaling.Value * 100).ToString("G29"),
                                                                Weight = w.Weapon.Weight,
                                                                Infusion = w.Infusion.Name,
                                                                DisplayOrder = w.Infusion.DisplayOrder,
                                                                FullName = w.Infusion.Name == "Normal" ? w.Weapon.Name : w.Infusion.Name + " " + w.Weapon.Name
                                                            });

                    ViewModels.WeaponDetails model = new ViewModels.WeaponDetails()
                    {
                        Normal = weapons.Single(w => w.Infusion == "Normal"),
                        Infusions = weapons.Where(w => w.Infusion != "Normal").OrderBy(w => w.DisplayOrder).ToList()
                };
                    return View(model);
                }
            }
        }
    }
}