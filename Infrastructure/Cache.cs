using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Autofac;
using DataAccess;
//using Models;
using Model;
using Model.DS2;

namespace DarkSoulsII.Infrastructure
{
    public class Cache : ICache
    {
        private const string WeaponCategoriesCacheKey = "Global_WeaponCategories";
        private const string WeaponsCacheKey = "Global_Weapons";
        private const string InfusionsCacheKey = "Global_Infusions";

        public void Prime()
        {
            GetWeaponCategories();
        }

        public object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        /// <summary>
        /// Inserts a value into the cache for the specified key. Fails if one exists already.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependencies"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="slidingExpiration"></param>
        /// <param name="priority"></param>
        /// <param name="onRemoveCallback"></param>
        /// <returns></returns>
        public object Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            return HttpRuntime.Cache.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }

        /// <summary>
        /// Inserts a value into the cache for the specified key. Overwrites it if one exists already.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name=""></param>
        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback)
        {
            HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration, onUpdateCallback);
        }

        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration);
        }

        public object Remove(string key)
        {
            return HttpRuntime.Cache.Remove(key);
        }

        public IList<WeaponCategory> GetWeaponCategories(Func<WeaponCategory, bool> filter = null, bool forceUpdate = false)
        {
            var categories = Get(WeaponCategoriesCacheKey) as IList<WeaponCategory>;
            if (categories == null || forceUpdate)
            {
                using (var lifetime = IoC.Container.BeginLifetimeScope())
                {
                    using (var unit = lifetime.Resolve<IUnitOfWork>())
                    {
                        categories = unit.GetRepository<WeaponCategory>().Get().ToList();
                        Add(WeaponCategoriesCacheKey, categories, null, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    }
                }
            }

            if (filter != null)
            {
                return categories.Where(filter).ToList();
            }

            return categories;
        }

        public IList<DS2Weapon> GetWeapons(Func<DS2Weapon, bool> filter = null, bool forceUpdate = false)
        {
            var weapons = Get(WeaponsCacheKey) as IList<DS2Weapon>;
            if (weapons == null || forceUpdate)
            {
                using (var lifetime = IoC.Container.BeginLifetimeScope())
                {
                    using (var unit = lifetime.Resolve<IUnitOfWork>())
                    {
                        weapons = unit.GetRepository<DS2Weapon>().Get().ToList();
                        Add(WeaponsCacheKey, weapons, null, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    }
                }
            }

            if (filter != null)
            {
                return weapons.Where(filter).ToList();
            }

            return weapons;
        }

        public IList<Infusion> GetInfusions(Func<Infusion, bool> filter = null, bool forceUpdate = false)
        {
            var infusions = Get(InfusionsCacheKey) as IList<Infusion>;
            if (infusions == null || forceUpdate)
            {
                using (var lifetime = IoC.Container.BeginLifetimeScope())
                {
                    using (var unit = lifetime.Resolve<IUnitOfWork>())
                    {
                        infusions = unit.GetRepository<Infusion>().Get().OrderBy(i => i.DisplayOrder).ToList();
                        Add(InfusionsCacheKey, infusions, null, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    }
                }
            }

            if (filter != null)
            {
                return infusions.Where(filter).ToList();
            }

            return infusions;
        }
    }
}