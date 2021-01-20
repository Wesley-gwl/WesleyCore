using Microsoft.AspNetCore.Http;
using System.Linq;

namespace WesleyCore.Infrastructure
{
    /// <summary>
    /// 租户提供者
    /// </summary>
    public class TenantProvider : ITenantProvider
    {
        private int _tenantId;
        private IHttpContextAccessor _accessor;

        public TenantProvider(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        /// <summary>
        /// 获取租户id
        /// </summary>
        /// <returns></returns>
        public int GetTenantId()
        {
            if (_accessor.HttpContext != null)
            {
                int.TryParse(_accessor.HttpContext.Request.Headers["TenantId"], out _tenantId);
                if (_tenantId == 0)
                    int.TryParse(_accessor.HttpContext.User.Claims.FirstOrDefault(m => m.Type == "TenantId")?.Value, out _tenantId);
            }
            return _tenantId;
        }
    }
}