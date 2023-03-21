using RGBKit.Core;

namespace RGBKit.Providers.NZXT
{
    /// <summary>
    /// The NZXT RGB Kit extension methods
    /// </summary>
    public static class NZXTDeviceProviderExtensions
    {
        /// <summary>
        /// Uses the NZXT device provider
        /// </summary>
        /// <param name="rgbKit">The RGB Kit instance to add the provider to</param>
        public static void UseNZXT(this IRGBKitService rgbKit)
        {
            rgbKit.AddProvider(new NZXTDeviceProvider());
        }
    }
}
