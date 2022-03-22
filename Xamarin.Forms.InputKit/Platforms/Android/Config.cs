using Android.App;
using Android.OS;
using System;

namespace Plugin.InputKit.Platforms.Droid
{
    public static class Config
    {
        private static Activity currentActivity;
        private static Bundle currentBundle;

        public static Activity CurrentActivity
        {
            get => currentActivity ?? throw new NotImplementedException("Android wasn't initialized! Make sure your MainActivity > OnCreate() contains Plugin.InputKit.Platforms.Droid.Config.Init(this, savedInstanceState)");
            private set => currentActivity = value;
        }

        public static Bundle CurrentBundle
        {
            get => currentBundle ?? throw new NotImplementedException("Android wasn't initialized! Make sure your MainActivity > OnCreate() contains Plugin.InputKit.Platforms.Droid.Config.Init(this, savedInstanceState)");
            set => currentBundle = value;
        }

        public static void Init(Activity activity, Bundle bundle)
        {
            CurrentActivity = activity;
            CurrentBundle = bundle;
            Console.WriteLine($"[{typeof(AutoCompleteViewRenderer).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(EmptyEntryRenderer).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(MenuEffect).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(NewIconViewRenderer).FullName}] Initialized.");
            Console.WriteLine($"[{typeof(StatefulStackLayoutRenderer).FullName}] Initialized.");
        }
    }
}
