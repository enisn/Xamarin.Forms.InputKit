using System;
using System.Collections.Generic;
using System.Text;

namespace Plugin.InputKit.Platforms.UWP
{
    public static class Config
    {
        public static void Init()
        {
            Console.WriteLine($"[{typeof(EmptyEntryRenderer).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(IconViewRenderer).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(InputKitImplementation).FullName}] Initialized.");
        }
    }
}
