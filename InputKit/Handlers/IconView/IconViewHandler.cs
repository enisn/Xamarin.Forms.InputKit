using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Plugin.InputKit.Shared.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.InputKit.Handlers.IconView
{
    public partial class IconViewHandler
    {
        public static PropertyMapper<IIConView, IconViewHandler> IconViewMapper = new PropertyMapper<IIConView, IconViewHandler>(ViewHandler.ViewMapper)
        {
            [nameof(IIConView.Source)] = MapSource,
            [nameof(IIConView.FillColor)] = MapFillColor,
        };

        public IconViewHandler() : base(IconViewMapper)
        {

        }

        public IconViewHandler(PropertyMapper mapper = null) : base(mapper ?? IconViewMapper)
        {

        }
    }
}
