namespace GameOff2026.Common.Data.Entity
{
    public abstract class BaseEntity<T>
    {
        public T value { get; protected set; }
        public virtual void Set(T t) => value = t;
        public virtual bool IsEqual(T t) => value.Equals(t);
    }
}