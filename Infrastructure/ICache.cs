using System;
using System.Collections.Generic;
using System.Web.Caching;
//using Models;
using Model.DS2;

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

        IList<WeaponCategory> GetWeaponCategories(Func<WeaponCategory, bool> filter = null, bool forceUpdate = false);
        IList<DS2Weapon> GetWeapons(Func<DS2Weapon, bool> filter = null, bool forceUpdate = false);
        IList<Infusion> GetInfusions(Func<Infusion, bool> filter = null, bool forceUpdate = false);
    }
}
