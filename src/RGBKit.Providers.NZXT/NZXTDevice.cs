using System;
using RGBKit.Core;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Drawing;

using NZXTSharp;
using NZXTSharp.KrakenX;
using NZXTSharp.Exceptions;

using Theraot.Collections;
using NZXTSharp.COM;
using System.Diagnostics;

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

        private bool ranOnce = false;

        /// <summary>
        /// The lights the device has
        /// </summary>
        public IEnumerable<IDeviceLight> Lights { get => _lights; }

        /// <summary>
        /// The device
        /// </summary>
        private KrakenX _device;

        /// <summary>
        /// The lights the device has
        /// </summary>
        private List<NZXTDeviceLight> _lights;

        /// <summary>
        /// Creates an NZXT KrakenX device
        /// </summary>
        /// <param name="device">The device</param>
        internal NZXTDevice(KrakenX device)
        {
            _device = device;
            Name = _device.DeviceID.ToString();
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
            _device.WriteCustom(buffer);
        }
    }
}
