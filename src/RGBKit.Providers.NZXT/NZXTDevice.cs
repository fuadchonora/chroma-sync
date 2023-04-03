using System.Linq;
using System.Collections.Generic;

using RGBKit.Core;
using HidLibrary;

namespace RGBKit.Providers.NZXT
{
    /// <summary>
    /// An Aura device
    /// </summary>
    class NZXTDevice : IDevice
    {
        /// <summary>
        /// The device name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The lights the device has
        /// </summary>
        public IEnumerable<IDeviceLight> Lights { get => _lights; }

        /// <summary>
        /// The device
        /// </summary>
        private HidDevice _device;

        /// <summary>
        /// The lights the device has
        /// </summary>
        private List<NZXTDeviceLight> _lights;

        /// <summary>
        /// Creates an NZXT HidDevice device
        /// </summary>
        /// <param name="device">The device</param>
        internal NZXTDevice(string devicePath)
        {
            _device = HidDevices.Enumerate(devicePath).FirstOrDefault();
           
            _lights = new List<NZXTDeviceLight>();

            for (int i = 0; i < 40; i++)
            {
                _lights.Add(new NZXTDeviceLight());
            }
        }

        /// <summary>
        /// Applies light changes to the device
        /// </summary>
        public void ApplyLights()
        {
            //Sync All LEDs
            byte[] header = { 42, 4, 7, 7, 0, 50, 0 };
            byte[] colors = { _lights[0].Color.G, _lights[0].Color.R, _lights[0].Color.B, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] footer = { 0, 1, 0, 40, 3 };
            
            byte[] buffer = header.Concat(colors).ToArray().Concat(footer).ToArray();
            _ = _device.WriteAsync(buffer);
        }
    }
}
