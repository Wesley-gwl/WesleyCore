namespace WesleyCore.Infrastruction
{
    /// <summary>
    /// 事务处理
    /// </summary>
    public class DomainContextTransactionBehavior<TRequest, TResponse> : TransactionBehavior<DomainContext>
    {
        public DomainContextTransactionBehavior(DomainContext context)
        {
        }
    }
}