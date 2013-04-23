namespace Core.Domain.Model
{
    public interface IEntity<T> : IEntity where T : IEntity<T>
    {

    }

    public interface IEntity
    {
        bool IsNew();
    }
}