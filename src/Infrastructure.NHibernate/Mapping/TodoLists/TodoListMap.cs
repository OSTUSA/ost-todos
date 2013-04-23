using Core.Domain.Model.TodoLists;
using FluentNHibernate.Mapping;

namespace Infrastructure.NHibernate.Mapping.TodoLists
{
    public class TodoListMap : ClassMap<TodoList>
    {
        public TodoListMap()
        {
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Name).Not.Nullable();
            HasMany(x => x.Todos).KeyColumn("List_id").AsSet().Inverse().Cascade.AllDeleteOrphan();
            References(x => x.Owner, "Owner_id").Not.Nullable();
        }
    }
}