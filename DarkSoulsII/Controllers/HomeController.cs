using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
//using Models;
using Model;
using Model.DS2;
using Autofac;

namespace DarkSoulsII.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //using (var lifetime = IoC.Container.BeginLifetimeScope())
            //{
            //    using (var unit = lifetime.Resolve<IUnitOfWork>())
            //    {
            //        IRepository<Weapon> weaponRepository = unit.GetRepository<Weapon>();
            //        IRepository<WeaponCategory> weaponCategoryRepo = unit.GetRepository<WeaponCategory>();
            //        WeaponCategory greatHammerCategory = weaponCategoryRepo.Get(c => c.Name.Equals("Great Hammer")).FirstOrDefault();
            //        if (greatHammerCategory == null)
            //        {
            //            greatHammerCategory = new WeaponCategory() { Name = "Great Hammer" };
            //            weaponCategoryRepo.Insert(greatHammerCategory);
            //            unit.Save();
            //        }
            //        weaponRepository.Insert(new Weapon() { Name = "Test Great Hammer 1", Category = greatHammerCategory });
            //        weaponRepository.Insert(new Weapon() { Name = "Test Great Hammer 2", Category = greatHammerCategory });
            //        unit.Save();
            //        IEnumerable<Weapon> greatHammers = weaponRepository.Get(w => w.Category.Name.Equals("Great Hammer"));
            //    }
            //}
                return View();
        }

        public ActionResult MyCharacters()
        {
            return View();
        }
    }
}
