using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccess;
using DarkSoulsII.Infrastructure;
using Autofac;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Model.DS3;
using DarkSoulsII.Areas.DS3.ViewModels;

namespace DarkSoulsII.Areas.DS3.Helpers
{
    public class WeaponsHelper
    {
        private readonly ICache _cache = IoC.Resolve<ICache>();

        public IEnumerable<WeaponViewModel> WeaponSearch(string searchValue = "",
                                                         int? weaponId = null,
                                                         int weaponTypeId = 0,
                                                         int? infusionId = null,
                                                         int upgradeLevel = 10,
                                                         double? weight = null,
                                                         int? STR = null,
                                                         int? DEX = null,
                                                         int? INT = null,
                                                         int? FTH = null)
        {
            using (var lifetime = IoC.Container.BeginLifetimeScope())
            {
                using (var unit = lifetime.Resolve<IUnitOfWork>())
                {
                    SqlParameter[] parameters = new SqlParameter[]
                       {
                            new SqlParameter("@SearchValue", string.IsNullOrWhiteSpace(searchValue) ? SqlString.Null : searchValue),
                            new SqlParameter("@WeaponId", weaponId ?? SqlInt32.Null),
                            new SqlParameter("@WeaponTypeId", weaponTypeId),
                            new SqlParameter("@InfusionId", infusionId ?? SqlInt32.Null),
                            new SqlParameter("@ReinforcementLevel", upgradeLevel),
                            new SqlParameter("@MaxWeight", weight ?? SqlDouble.Null),
                            new SqlParameter("@STR", STR ?? SqlInt32.Null),
                            new SqlParameter("@DEX", DEX ?? SqlInt32.Null),
                            new SqlParameter("@INT", INT ?? SqlInt32.Null),
                            new SqlParameter("@FTH", FTH ?? SqlInt32.Null)
                       };

                    string query = "Exec [DS3].[WeaponSearch] @SearchValue, @WeaponId, @WeaponTypeId, @InfusionId, @ReinforcementLevel, @MaxWeight, @STR, @DEX, @INT, @FTH";

                    IEnumerable<WeaponViewModel> weapons = unit.SqlQuery<WeaponViewModel>(query, parameters).ToList();

                    foreach (WeaponViewModel weapon in weapons)
                    {
                        weapon.UpgradeLevel = upgradeLevel;
                    }

                    return weapons;
                }
            }
        }

        public IEnumerable<SelectListItem> GetWeaponTypes(int weaponTypeId = 0)
        {
            IEnumerable<WeaponType> weaponTypes = _cache.GetDS3WeaponTypes();
            List<SelectListItem> weaponTypeSelectList = new List<SelectListItem>();
            weaponTypeSelectList.Add(new SelectListItem() { Text = "All", Value = "0", Selected = weaponTypeId == 0 });
            weaponTypeSelectList.AddRange(weaponTypes
            .OrderBy(wt => wt.Name)
            .Select(wt => new SelectListItem()
            {
                Selected = wt.WeaponTypeId == weaponTypeId,
                Text = wt.Name,
                Value = wt.WeaponTypeId.ToString()
            })
            .ToList());
            return weaponTypeSelectList;
        }

        public IEnumerable<SelectListItem> GetInfusionTypes(int infusionId = 1, int? weaponId = null)
        {
            IEnumerable<InfusionType> infusionTypes = _cache.GetDS3Infusions();
            if (weaponId != null)
            {
                Weapon weapon = _cache.GetDS3Weapons().Single(w => w.WeaponId == weaponId);
                if (!weapon.CanInfuse)
                {
                    infusionTypes = infusionTypes.Where(i => i.InfusionId == 1);
                }
            }
            List<SelectListItem> infusionSelectList = infusionTypes
            .OrderBy(i => i.InfusionId)
            .Select(i => new SelectListItem()
            {
                Selected = i.InfusionId == infusionId,
                Text = i.Name,
                Value = i.InfusionId.ToString()
            })
            .ToList();
            return infusionSelectList;
        }

        public IEnumerable<SelectListItem> GetWeapons(int? weaponId = null, int weaponTypeId = 0)
        {
            IEnumerable<Weapon> weapons = _cache.GetDS3Weapons();
            List<SelectListItem> weaponSelectList = weapons
            .Where(w => weaponTypeId == 0 || w.WeaponTypeId == weaponTypeId)
            .OrderBy(w => w.Name)
            .Select(w => new SelectListItem()
            {
                Selected = (weaponId != null || w.WeaponId == weaponId),
                Text = w.Name,
                Value = w.WeaponId.ToString()
            })
            .ToList();
            return weaponSelectList;
        }

        public WeaponARModel CalculateAR(int weaponId, int infusionId, int upgradeLevel, int STR, int DEX, int INT, int FTH)
        {
            using (var lifetime = IoC.Container.BeginLifetimeScope())
            {
                using (var unit = lifetime.Resolve<IUnitOfWork>())
                {
                    SqlParameter[] parameters = new SqlParameter[]
                      {
                            new SqlParameter("@WeaponId", weaponId),
                            new SqlParameter("@InfusionId", infusionId),
                            new SqlParameter("@ReinforcementLevel", upgradeLevel),
                            new SqlParameter("@STR", STR),
                            new SqlParameter("@DEX", DEX),
                            new SqlParameter("@INT", INT),
                            new SqlParameter("@FTH", FTH)
                      };

                    string query = "Exec [DS3].[CalculateWeaponAR] @WeaponId, @InfusionId, @ReinforcementLevel, @STR, @DEX, @INT, @FTH";

                    WeaponARModel result = unit.SqlQuery<WeaponARModel>(query, parameters).FirstOrDefault();

                    return result;
                }
            }
        }
    }
}