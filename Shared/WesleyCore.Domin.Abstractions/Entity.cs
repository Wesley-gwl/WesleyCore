namespace WesleyCore.Domin.Abstractions
{
    public abstract class Entity : IEntity
    {
        public abstract object[] GetKeys();
    }

    public abstract class Entity<TKey> : Entity, IEntity<TKey>
    {
        public virtual TKey Id { get; protected set; };

        public abstract object[] GetKeys();
    }
}