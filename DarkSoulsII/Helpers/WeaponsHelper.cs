using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccess;
using DarkSoulsII.Infrastructure;
using Autofac;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Model.DS2;
using DarkSoulsII.ViewModels;

namespace DarkSoulsII.Helpers
{
    public class WeaponsHelper
    {
        private readonly ICache _cache = IoC.Resolve<ICache>();

        public IEnumerable<WeaponViewModel> WeaponSearch(string searchValue = "",
                                                         int? weaponId = null,
                                                         int weaponCategoryId = 0,
                                                         int? infusionId = null,
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
                            new SqlParameter("@WeaponCategoryId", weaponCategoryId),
                            new SqlParameter("@InfusionId", infusionId ?? SqlInt32.Null),
                            new SqlParameter("@MaxWeight", weight ?? SqlDouble.Null),
                            new SqlParameter("@STR", STR ?? SqlInt32.Null),
                            new SqlParameter("@DEX", DEX ?? SqlInt32.Null),
                            new SqlParameter("@INT", INT ?? SqlInt32.Null),
                            new SqlParameter("@FTH", FTH ?? SqlInt32.Null)
                       };

                    string query = "Exec [dbo].[WeaponSearch] @SearchValue, @WeaponId, @WeaponCategoryId, @InfusionId, @MaxWeight, @STR, @DEX, @INT, @FTH";

                    IEnumerable<WeaponViewModel> weapons = unit.SqlQuery<WeaponViewModel>(query, parameters).ToList();
                    
                    return weapons;
                }
            }
        }

        public IEnumerable<SelectListItem> GetWeaponTypes(int weaponCategoryId = 0)
        {
            IEnumerable<WeaponCategory> weaponTypes = _cache.GetWeaponCategories();
            List<SelectListItem> weaponTypeSelectList = new List<SelectListItem>();
            weaponTypeSelectList.Add(new SelectListItem() { Text = "All", Value = "0", Selected = weaponCategoryId == 0 });
            weaponTypeSelectList.AddRange(weaponTypes
            .OrderBy(wt => wt.Name)
            .Select(wt => new SelectListItem()
            {
                Selected = wt.WeaponCategoryId == weaponCategoryId,
                Text = wt.Name,
                Value = wt.WeaponCategoryId.ToString()
            })
            .ToList());
            return weaponTypeSelectList;
        }

        public IEnumerable<SelectListItem> GetInfusionTypes(int infusionId = 6, int? weaponId = null)
        {
            IEnumerable<Infusion> infusionTypes = _cache.GetInfusions();
            //if (weaponId != null)
            //{
            //    DS2Weapon weapon = _cache.GetWeapons().Single(w => w.WeaponId == weaponId);
            //    if (!weapon.CanInfuse)
            //    {
            //        infusionTypes = infusionTypes.Where(i => i.InfusionId == 1);
            //    }
            //}
            List<SelectListItem> infusionSelectList = infusionTypes
            .OrderBy(i => i.DisplayOrder)
            .Select(i => new SelectListItem()
            {
                Selected = i.InfusionId == infusionId,
                Text = i.Name,
                Value = i.InfusionId.ToString()
            })
            .ToList();
            return infusionSelectList;
        }

        public IEnumerable<SelectListItem> GetWeapons(int? weaponId = null, int weaponCategoryId = 0)
        {
            IEnumerable<DS2Weapon> weapons = _cache.GetWeapons();
            List<SelectListItem> weaponSelectList = weapons
            .Where(w => weaponCategoryId == 0 || w.WeaponCategoryId == weaponCategoryId)
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

        public WeaponARModel CalculateAR(int weaponId, int infusionId, int STR, int DEX, int INT, int FTH)
        {
            using (var lifetime = IoC.Container.BeginLifetimeScope())
            {
                using (var unit = lifetime.Resolve<IUnitOfWork>())
                {
                    WeaponARModel result;

                    DS2Weapon weapon = unit.GetRepository<DS2Weapon>().GetById(weaponId);

                    if (weapon.StrReq > STR || weapon.DexReq > DEX || weapon.IntReq > INT || weapon.FthReq > FTH)
                    {
                        result = new WeaponARModel()
                        {
                            RequirementsMet = false,
                            StrReq = weapon.StrReq,
                            DexReq = weapon.DexReq,
                            IntReq = weapon.IntReq,
                            FthReq = weapon.FthReq
                        };
                    }
                    else
                    {
                        
                        SqlParameter[] parameters = new SqlParameter[]
                          {
                            new SqlParameter("@WeaponId", weaponId),
                            new SqlParameter("@InfusionId", infusionId),
                            new SqlParameter("@STR", STR),
                            new SqlParameter("@DEX", DEX),
                            new SqlParameter("@INT", INT),
                            new SqlParameter("@FTH", FTH)
                          };

                        string query = "Exec [dbo].[CalculateWeaponAR] @WeaponId, @InfusionId, @STR, @DEX, @INT, @FTH";
                        result = unit.SqlQuery<WeaponARModel>(query, parameters).FirstOrDefault();
                        result.RequirementsMet = true;
                    }
                    return result;
                }
            }
        }
    }
}