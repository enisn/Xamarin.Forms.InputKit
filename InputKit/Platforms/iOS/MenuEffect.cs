using Plugin.InputKit.Platforms.iOS;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using static Plugin.InputKit.Shared.Utils.PopupMenu;

[assembly: ResolutionGroupName("Plugin.InputKit.Platforms")]
[assembly: ExportEffect(typeof(MenuEffect), "MenuEffect")]
namespace Plugin.InputKit.Platforms.iOS
{
    public class MenuEffect : PlatformEffect
    {
        public static string UIAlertControllerCancelText { get; set; } = "Cancel";
        public static UIPopoverArrowDirection PermittedArrowDirections = UIPopoverArrowDirection.Up;

        InternalPopupEffect Effect;
        UIViewController RootViewController;


        protected override void OnAttached()
        {
            Effect = (InternalPopupEffect)Element.Effects.FirstOrDefault(e => e is InternalPopupEffect);

            if (Effect != null)
                Effect.Parent.OnPopupRequest += OnPopupRequest;
        }

        void OnPopupRequest(View view)
        {
            if (Effect.Parent.ItemsSource == null)
                return;

            RootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            
            UIAlertController actionSheetAlert = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);

            int i = 0;
            foreach (var item in Effect.Parent.ItemsSource)
            {
                actionSheetAlert.AddAction(UIAlertAction.Create(item.ToString(), UIAlertActionStyle.Default, (action) => Effect.Parent.InvokeItemSelected(item.ToString(),i)));
                i++;
            }

            actionSheetAlert.AddAction(UIAlertAction.Create(UIAlertControllerCancelText, UIAlertActionStyle.Destructive, null));

            // Required for iPad - You must specify a source for the Action Sheet since it is
            // displayed as a popover
            if (Device.Idiom != TargetIdiom.Phone)
            {
                UIPopoverPresentationController presentationPopover = actionSheetAlert.PopoverPresentationController;
                if (presentationPopover != null)
                {
                    if (Control != null)
                    {
                        presentationPopover.SourceRect = Control.Frame;
                        presentationPopover.SourceView = Control;
                    }

                    else if (Container != null)
                    {
                        presentationPopover.SourceRect = Container.Frame;
                        presentationPopover.SourceView = Container;
                    }

                    else
                    {
                        presentationPopover.SourceRect = RootViewController.View.Frame;
                        presentationPopover.SourceView = RootViewController.View;
                    }

                    presentationPopover.PermittedArrowDirections = PermittedArrowDirections;
                }
            }

            RootViewController.PresentViewController(actionSheetAlert, true, null);
        }

        protected override void OnDetached()
        {
            if (Effect != null)
                Effect.Parent.OnPopupRequest -= OnPopupRequest;
        }
    }
}
