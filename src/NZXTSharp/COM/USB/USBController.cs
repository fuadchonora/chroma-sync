/*
USBController.cs
Copyright (C) 2019  Ari Madian

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

using HidLibrary;

using NZXTSharp.Exceptions;
using System.Threading.Tasks;

namespace NZXTSharp.COM {
    public class USBController
    { 

        private NZXTDeviceType _Type;
        private HIDDeviceID _ID;
        private int CurrProductID;
        private readonly HIDDeviceID _VendorID = HIDDeviceID.VendorID;
        private HidReport _LastReport;
        private bool _IsAttached = false;
        private HidDevice _Device;

        /// <summary>
        /// The type of device the <see cref="USBController"/> is connected to.
        /// </summary>
        public NZXTDeviceType Type { get => _Type; }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentProductID { get => CurrProductID; }

        /// <summary>
        /// The <see cref="HIDDeviceID"/> of the <see cref="HidDevice"/> the <see cref="USBController"/> is connected to.
        /// </summary>
        public HIDDeviceID DeviceID { get => _ID; }

        /// <summary>
        /// The last <see cref="HidReport"/> received from the connected <see cref="HidDevice"/>.
        /// </summary>
        public HidReport LastReport { get => _LastReport; }

        /// <summary>
        /// Whether or not the <see cref="USBController"/> is currently connected to its <see cref="HidDevice"/>.
        /// </summary>
        public bool IsAttached { get => IsAttached; }

        
        /// <summary>
        /// Constructs a <see cref="USBController"/> and attempts to connect to a given <see cref="NZXTDeviceType"/>.
        /// </summary>
        /// <param name="Type"></param>
        public USBController(NZXTDeviceType Type) {
            this._Type = Type;
            ResolveDeviceID();
            Initialize();
        }

        /// <summary>
        /// Initializes the connection to a <see cref="HidDevice"/>.
        /// </summary>
        public void Initialize()
        {
            //HidDevice[] devices = HidDevices.Enumerate((int)_VendorID).ToArray();
            //for(int i = 0; i < devices.Length; i++)
            //{
            //    Console.WriteLine("Device-" + i + ": " + devices[i]);
            //}
            //_Device = HidDevices.Enumerate((int)_VendorID, (int)_ID).FirstOrDefault();

            string device_path = "\\\\?\\hid#vid_1e71&pid_2007#7&1f09b849&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}";
            _Device = HidDevices.Enumerate(device_path).FirstOrDefault();
            if (_Device == null)
            {
                Console.WriteLine("No devices found");
                throw new Exception("No devices found");
            }

            _Device.OpenDevice();

            _Device.Inserted += DeviceAttachedHandler;
            _Device.Removed += DeviceRemovedHandler;

            //Console.WriteLine("Reading Report...");
            //_Device.ReadReport(OnReport);

            var ReportDevice = Task.Run(async () => await _Device.ReadReportAsync());
            ReportDevice.ContinueWith(t => OnReport(ReportDevice.Result));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Report"></param>
        internal void OnReport(HidReport Report)
        {
            //Console.WriteLine("OnReport: Triggered");
            this._LastReport = Report;
            //_Device.ReadReport(OnReport);
            var NewReportDevice = Task.Run(async () => await _Device.ReadReportAsync());
            NewReportDevice.ContinueWith(t => OnReport(NewReportDevice.Result));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        public void SimulWrite(byte[][] Buffer)
        {
            foreach (byte[] command in Buffer)
            {
                _Device.Write(command);
            }

        }

        /// <summary>
        /// Disposes of the current <see cref="USBController"/> instance.
        /// </summary>
        public void Dispose()
        {
            _Device.CloseDevice();
        }

        /// <summary>
        /// Writes a given <paramref name="Buffer"/> to the <see cref="HidDevice"/> the <see cref="USBController"/> is connected to.
        /// </summary>
        /// <param name="Buffer"></param>
        public void Write(byte[] Buffer)
        {
            _Device.WriteAsync(Buffer);
        }

        /// <summary>
        /// Triggers when the <see cref="USBController"/> connects to a new device.
        /// </summary>
        internal void DeviceAttachedHandler()
        {
            this._IsAttached = true;
        }

        /// <summary>
        /// Triggers when the <see cref="HidDevice"/> the <see cref="USBController"/> is connected to disconnects.
        /// </summary>
        internal void DeviceRemovedHandler()
        {
            this._IsAttached = false;
        }

        /// <summary>
        /// Resolves the <see cref="HIDDeviceID"/> matching a <see cref="NZXTDeviceType"/>.
        /// </summary>
        private void ResolveDeviceID()
        {
            switch (_Type) // TODO: Find better way
            {
                // Kraken Devices
                case NZXTDeviceType.KrakenM:
                    this._ID = HIDDeviceID.KrakenM;
                    break;
                case NZXTDeviceType.KrakenM22:
                    this._ID = HIDDeviceID.KrakenM;
                    break;
                case NZXTDeviceType.KrakenX:
                    this._ID = HIDDeviceID.KrakenX;
                    break;
                case NZXTDeviceType.KrakenX42:
                    this._ID = HIDDeviceID.KrakenX;
                    break;
                case NZXTDeviceType.KrakenX52:
                    this._ID = HIDDeviceID.KrakenX;
                    break;
                case NZXTDeviceType.KrakenX62:
                    this._ID = HIDDeviceID.KrakenX;
                    break;
                case NZXTDeviceType.KrakenX72:
                    this._ID = HIDDeviceID.KrakenX;
                    break;

                // Hue Devices
                case NZXTDeviceType.Hue2:
                    this._ID = HIDDeviceID.Hue2;
                    break;
                case NZXTDeviceType.HueAmbient:
                    this._ID = HIDDeviceID.HueAmbient;
                    break;

                // Grid Devices
                case NZXTDeviceType.GridV3:
                    this._ID = HIDDeviceID.GridV3;
                    break;

                // Motherboards
                case NZXTDeviceType.N7:
                    this._ID = HIDDeviceID.N7;
                    break;
                case NZXTDeviceType.N7_Z390:
                    this._ID = HIDDeviceID.N7_Z390;
                    break;

                // Misc
                case NZXTDeviceType.H7Lumi:
                    this._ID = HIDDeviceID.H7Lumi;
                    break;
                case NZXTDeviceType.SmartDevice:
                    this._ID = HIDDeviceID.SmartDevice;
                    break;

                default:
                    throw new IncompatibleDeviceTypeException();
            }
        }
        

        public string[] ScanForDevices()
        {
            // TODO
            return new[] { "" };
        }
    }
}
