namespace WesleyCore.Infrastructure
{
    public interface ITenantProvider
    {
        /// <summary>
        /// 获取租户id
        /// </summary>
        /// <returns></returns>
        int GetTenantId();
    }
}