using Plugin.InputKit.Platforms.iOS.Controls;
using Plugin.InputKit.Platforms.iOS.Helpers;
using System;

namespace Plugin.InputKit.Platforms.iOS
{
    public static class Config
    {
        public static void Init()
        {
            Console.WriteLine($"[{typeof(AutoCompleteViewRenderer).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(EmptyEntryRenderer).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(MenuEffect).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(IconViewRenderer).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(StackLayoutWithVisualStatesRenderer).FullName}] Initialized.");

            Console.WriteLine($"[{typeof(AutoCompleteDefaultDataSource).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(AutoCompleteViewSource).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(Extensions).FullName}] Initialized.");

            Console.WriteLine($"[{typeof(AutoCompleteTableView).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(AutoCompleteTextField).FullName}] Initialized.");
        }
    }
}
