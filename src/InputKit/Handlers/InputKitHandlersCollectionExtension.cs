using Microsoft.Maui.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputKit.Handlers
{
    public static class InputKitHandlersCollectionExtension
    {
        public static IMauiHandlersCollection AddInputKitHandlers(this IMauiHandlersCollection collection)
        {
            return collection
                .AddHandler(typeof(Shared.Controls.IconView), typeof(InputKit.Handlers.IconView.IconViewHandler));
        }
    }
}
