using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormAD
{
    public interface IAuthenticationSettings
    {
        string ClientId { get; set; }
        string TenantId { get; set; }
        string ResourceIdForServer1 { get; set; }
        string ResourceIdForServer2 { get; set; }
        string AADInstance { get; set; }
        string RedirectUri { get; set; }
    }
}
