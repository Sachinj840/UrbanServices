using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintBookApp.Effects
{
    public class ShowHidePassEffect : RoutingEffect
    {
        public string EntryText { get; set; }
        public ShowHidePassEffect() : base("Xamarin.ShowHidePassEffect") { }
    }
}
