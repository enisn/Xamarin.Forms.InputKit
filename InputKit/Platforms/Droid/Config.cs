using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.InputKit.Platforms.Droid
{
    public static class Config
    {
        public static void Init()
        {
            Console.WriteLine($"[{typeof(AutoCompleteViewRenderer).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(EmptyEntryRenderer).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(MenuEffect).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(NewIconViewRenderer).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(StackLayoutWithVisualStatesRenderer).FullName}] Initialized.");
        }
    }
}
