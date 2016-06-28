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
    }
}