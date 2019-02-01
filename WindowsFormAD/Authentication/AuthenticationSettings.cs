using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormAD
{
    public class AuthenticationSettings : IAuthenticationSettings
    {
        public string ClientId { get; set; }
        public string TenantId { get; set; }
       
        public string AADInstance { get; set; }
        public string RedirectUri { get; set; }
        public string ResourceIdForServer1 { get; set; }
        public string ResourceIdForServer2 { get; set; }
    }
}
