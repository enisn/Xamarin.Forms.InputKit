using System;

namespace Plugin.InputKit
{
    /// <summary>
    /// Cross InputKit
    /// </summary>
    public static class CrossInputKit
    {
        static Lazy<IInputKit> implementation = new Lazy<IInputKit>(() => CreateInputKit(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => implementation.Value == null ? false : true;

        /// <summary>
        /// Current plugin implementation to use
        /// </summary>
        public static IInputKit Current
        {
            get
            {
                IInputKit ret = implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IInputKit CreateInputKit()
        {
            return null;
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");

    }
}
