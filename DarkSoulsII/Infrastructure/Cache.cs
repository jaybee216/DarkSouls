using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Autofac;
using DataAccess;

using DS2 = Model.DS2;
using DS3 = Model.DS3;

namespace DarkSoulsII.Infrastructure
{
    public class Cache : ICache
    {
        private const string WeaponCategoriesCacheKey = "Global_WeaponCategories";
        private const string WeaponsCacheKey = "Global_Weapons";
        private const string InfusionsCacheKey = "Global_Infusions";

        private const string DS3WeaponTypesCacheKey = "Global_DS3_WeaponTypes";
        private const string DS3WeaponsCacheKey = "Global_DS3_Weapons";
        private const string DS3ArmorTypesCacheKey = "Global_DS3_ArmorTypes";
        private const string DS3ArmorCacheKey = "Global_DS3_Armor";
        private const string DS3InfusionsCacheKey = "Global_DS3_Infusions";
        private const string DS3StartingClassesCacheKey = "Global_DS3_StartingClasses";

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

        public IList<DS2.WeaponCategory> GetWeaponCategories(Func<DS2.WeaponCategory, bool> filter = null, bool forceUpdate = false)
        {
            var categories = Get(WeaponCategoriesCacheKey) as IList<DS2.WeaponCategory>;
            if (categories == null || forceUpdate)
            {
                using (var lifetime = IoC.Container.BeginLifetimeScope())
                {
                    using (var unit = lifetime.Resolve<IUnitOfWork>())
                    {
                        categories = unit.GetRepository<DS2.WeaponCategory>().Get().ToList();
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

        public IList<DS2.DS2Weapon> GetWeapons(Func<DS2.DS2Weapon, bool> filter = null, bool forceUpdate = false)
        {
            var weapons = Get(WeaponsCacheKey) as IList<DS2.DS2Weapon>;
            if (weapons == null || forceUpdate)
            {
                using (var lifetime = IoC.Container.BeginLifetimeScope())
                {
                    using (var unit = lifetime.Resolve<IUnitOfWork>())
                    {
                        weapons = unit.GetRepository<DS2.DS2Weapon>().Get().ToList();
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

        public IList<DS2.Infusion> GetInfusions(Func<DS2.Infusion, bool> filter = null, bool forceUpdate = false)
        {
            var infusions = Get(InfusionsCacheKey) as IList<DS2.Infusion>;
            if (infusions == null || forceUpdate)
            {
                using (var lifetime = IoC.Container.BeginLifetimeScope())
                {
                    using (var unit = lifetime.Resolve<IUnitOfWork>())
                    {
                        infusions = unit.GetRepository<DS2.Infusion>().Get().OrderBy(i => i.DisplayOrder).ToList();
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

        public IList<DS3.WeaponType> GetDS3WeaponTypes(Func<DS3.WeaponType, bool> filter = null, bool forceUpdate = false)
        {
            var categories = Get(DS3WeaponTypesCacheKey) as IList<DS3.WeaponType>;
            if (categories == null || forceUpdate)
            {
                using (var lifetime = IoC.Container.BeginLifetimeScope())
                {
                    using (var unit = lifetime.Resolve<IUnitOfWork>())
                    {
                        categories = unit.GetRepository<DS3.WeaponType>().Get().ToList();
                        Add(DS3WeaponTypesCacheKey, categories, null, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    }
                }
            }

            if (filter != null)
            {
                return categories.Where(filter).ToList();
            }

            return categories;
        }

        public IList<DS3.Weapon> GetDS3Weapons(Func<DS3.Weapon, bool> filter = null, bool forceUpdate = false)
        {
            var weapons = Get(DS3WeaponsCacheKey) as IList<DS3.Weapon>;
            if (weapons == null || forceUpdate)
            {
                using (var lifetime = IoC.Container.BeginLifetimeScope())
                {
                    using (var unit = lifetime.Resolve<IUnitOfWork>())
                    {
                        weapons = unit.GetRepository<DS3.Weapon>().Get().ToList();
                        Add(DS3WeaponsCacheKey, weapons, null, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    }
                }
            }

            if (filter != null)
            {
                return weapons.Where(filter).ToList();
            }

            return weapons;
        }

        public IList<DS3.ArmorType> GetDS3ArmorTypes(Func<DS3.ArmorType, bool> filter = null, bool forceUpdate = false)
        {
            var categories = Get(DS3ArmorTypesCacheKey) as IList<DS3.ArmorType>;
            if (categories == null || forceUpdate)
            {
                using (var lifetime = IoC.Container.BeginLifetimeScope())
                {
                    using (var unit = lifetime.Resolve<IUnitOfWork>())
                    {
                        categories = unit.GetRepository<DS3.ArmorType>().Get().ToList();
                        Add(DS3WeaponTypesCacheKey, categories, null, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    }
                }
            }

            if (filter != null)
            {
                return categories.Where(filter).ToList();
            }

            return categories;
        }

        public IList<DS3.Armor> GetDS3Armor(Func<DS3.Armor, bool> filter = null, bool forceUpdate = false)
        {
            var armors = Get(DS3ArmorCacheKey) as IList<DS3.Armor>;
            if (armors == null || forceUpdate)
            {
                using (var lifetime = IoC.Container.BeginLifetimeScope())
                {
                    using (var unit = lifetime.Resolve<IUnitOfWork>())
                    {
                        armors = unit.GetRepository<DS3.Armor>().Get().ToList();
                        Add(DS3ArmorCacheKey, armors, null, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    }
                }
            }

            if (filter != null)
            {
                return armors.Where(filter).ToList();
            }

            return armors;
        }

        public IList<DS3.InfusionType> GetDS3Infusions(Func<DS3.InfusionType, bool> filter = null, bool forceUpdate = false)
        {
            var infusions = Get(DS3InfusionsCacheKey) as IList<DS3.InfusionType>;
            if (infusions == null || forceUpdate)
            {
                using (var lifetime = IoC.Container.BeginLifetimeScope())
                {
                    using (var unit = lifetime.Resolve<IUnitOfWork>())
                    {
                        infusions = unit.GetRepository<DS3.InfusionType>().Get().OrderBy(i => i.InfusionId).ToList();
                        Add(DS3InfusionsCacheKey, infusions, null, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    }
                }
            }

            if (filter != null)
            {
                return infusions.Where(filter).ToList();
            }

            return infusions;
        }

        public IList<DS3.StartingClass> GetDS3StartingClasses(Func<DS3.StartingClass, bool> filter = null, bool forceUpdate = false)
        {
            var classes = Get(DS3StartingClassesCacheKey) as IList<DS3.StartingClass>;
            if (classes == null || forceUpdate)
            {
                using (var lifetime = IoC.Container.BeginLifetimeScope())
                {
                    using (var unit = lifetime.Resolve<IUnitOfWork>())
                    {
                        classes = unit.GetRepository<DS3.StartingClass>().Get().OrderBy(c => c.StartingClassId).ToList();
                        Add(DS3StartingClassesCacheKey, classes, null, DateTime.Now.AddHours(6), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    }
                }
            }

            if (filter != null)
            {
                return classes.Where(filter).ToList();
            }

            return classes;
        }
    }
}