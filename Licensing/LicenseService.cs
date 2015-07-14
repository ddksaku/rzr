using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Xml.Linq;
using Rzr.Core.Account;

namespace Rzr.Core.Licensing
{
    public class LicenseService
    {
        public bool TryVerifyLicense(string uniqueCode)
        {
            XDocument verifyDoc = XDocument.Load(AccountService.GetLicenseVerificationUrl(uniqueCode, GetUniqueID()));
            XElement statusElement = verifyDoc.Descendants().FirstOrDefault(x => x.Name == "Status");
            return (statusElement != null && statusElement.Value == "OK");
        }

        public bool CheckLicense(string email, string password, string key)
        {
            // Need a post request which posts email and 
            return false;
        }

        protected string GetUniqueID()
        {
            try
            {
                return GetCPUInfo() + GetHDInfo();
            }
            catch
            {
                return "UnknownID";
            }
        }

        protected string GetCPUInfo()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                return mo.Properties["processorID"].Value.ToString();                
            }
            return String.Empty;
        }

        protected string GetHDInfo()
        {
            string drive = "C";
            ManagementObject dsk = new ManagementObject(
                @"win32_logicaldisk.deviceid=""" + drive + @":""");
            dsk.Get();
            return dsk["VolumeSerialNumber"].ToString();
        }
    }
}
