using InputKit.Shared.Layouts;
using Plainer.Maui;

namespace InputKit.Handlers
{
    public static class InputKitHandlersCollectionExtension
    {
        public static IMauiHandlersCollection AddInputKitHandlers(this IMauiHandlersCollection collection)
        {
            return collection
                .AddPlainer()
                .AddHandler(typeof(Shared.Controls.IconView), typeof(InputKit.Handlers.IconView.IconViewHandler))
                .AddHandler(typeof(StatefulStackLayout), typeof(InputKit.Handlers.StatefulStackLayoutHandler));
        }
    }
}
