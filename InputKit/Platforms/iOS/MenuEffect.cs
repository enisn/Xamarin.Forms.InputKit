using Plugin.InputKit.Platforms.iOS;
using System;
using System.Collections.Generic;
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

            RootViewController = GetVisibleViewController();

            UIAlertController actionSheetAlert = UIAlertController.Create(null, null, UIAlertControllerStyle.Alert);
            var _enumerator = Effect.Parent.ItemsSource.GetEnumerator();
            while (_enumerator.MoveNext())
            {
                UIAlertAction alertAction = UIAlertAction.Create(_enumerator.Current?.ToString(), UIAlertActionStyle.Default, (action) =>
                {
                    int index = actionSheetAlert.Actions.ToList<UIAlertAction>().IndexOf(action);
                    Effect.Parent.InvokeItemSelected(action.Title, index);
                });
                actionSheetAlert.AddAction(alertAction);
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

        UIViewController GetVisibleViewController()
        {
            var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (rootController.PresentedViewController == null)
                return rootController;

            if (rootController.PresentedViewController is UINavigationController)
            {
                return ((UINavigationController)rootController.PresentedViewController).VisibleViewController;
            }

            if (rootController.PresentedViewController is UITabBarController)
            {
                return ((UITabBarController)rootController.PresentedViewController).SelectedViewController;
            }

            return rootController.PresentedViewController;
        }
    }
}
