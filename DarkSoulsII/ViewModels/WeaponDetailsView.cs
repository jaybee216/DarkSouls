using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarkSoulsII.ViewModels
{
    public class WeaponDetailsView
    {
        public WeaponViewModel Normal { get; set; }
        public List<WeaponViewModel> Infusions { get; set; }
    }
}