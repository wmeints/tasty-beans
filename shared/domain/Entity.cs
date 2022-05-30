namespace TastyBeans.Shared.Domain;

public abstract class Entity<T>
{
    public T Id { get; private set; }

    protected Entity(T id)
    {
        Id = id;
    }

    protected bool Equals(Entity<T> other)
    {
        return EqualityComparer<T>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Entity<T>)obj);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<T>.Default.GetHashCode(Id);
    }
}