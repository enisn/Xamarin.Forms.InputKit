using Microsoft.Maui.Controls;
using System.Collections;
using System.Linq;

namespace InputKit.Shared.Utils;

internal class PopupMenu : BindableObject
{

    public delegate void PopupShowRequestDelegate(View view);
    public event PopupShowRequestDelegate OnPopupRequest;

    public delegate void ItemSelectedDelegate(string item, int index);
    public event ItemSelectedDelegate OnItemSelected;


    #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(PopupMenu), default(IList));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    #endregion

    //---------------------------------------------------------------------------------------------------
    /// <summary>
    /// Items to display.
    /// </summary>
    public IList ItemsSource
    {
        get { return (IList)GetValue(ItemsSourceProperty); }
        set { SetValue(ItemsSourceProperty, value); }
    }
    public InternalPopupEffect InternalEffect { get; private set; }

    //---------------------------------------------------------------------------------------------------
    /// <summary>
    /// Default constructor
    /// </summary>
    public PopupMenu()
    {
        InternalEffect = new InternalPopupEffect(this);
    }

    #region methods
    public void ShowPopup(View sender)
    {
        var effects = sender.Effects.Where(c => c is InternalPopupEffect).ToList();

        // Remove all old popups
        if (effects.Count > 0 && effects[0] != InternalEffect)
            foreach (var effect in effects)
                sender.Effects.Remove(effect);

        // Add new popup
        sender.Effects.Add(InternalEffect);

        // Invoke
        OnPopupRequest?.Invoke(sender);
    }

    public void InvokeItemSelected(string item, int index) => OnItemSelected?.Invoke(item, index);
    #endregion

    #region classes
    /// <summary>
    /// INTERNAL USE ONLY.
    /// This is used by the Popup Menu as routing effects can not normally be bound, this provides the routing effect whereas the PopupMenu provides the bindable.
    /// </summary>
    public sealed class InternalPopupEffect : RoutingEffect
    {
        public PopupMenu Parent;
        internal InternalPopupEffect(PopupMenu menu) : base("Plugin.InputKit.Platforms.MenuEffect")
        {
            Parent = menu;
        }
    }
    #endregion
}
