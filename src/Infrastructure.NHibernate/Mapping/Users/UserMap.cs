using Core.Domain.Model;
using Core.Domain.Model.Users;
using FluentNHibernate.Mapping;

namespace Infrastructure.NHibernate.Mapping.Users
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Email).Unique().Not.Nullable();
            Map(x => x.Name).Not.Nullable();
            Map(x => x.Password).Not.Nullable();
            HasMany(x => x.Lists).KeyColumn("Owner_id").AsSet().Inverse().Cascade.AllDeleteOrphan();
        }
    }
}