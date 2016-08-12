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
    public class RingsHelper
    {
        private readonly ICache _cache = IoC.Resolve<ICache>();

        public IEnumerable<SelectListItem> GetRings(int? ringId = null)
        {
            IEnumerable<Ring> rings = _cache.GetRings();
            List<SelectListItem> ringSelectList = new List<SelectListItem>();
            ringSelectList.Add(new SelectListItem() { Text = "None", Value = "0", Selected = ringId == null });
            ringSelectList.AddRange(rings
            .Where(r => ringId == null || r.RingId == ringId)
            .OrderBy(a => a.Name)
            .Select(a => new SelectListItem()
            {
                Selected = (ringId != null || a.RingId == ringId),
                Text = a.Name,
                Value = a.RingId.ToString()
            })
            .ToList());
            return ringSelectList;
        }

        public Ring GetRing(int ringId)
        {
            return _cache.GetRings(r => r.RingId == ringId).First();
        }
    }
}