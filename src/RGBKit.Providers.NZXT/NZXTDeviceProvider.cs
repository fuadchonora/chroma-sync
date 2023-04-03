using System.Threading;
using System.Collections.Generic;
using RGBKit.Core;

namespace RGBKit.Providers.NZXT
{
    /// <summary>
    /// The Aura device provider
    /// </summary>
    class NZXTDeviceProvider : IDeviceProvider
    {
        /// <summary>
        /// The provider name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The devices the provider has
        /// </summary>
        public IEnumerable<IDevice> Devices { get => _devices; }

        /// <summary>
        /// The devices the provider has
        /// </summary>
        private List<NZXTDevice> _devices;

        /// <summary>
        /// If the provider is in exclusive mode
        /// </summary>
        private bool inExcluseMode;

        /// <summary>
        /// Creates an NZXT device provider
        /// </summary>
        public NZXTDeviceProvider()
        {
            Name = "NZXT";
            _devices = new List<NZXTDevice>();

            inExcluseMode = false;
        }

        /// <summary>
        /// Initializes the provider
        /// </summary>
        public void Initialize()
        {
            PerformHealthCheck();

            // Add KrakenX3 Device
            string devicePath = "\\\\?\\hid#vid_1e71&pid_2007#7&1f09b849&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}";
            _devices.Add(new NZXTDevice(devicePath));
        }

        /// <summary>
        /// Performs a health check on the provider
        /// </summary>
        public void PerformHealthCheck()
        {
            ///
            /// TO-DO Using NZXTSharp
            ///
        }

        /// <summary>
        /// Requests exclusive control over the provider
        /// </summary>
        public void RequestControl()
        {
            if (!inExcluseMode)
            {
                ///
                /// TO-DO
                ///
            }
        }

        /// <summary>
        /// Releases exclusive control over the provider
        /// </summary>
        public void ReleaseControl()
        {
            if (inExcluseMode)
            {
                ///
                /// TO-DO
                ///
            }
        }
    }
}
