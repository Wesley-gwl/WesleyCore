namespace WesleyCore.Domin.Abstractions
{
    /// <summary>
    /// 租户
    /// </summary>
    public interface IMustHaveTenant
    {
        int TenantId { get; set; }
    }
}