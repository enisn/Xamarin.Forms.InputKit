﻿using Android.Content;
using Android.OS;
#if MONOANDROID10_0 || MONOANDROID11_0 || MONOANDROID12_0
using AndroidX.AppCompat.Widget;
#else
using Android.Support.V7.Widget;
#endif
using Plugin.InputKit.Platforms.Droid;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Plugin.InputKit.Shared.Utils.PopupMenu;

[assembly: ResolutionGroupName("Plugin.InputKit.Platforms")]
[assembly: ExportEffect(typeof(MenuEffect), nameof(MenuEffect))]
namespace Plugin.InputKit.Platforms.Droid
{
    public class MenuEffect : PlatformEffect
    {
        PopupMenu ToggleMenu;
        InternalPopupEffect Effect;
        
        public MenuEffect()
        {
        }
        
        protected override void OnAttached()
        {
            Effect = (InternalPopupEffect)Element.Effects.FirstOrDefault(e => e is InternalPopupEffect);

            if (Effect != null)
                Effect.Parent.OnPopupRequest += OnPopupRequest;
            
            Context context = Config.CurrentActivity;
#if MONOANDROID10_0 || MONOANDROID11_0 || MONOANDROID12_0
            Context wrapper = new Android.Views.ContextThemeWrapper(context, Resource.Style.MyPopupMenu);
#else
            Context wrapper = new Android.Support.V7.View.ContextThemeWrapper(context, Resource.Style.MyPopupMenu);
#endif

            if (Control != null)
            {
                ToggleMenu = new PopupMenu(wrapper, Control);
            }
            else if (Container != null)
            {
                ToggleMenu = new PopupMenu(wrapper, Container);
            }
            ToggleMenu.Gravity = (int)Android.Views.GravityFlags.Right;
            ToggleMenu.MenuItemClick += MenuItemClick;
        }

        void OnPopupRequest(View view)
        {
            if (Effect.Parent.ItemsSource == null)
                return;

            ToggleMenu.Menu.Clear();

            for (int i = 0; i < Effect.Parent.ItemsSource.Count; i++)
            {
                ToggleMenu.Menu.Add(0, i, 0, new Java.Lang.String(Effect.Parent.ItemsSource[i] != null ? Effect.Parent.ItemsSource[i].ToString() : ""));
            }
            ToggleMenu.Show();
        }

        protected override void OnDetached()
        {
            if (ToggleMenu != null)
                ToggleMenu.MenuItemClick -= MenuItemClick;

            if (Effect != null)
                Effect.Parent.OnPopupRequest -= OnPopupRequest;
        }

        void MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
            => Effect?.Parent.InvokeItemSelected(e.Item.ToString(), e.Item.ItemId);
    }
}
