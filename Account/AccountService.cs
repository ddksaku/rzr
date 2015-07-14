using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Account
{
    public class AccountService
    {
        public static string GetLicenseVerificationUrl(string uniqueCode, string machineId)
        {
            return RzrConfiguration.RzrSiteLicenseVerificationUrl + "?uid=" + uniqueCode + "&mid=" + machineId;
        }
    }
}
