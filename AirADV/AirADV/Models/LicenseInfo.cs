using System;
using AirADV.Services.Licensing;

namespace AirADV.Models
{
    public class LicenseInfo
    {
        public const string SERIAL_PREFIX = "AAD-";

        public string SerialKey { get; set; }
        public string OwnerName { get; set; }
        public DateTime ActivatedOn { get; set; }
        public string MachineID { get; set; }
        public string ProductName { get; set; }
        public string Version { get; set; }
        public bool IsActivated { get; set; }

        public LicenseInfo()
        {
            SerialKey = string.Empty;
            OwnerName = string.Empty;
            ActivatedOn = DateTime.MinValue;
            MachineID = string.Empty;
            ProductName = "AirADV";
            Version = "1.0.0";
            IsActivated = false;
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(SerialKey)) return false;
            if (!IsActivated) return false;
            if (!IsValidSerialFormat(SerialKey)) return false;
            if (!string.IsNullOrEmpty(MachineID) && MachineID != HardwareIdentifier.GetMachineID()) return false;
            return true;
        }

        public static bool IsValidSerialFormat(string serial)
        {
            if (string.IsNullOrEmpty(serial)) return false;
            if (!serial.StartsWith(SERIAL_PREFIX)) return false;
            string[] parts = serial.Split('-');
            if (parts.Length != 4) return false;
            if (parts[0] != SERIAL_PREFIX.TrimEnd('-')) return false;
            if (parts[1].Length != 4) return false;
            if (parts[2].Length != 4) return false;
            if (parts[3].Length != 4) return false;
            for (int i = 1; i < parts.Length; i++)
            {
                foreach (char c in parts[i])
                {
                    if (!char.IsLetterOrDigit(c)) return false;
                }
            }
            return true;
        }

        public override string ToString()
        {
            string displayName = !string.IsNullOrEmpty(OwnerName) ? OwnerName : SerialKey;
            return $"{displayName} - Activated: {ActivatedOn:dd/MM/yyyy HH:mm}";
        }
    }
}
