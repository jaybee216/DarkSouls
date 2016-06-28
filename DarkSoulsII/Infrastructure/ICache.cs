using System;
using System.Collections.Generic;
using System.Web.Caching;

namespace DarkSoulsII.Infrastructure
{
    interface ICache
    {
        void Prime();
        object Get(string key);
        object Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback);
        void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback);
        void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration);
        object Remove(string key);

        IList<Model.DS2.WeaponCategory> GetWeaponCategories(Func<Model.DS2.WeaponCategory, bool> filter = null, bool forceUpdate = false);
        IList<Model.DS2.DS2Weapon> GetWeapons(Func<Model.DS2.DS2Weapon, bool> filter = null, bool forceUpdate = false);
        IList<Model.DS2.Infusion> GetInfusions(Func<Model.DS2.Infusion, bool> filter = null, bool forceUpdate = false);

        IList<Model.DS3.WeaponType> GetDS3WeaponTypes(Func<Model.DS3.WeaponType, bool> filter = null, bool forceUpdate = false);
        IList<Model.DS3.Weapon> GetDS3Weapons(Func<Model.DS3.Weapon, bool> filter = null, bool forceUpdate = false);
        IList<Model.DS3.ArmorType> GetDS3ArmorTypes(Func<Model.DS3.ArmorType, bool> filter = null, bool forceUpdate = false);
        IList<Model.DS3.Armor> GetDS3Armor(Func<Model.DS3.Armor, bool> filter = null, bool forceUpdate = false);
        IList<Model.DS3.InfusionType> GetDS3Infusions(Func<Model.DS3.InfusionType, bool> filter = null, bool forceUpdate = false);
        IList<Model.DS3.StartingClass> GetDS3StartingClasses(Func<Model.DS3.StartingClass, bool> filter = null, bool forceUpdate = false);
    }
}
