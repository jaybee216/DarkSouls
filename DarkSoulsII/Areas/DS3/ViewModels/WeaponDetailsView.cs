using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarkSoulsII.Areas.DS3.ViewModels
{
    public class WeaponDetailsView
    {
        public WeaponViewModel Normal { get; set; }
        public List<WeaponViewModel> Infusions { get; set; }
    }
}