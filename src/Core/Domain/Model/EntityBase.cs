namespace Core.Domain.Model
{
    abstract public class EntityBase<T> : IEntity<T> where T : IEntity<T>
    {
        public virtual long Id { get; protected set; }

        public override bool Equals(object obj)
        {
            var other = obj as EntityBase<T>;
            if (other == null) return false;
            if (IsNew() && other.IsNew())
                return ReferenceEquals(this, other);
            return Id.Equals(other.Id);
        }

        private int? oldHashCode;
        public override int GetHashCode()
        {
            if (oldHashCode.HasValue)
                return oldHashCode.Value;
            if (IsNew())
            {
                oldHashCode = base.GetHashCode();
                return oldHashCode.Value;
            }
            return Id.GetHashCode();
        }

        public static bool operator ==(EntityBase<T> lhs, EntityBase<T> rhs)
        {
            return Equals(lhs, rhs);
        }

        public static bool operator !=(EntityBase<T> lhs, EntityBase<T> rhs)
        {
            return !Equals(lhs, rhs);
        }

        public virtual bool IsNew()
        {
            return Id.Equals(default(long));
        }
    }
}