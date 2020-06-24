namespace WesleyCore.Domin.Abstractions
{
    /// <summary>
    /// 多个主键
    /// </summary>
    public interface IEntity
    {
        object[] GetKeys();
    }

    /// <summary>
    /// 一个主键
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; }
    }
}