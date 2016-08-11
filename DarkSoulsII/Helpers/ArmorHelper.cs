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
    public class ArmorHelper
    {
        private readonly ICache _cache = IoC.Resolve<ICache>();

        public IEnumerable<ArmorViewModel> ArmorSearch(string searchValue = "",
                                                         int? armorId = null,
                                                         int armorTypeId = 0,
                                                         double? maxWeight = null,
                                                         double? minPoise = null)
        {
            using (var lifetime = IoC.Container.BeginLifetimeScope())
            {
                using (var unit = lifetime.Resolve<IUnitOfWork>())
                {
                    SqlParameter[] parameters = new SqlParameter[]
                       {
                            new SqlParameter("@SearchValue", string.IsNullOrWhiteSpace(searchValue) ? SqlString.Null : searchValue),
                            new SqlParameter("@ArmorId", armorId ?? SqlInt32.Null),
                            new SqlParameter("@ArmorTypeId", armorTypeId),
                            new SqlParameter("@MaxWeight", maxWeight ?? SqlDouble.Null),
                            new SqlParameter("@MinPoise", minPoise ?? SqlDouble.Null)
                       };

                    string query = "Exec [dbo].[ArmorSearch] @SearchValue, @ArmorId, @ArmorTypeId, @MaxWeight, @MinPoise";

                    IEnumerable<ArmorViewModel> armors = unit.SqlQuery<ArmorViewModel>(query, parameters).ToList();
                    
                    return armors;
                }
            }
        }
        
        public IEnumerable<SelectListItem> GetArmorTypes(int armorTypeId = 0)
        {
            IEnumerable<DS2ArmorType> armorTypes = _cache.GetArmorTypes();
            List<SelectListItem> armorTypeSelectList = new List<SelectListItem>();
            armorTypeSelectList.Add(new SelectListItem() { Text = "All", Value = "0", Selected = armorTypeId == 0 });
            armorTypeSelectList.AddRange(armorTypes
            .OrderBy(at => at.Name)
            .Select(at => new SelectListItem()
            {
                Selected = at.ArmorTypeId == armorTypeId,
                Text = at.Name,
                Value = at.ArmorTypeId.ToString()
            })
            .ToList());
            return armorTypeSelectList;
        }

        public IEnumerable<SelectListItem> GetArmor(int? armorId = null, int armorTypeId = 0)
        {
            IEnumerable<DS2Armor> armors = _cache.GetArmor();
            List<SelectListItem> armorSelectList = armors
            .Where(a => armorTypeId == 0 || a.ArmorTypeId == armorTypeId)
            .OrderBy(a => a.Name)
            .Select(a => new SelectListItem()
            {
                Selected = (armorId != null || a.ArmorId == armorId),
                Text = a.Name,
                Value = a.ArmorId.ToString()
            })
            .ToList();
            return armorSelectList;
        }

    }
}